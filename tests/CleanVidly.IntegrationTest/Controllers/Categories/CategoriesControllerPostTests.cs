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
using FluentValidation.Results;
using Xunit;
using Xunit.Abstractions;

namespace CleanVidly.IntegrationTest.Controllers.Categories
{
    public class CategoriesControllerPostTests : IClassFixture<Request<Startup>>, IClassFixture<DbContextFactory>, IDisposable
    {
        private readonly Request<Startup> request;
        private readonly ITestOutputHelper output;
        private readonly CleanVidlyDbContext context;
        private string Description;
        private string Token;
        public CategoriesControllerPostTests(ITestOutputHelper output, Request<Startup> request, DbContextFactory contextFactory)
        {
            this.output = output;
            this.request = request;
            this.context = contextFactory.Context;

            Description = "Valid Category";

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

        public Task<HttpResponseMessage> Exec() => request.AddAuth(Token).Post("/api/categories", new { Description = Description });

        [Fact]
        public async Task ShouldSave_Category_IfInputValid()
        {
            await Exec();
            var categoryInDb = context.Categories.FirstOrDefault(c => c.Description == Description);
            categoryInDb.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldRetuns_Category_IfInputValid()
        {
            var res = await Exec();
            var body = await res.BodyAs<KeyValuePairResource>();

            body.Id.Should().BeGreaterThan(0, "Id should by set by EF");
            body.Description.Should().Be(Description, "Is the same description sended on Exec();");
        }

        [Fact]
        public async Task ShouldRetun_BadRequest400_IfDescription_LessThanFourCharacters()
        {
            Description = "a";

            var res = await Exec();
            var body = await res.BodyAs<ValidationErrorResource>();

            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Errors.Should().ContainKey("Description");
        }

        [Fact]
        public async Task ShouldRetun_BadRequest400_IfDescription_MoreThanSixtyFourCharacters()
        {

            Description = string.Join("a", new char[66]);

            var res = await Exec();
            var body = await res.BodyAs<ValidationErrorResource>();

            res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            body.Errors.Should().ContainKey("Description");
        }

        [Fact]
        public async Task ShouldRetun_Unauthorized401_IfNoTokenProvided()
        {
            Token = "";

            var res = await Exec();

            res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }


    }
}