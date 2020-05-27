﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BShop.API.Application.Categories.Commands.Create;
using BShop.API.Application.Categories.Models;
using BShop.API.Application.Products.Commands.Put;
using BShop.API.FunctionalTests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace BShop.API.FunctionalTests
{
    public class CategoriesApiTests : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private const string Uri = "api/categories";
        private readonly TestWebApplicationFactory<Startup> _factory;

        public CategoriesApiTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_Success()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(Uri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsAsync<IEnumerable<CategoryDto>>();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Get_Success()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{Uri}/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsAsync<CategoryDto>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_NoAuthentication_Unauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();
            var command = new CreateCategoryCommand { Name = "Test no authentication" };

            // Act
            var response = await client.PostAsJsonAsync(Uri, command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_Authenticated_Success()
        {
            // Arrange
            var client = _factory.CreateAuthenticatedClient();
            var command = new CreateCategoryCommand { Name = "Created" };

            // Act
            var response = await client.PostAsJsonAsync(Uri, command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var result = await response.Content.ReadAsAsync<CategoryDto>();
            result.Should().BeEquivalentTo(new CategoryDto
            {
                Name = "Created"
            }, s => s.Excluding(p => p.Id));
        }

        [Fact]
        public async Task Put_Authenticated_Success()
        {
            // Arrange
            var client = _factory.CreateAuthenticatedClient();
            var command = new PutProductCommand { Name = "Changed" };

            // Act
            var response = await client.PutAsJsonAsync($"{Uri}/1", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_Authenticated_Success()
        {
            // Arrange
            var client = _factory.CreateAuthenticatedClient();

            // Act
            var response = await client.DeleteAsync($"{Uri}/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
