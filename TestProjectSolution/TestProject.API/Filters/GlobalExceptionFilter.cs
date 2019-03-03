using System;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace TestProject.API.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _env;
        public GlobalExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            try
            {
                var status = HttpStatusCode.InternalServerError;

                var ex = context.Exception;
                var innerEx = ex.InnerException;

                var exType = ex.GetType();
                var exMessage = ex.Message;
                var exStack = _env.IsDevelopment() == true ? ex.StackTrace.Split("\r\n") : new string[0];

                if (exType == typeof(UnauthorizedAccessException))
                {
                    status = HttpStatusCode.Unauthorized;
                }
                else if (exType == typeof(DbUpdateException))
                {
                    if (innerEx is SqlException)
                    {
                        Log.Error(innerEx.Message);
                        return;
                    }
                }

                context.ExceptionHandled = true;

                var response = context.HttpContext.Response;
                response.StatusCode = (int)status;
                response.ContentType = "application/json";
                object error;

                if (innerEx == null)
                {
                    error = new
                    {
                        error = new
                        {
                            errorType = exType.Name,
                            errorMessage = exMessage,
                            stack = exStack
                        }
                    };
                }
                else
                {
                    error = new
                    {
                        error = new
                        {
                            errorType = exType.Name,
                            errorMessage = exMessage,
                            innerErrorType = innerEx.GetType().Name,
                            innerErrorMessage = innerEx.Message,
                            stack = exStack
                        }
                    };
                }

                var result = JsonConvert.SerializeObject(error);

                Log.Error(result);

                await response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GlobalExceptionFilter!!!");
            }
        }
    }
}
