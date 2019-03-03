using Microsoft.AspNetCore.Hosting;

namespace TestProject.API.ApiExtensions
{
    public static class HostingEnvironmentExtensions
    {
        public const string TestEnvironment = "Test";
        public static bool IsTest(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(TestEnvironment);
        }
    }
}
