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
    public class CategoriesControllerPutTests : IClassFixture<Request<Startup>>, IClassFixture<DbContextFactory>, IDisposable
    {
        private readonly Request<Startup> request;
        private readonly ITestOutputHelper output;
        private readonly CleanVidlyDbContext context;

        private int Id;
        private string Description;
        private string Token;
        public CategoriesControllerPutTests(ITestOutputHelper output, Request<Startup> request, DbContextFactory contextFactory)
        {
            this.output = output;
            this.request = request;
            this.context = contextFactory.Context;

            Description = "New Description";

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
            request.AddAuth(Token).Put($"/api/categories/{Id}", new { Description = Description });


        [Fact]
        public async Task ShouldUpdate_ExistingCategory_IfExistAndValid()
        {
            await Exec();

            var categoryInDb = context.Categories.FirstOrDefault(c => c.Id == Id);

            categoryInDb.Description.Should().Be("New Description");
        }


        [Fact]
        public async Task ShouldReturn_UpdatedCategory_IfExistAndValid()
        {
            var res = await Exec();
            var body = await res.BodyAs<KeyValuePairResource>();

            body.Description.Should().Be("New Description");
            body.Id.Should().Be(Id);
        }

        [Fact]
        public async Task ShouldReturn_400BadRequest_IdDescriptionLessThanFourCharacters()
        {
            Description = "a";

            var res = await Exec();
            var body = await res.BodyAs<ValidationErrorResource>();

            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Errors.Should().ContainKey("Description");
        }

        [Fact]
        public async Task ShouldReturn_400BadRequest_IdDescriptionMoreThanSixtyFourCharacters()
        {
            Description = string.Join("a", new char[66]);

            var res = await Exec();
            var body = await res.BodyAs<ValidationErrorResource>();

            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Errors.Should().ContainKey("Description");
        }

        [Fact]
        public async Task ShouldReturn_404NotFound_IfIdIsInvalid()
        {
            Id = -1;

            var res = await Exec();

            res.StatusCode.Should().Be(HttpStatusCode.NotFound);
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