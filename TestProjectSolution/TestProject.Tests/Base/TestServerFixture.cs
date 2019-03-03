using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestProject.API;
using TestProject.API.ApiExtensions;

namespace TestProject.Tests.Base
{
    public class TestServerFixture : IDisposable
    {
        public readonly HttpClient Client;
        public readonly IServiceProvider ServiceProvider;
        public readonly IConfiguration Configuration;
        public readonly IHostingEnvironment Environment;
        public readonly IApplicationBuilder ApplicationBuilder;
        private readonly TestServer _testServer;

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                   .UseContentRoot(GetContentRootPath())
                   .UseEnvironment("Test")
                   .UseStartup<Startup>();

            _testServer = new TestServer(builder);
            ServiceProvider = _testServer.Host.Services;

            Configuration = ServiceProvider.GetService<IConfiguration>();
            Environment = ServiceProvider.GetService<IHostingEnvironment>();
            ApplicationBuilder = new ApplicationBuilder(ServiceProvider);

            ApplicationBuilder.UseDbInitializer(Configuration, Environment);

            Client = _testServer.CreateClient();
        }

        private string GetContentRootPath()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "TestProject.API"));
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
