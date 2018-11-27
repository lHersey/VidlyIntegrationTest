using System;
using System.Net;
using System.Threading.Tasks;
using CleanVidly.Controllers.Resources;
using CleanVidly.Core.Entities;
using CleanVidly.IntegrationTest.Extensions;
using CleanVidly.IntegrationTest.Helpers;
using CleanVidly.Persistance;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace CleanVidly.IntegrationTest.Controllers.Categories
{
    public class CategoriesControllerGetByIdTests : IClassFixture<Request<Startup>>, IClassFixture<DbContextFactory>, IDisposable
    {
        private readonly Request<Startup> request;
        private readonly ITestOutputHelper output;
        private readonly CleanVidlyDbContext context;
        public CategoriesControllerGetByIdTests(ITestOutputHelper output, Request<Startup> request, DbContextFactory contextFactory)
        {
            this.output = output;
            this.request = request;
            this.context = contextFactory.Context;
        }

        public void Dispose()
        {
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();
        }

        [Fact]
        public async Task ShouldReturn_Cateogory_ById()
        {
            var category = new Category() { Description = "Category1" };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var response = await request.Get($"/api/categories/{category.Id}");
            var body = await response.BodyAs<KeyValuePairResource>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            body.Description.Should().Be(category.Description);
        }

        [Fact]
        public async Task ShouldReturn_404NotFound_ById()
        {
            var response = await request.Get("/api/categories/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}