

namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Queries
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Repositories;
    using bbv_MicroserviceDemo.Domains.Entities;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;
    using System.Linq;
    using bbv_MicroserviceDemo.Common.Enums;

    public class GetOrderByCustomerIdTests : TestWithSqlite
    {
        private readonly IMapper _mapper;
        private readonly Repository<Order> _repository;
        public GetOrderByCustomerIdTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(typeof(Events.Queries.GetOrderByCustomerId.Profile));
            });

            _mapper = config.CreateMapper();
            _repository = new Repository<Order>(_context);

            var customers = new List<Order>
            {
                new Order
                {
                    CustomerGuid = Guid.Parse("9f35b48d-cb87-4783-bfdb-21e36012930a"),
                    CustomerFullName = "a b",
                    Id = Guid.NewGuid(),
                    OrderState = (int)OrderStatus.Unpaid
                },
                new Order
                {
                    CustomerGuid = Guid.Parse("654b7573-9501-436a-ad36-94c5696ac28f"),
                    CustomerFullName = "c d",
                    Id = Guid.NewGuid(),
                    OrderState = (int)OrderStatus.Paid
                },
                new Order
                {
                    CustomerGuid = Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066"),
                    CustomerFullName = "e f",
                    Id = Guid.NewGuid(),
                    OrderState = (int)OrderStatus.Unpaid
                }
            };

            _context.AddRange(customers);
            _context.SaveChanges();
        }

        [Fact]
        public async void GetOrderByCustomerId_should_return_customer_orders()
        {
            //Arrange
            var query = new Events.Queries.GetOrderByCustomerId.Query 
            {
                CustomerId = Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066")
            };
            var sut = new Events.Queries.GetOrderByCustomerId.QueryHandler(_repository, _mapper);

            //Act
            var result = await sut.Handle(query, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("e f", result.Payload.Orders.First().CustomerFullName);
        }

        [Fact]
        public async void GetOrderByCustomerId_should_return_empty_if_customer_not_existing()
        {
            //Arrange
            var query = new Events.Queries.GetOrderByCustomerId.Query
            {
                CustomerId = Guid.NewGuid()
            };
            var sut = new Events.Queries.GetOrderByCustomerId.QueryHandler(_repository, _mapper);

            //Act
            var result = await sut.Handle(query, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Payload.Orders);
        }
    }
}
