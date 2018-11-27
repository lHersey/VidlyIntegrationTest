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

namespace CleanVidly.IntegrationTest.Controllers.Roles
{
    public class RolesControllerGetTests : IClassFixture<Request<Startup>>, IClassFixture<DbContextFactory>, IDisposable
    {
        private readonly Request<Startup> request;
        private readonly ITestOutputHelper output;
        private readonly CleanVidlyDbContext context;
        public RolesControllerGetTests(ITestOutputHelper output, Request<Startup> request, DbContextFactory contextFactory)
        {
            this.output = output;
            this.request = request;
            this.context = contextFactory.Context;
        }

        public void Dispose()
        {
            context.Roles.RemoveRange(context.Roles);
            context.SaveChanges();
        }

        [Fact]
        public async Task ShouldReturn_AllCategories()
        {
            await context.Roles.AddRangeAsync(new Role[]{
                new Role{ Description = "Role1" },
                new Role{ Description = "Role2" }
            });

            await context.SaveChangesAsync();

            var response = await request.Get("/api/roles");
            var body = await response.BodyAs<KeyValuePairResource[]>();


            response.StatusCode.Should().Be(HttpStatusCode.OK);
            body.Length.Should().Be(2);
            body.Should().ContainSingle(gn => gn.Description == "Role1");
            body.Should().ContainSingle(gn => gn.Description == "Role2");
        }


    }
}