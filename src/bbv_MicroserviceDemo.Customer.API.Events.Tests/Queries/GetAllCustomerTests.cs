namespace bbv_MicroserviceDemo.Customer.API.Events.Tests
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Xunit;

    public class GetAllCustomerTests : TestWithSqlite
    {
        private readonly IMapper _mapper;
        private readonly Repository<Customer> _repository;

        public GetAllCustomerTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(typeof(Queries.GetAllCustomer.Profile));
            });

            _mapper = config.CreateMapper();
            _repository = new Repository<Customer>(_context);

            var customers = new List<Customer>
            {
                new Customer 
                { 
                    Id = Guid.Parse("9f35b48d-cb87-4783-bfdb-21e36012930a"), 
                    Age = 10,
                    Birthday = DateTime.Now,
                    FirstName = "a",
                    LastName = "b"
                },
                new Customer
                {
                    Id = Guid.Parse("654b7573-9501-436a-ad36-94c5696ac28f"),
                    Age = 20,
                    Birthday = DateTime.Now,
                    FirstName = "c",
                    LastName = "d"
                },
                new Customer
                {
                    Id = Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066"),
                    Age = 30,
                    Birthday = DateTime.Now,
                    FirstName = "e",
                    LastName = "f"
                }
            };

            _context.AddRange(customers);
            _context.SaveChanges();
        }

        [Fact]
        public async void GetAllCustomer_should_return_all_customers()
        {
            //Arrange
            var query = new Queries.GetAllCustomer.Query();
            var sut = new Queries.GetAllCustomer.QueryHandler(_repository, _mapper);

            //Act
            var result = await sut.Handle(query, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Payload.Customers.Count);
        }
    }
}
