using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;

namespace TestProject.API.ApiExtensions
{
    public static class ServiceCollectionExtensions
    {

        public static class PolicyHandler
        {
            private static readonly Random Jitterer = new Random();

            public static IAsyncPolicy<HttpResponseMessage> WaitAndRetry(int retryCount = 5) =>
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => (int)msg.StatusCode >= 400)
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(1.1, retryAttempt)) + TimeSpan.FromMilliseconds(Jitterer.Next(0, 100)),
                        onRetry: (outcome, timespan, retryAttempt, context) =>
                        {
                            Log.Information("Delaying for {0}ms, then making retry {1}.", timespan.TotalMilliseconds, retryAttempt);
                        });

            public static IAsyncPolicy<HttpResponseMessage> Timeout(int seconds = 5) =>
                Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds));
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            return services;
        }
    }
}
