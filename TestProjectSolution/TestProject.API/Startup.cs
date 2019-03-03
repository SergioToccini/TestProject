using System.Globalization;
using AutoMapper;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.SystemConsole.Themes;
using TestProject.API.ApiExtensions;
using TestProject.API.Filters;
using TestProject.Domain.Entities;
using TestProject.Domain.Repositories;
using TestProject.Infrastructure.Database;
using TestProject.Infrastructure.Repositories;

namespace TestProject.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        private readonly string _defaultConnection;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _environment = environment;
            _configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();

            _defaultConnection = _configuration.GetConnectionString("DefaultConnection");

            var columnOptions = new ColumnOptions();
            // Do include the log event data as JSON.
            columnOptions.Store.Add(StandardColumn.LogEvent);

            // Serilog config
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Application", "TestProject API")
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level} {EnvironmentUserName}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .WriteTo.MSSqlServer(
                    connectionString: _defaultConnection,
                    tableName: nameof(Logs),
                    LogEventLevel.Debug,
                    batchPostingLimit: 100,
                    period: null,
                    columnOptions: columnOptions)
                .CreateLogger();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsTest())
            {
                services.AddDbContextPool<DefaultContext>(options => options.UseInMemoryDatabase("testprojectdb"));
            }
            else
            {
                services.AddDbContextPool<DefaultContext>(options => options.UseSqlServer(_defaultConnection));
            }

            // Set culture to inflector
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en");

            // Logging
            services.AddLogging(builder => builder.AddSerilog());
            Serilog.Debugging.SelfLog.Enable(Log.Error);

            services.AddAutoMapper();
            services.AddSwaggerDocumentation();

            services.AddScoped<IReadOnlyRepository, ReadOnlyRepository>();
            services.AddScoped<IWriteOnlyRepository, WriteOnlyRepository>();

            services.AddMvc(config =>
                {
                    config.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (_environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            app.UseDbInitializer(_configuration, _environment);

            app.UseMvc();
        }
    }
}
