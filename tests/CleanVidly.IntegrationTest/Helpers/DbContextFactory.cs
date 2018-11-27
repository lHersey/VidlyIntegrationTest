using System;
using CleanVidly.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace CleanVidly.IntegrationTest.Helpers
{
    public class DbContextFactory : IDisposable
    {
        public CleanVidlyDbContext Context { get; private set; }
        public DbContextFactory()
        {
            var dbBuilder = GetContextBuilderOptions<CleanVidlyDbContext>("vidly_db");

            Context = new CleanVidlyDbContext(dbBuilder.Options);
            Context.Database.Migrate();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public CleanVidlyDbContext GetRefreshContext()
        {
            var dbBuilder = GetContextBuilderOptions<CleanVidlyDbContext>("vidly_db");
            Context = new CleanVidlyDbContext(dbBuilder.Options);

            return Context;
        }

        private DbContextOptionsBuilder<CleanVidlyDbContext> GetContextBuilderOptions<T>(string connectionStringName)
        {
            var connectionString = ConfigurationSingleton.GetConfiguration().GetConnectionString(connectionStringName);
            var contextBuilder = new DbContextOptionsBuilder<CleanVidlyDbContext>();
            var servicesCollection = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();

            contextBuilder.UseSqlServer(connectionString).UseInternalServiceProvider(servicesCollection);

            return contextBuilder;
        }
    }
}