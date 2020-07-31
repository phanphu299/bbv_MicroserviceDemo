namespace bbv_MicroserviceDemo.Customer.API.Events.Tests
{
    using bbv_MicroserviceDemo.Customer.API.DataAccess;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using System;
    public abstract class TestWithSqlite : IDisposable
    {
        private const string _inMemoryConnectionString = "DataSource=:memory:;Foreign Keys=False";
        private readonly SqliteConnection _connection;

        protected readonly CustomerContext _context;

        protected TestWithSqlite()
        {
            _connection = new SqliteConnection(_inMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<CustomerContext>()
                    .UseSqlite(_connection)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .Options;
            _context = new CustomerContext(options);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
