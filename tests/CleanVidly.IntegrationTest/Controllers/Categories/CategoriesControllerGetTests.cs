using System;
using System.Net;
using System.Net.Http;
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
    public class CategoriesControllerGetTests : IClassFixture<Request<Startup>>, IClassFixture<DbContextFactory>, IDisposable
    {
        private readonly Request<Startup> request;
        private readonly ITestOutputHelper output;
        private readonly CleanVidlyDbContext context;
        public CategoriesControllerGetTests(ITestOutputHelper output, Request<Startup> request, DbContextFactory contextFactory)
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
        public async Task ShouldReturn_AllCategories()
        {
            await context.Categories.AddRangeAsync(new Category[]{
                new Category(){ Description = "Category1" },
                new Category(){ Description = "Category2" }
            });

            await context.SaveChangesAsync();

            var response = await request.Get("/api/categories");
            var body = await response.BodyAs<KeyValuePairResource[]>();


            response.StatusCode.Should().Be(HttpStatusCode.OK);
            body.Length.Should().Be(2);
            body.Should().ContainSingle(gn => gn.Description == "Category1");
            body.Should().ContainSingle(gn => gn.Description == "Category2");
        }


    }
}