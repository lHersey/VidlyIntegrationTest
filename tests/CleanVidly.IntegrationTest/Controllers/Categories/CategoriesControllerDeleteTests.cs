using System;
using System.Linq;
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
    public class CategoriesControllerDeleteTests : IClassFixture<Request<Startup>>, IClassFixture<DbContextFactory>, IDisposable
    {
        private readonly Request<Startup> request;
        private readonly ITestOutputHelper output;
        private readonly CleanVidlyDbContext context;
        private int Id;

        private string Token;

        public CategoriesControllerDeleteTests(Request<Startup> request, DbContextFactory contextFactory, ITestOutputHelper output)
        {
            this.request = request;
            this.context = contextFactory.Context;
            this.output = output;

            var category = new Category { Description = "Description" };

            context.Add(category);
            context.SaveChanges();

            Id = category.Id;

            context = contextFactory.GetRefreshContext();

            var user = new User()
            {
                Email = "",
                Name = "",
                Lastname = "",
                Id = 1
            };

            Token = request.Jwt.GenerateToken(user);
        }

        public void Dispose()
        {
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();
        }

        public Task<HttpResponseMessage> Exec() =>
            request.AddAuth(Token).Delete($"/api/categories/{Id}");


        [Fact]
        public async Task ShouldReturns_404NotFound_IfInvalidId()
        {

            Id = -1;

            var result = await Exec();

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldDelete_TheCategory_IfValidId()
        {
            await Exec();

            var categoryInDb = context.Categories.FirstOrDefault(c => c.Id == Id);

            categoryInDb.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturn_401NotFound_IfNoTokenProvided()
        {
            Token = "";

            var res = await Exec();

            res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }


    }
}