using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestProject.Domain.Models;
using TestProject.Tests.Base;
using Xunit;

namespace TestProject.Tests.IntegrationTests
{
    public class StockController_IntegrationTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        public StockController_IntegrationTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_OK_ResultNotNull()
        {
            // Arrange
            var url = $"/api/stock";

            // Act
            var response = await _fixture.Client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsAsync<IEnumerable<StockGetModel>>();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(200, (int)response.StatusCode);
            Assert.NotNull(jsonResult);
        }
    }
}
