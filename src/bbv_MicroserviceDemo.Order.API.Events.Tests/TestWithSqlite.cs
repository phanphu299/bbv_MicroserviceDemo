
namespace bbv_MicroserviceDemo.Order.API.Events.Tests
{
    using bbv_MicroserviceDemo.Order.API.DataAccess;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using System;

    public abstract class TestWithSqlite : IDisposable
    {
        private const string _inMemoryConnectionString = "DataSource=:memory:;Foreign Keys=False";
        private readonly SqliteConnection _connection;

        protected readonly OrderContext _context;

        protected TestWithSqlite()
        {
            _connection = new SqliteConnection(_inMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<OrderContext>()
                    .UseSqlite(_connection)
                    .Options;
            _context = new OrderContext(options);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
