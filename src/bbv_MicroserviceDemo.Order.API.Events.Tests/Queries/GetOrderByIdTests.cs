
namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Queries
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Common.Enums;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class GetOrderByIdTests : TestWithSqlite
    {
        private readonly IMapper _mapper;
        private readonly Repository<Order> _repository;
        public GetOrderByIdTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(typeof(Events.Queries.GetOrderById.Profile));
            });

            _mapper = config.CreateMapper();
            _repository = new Repository<Order>(_context);

            var customers = new List<Order>
            {
                new Order
                {
                    Id = Guid.Parse("9f35b48d-cb87-4783-bfdb-21e36012930a"),
                    CustomerFullName = "a b",
                    CustomerGuid = Guid.NewGuid(),
                    OrderState = (int)OrderStatus.Unpaid
                },
                new Order
                {
                    Id = Guid.Parse("654b7573-9501-436a-ad36-94c5696ac28f"),
                    CustomerFullName = "c d",
                    CustomerGuid = Guid.NewGuid(),
                    OrderState = (int)OrderStatus.Paid
                },
                new Order
                {
                    Id = Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066"),
                    CustomerFullName = "e f",
                    CustomerGuid = Guid.NewGuid(),
                    OrderState = (int)OrderStatus.Unpaid
                }
            };

            _context.AddRange(customers);
            _context.SaveChanges();
        }

        [Fact]
        public async void GetOrderById_should_return_an_order()
        {
            //Arrange
            var query = new Events.Queries.GetOrderById.Query 
            {
                Id = Guid.Parse("654b7573-9501-436a-ad36-94c5696ac28f")
            };
            var sut = new Events.Queries.GetOrderById.QueryHandler(_repository, _mapper);

            //Act
            var result = await sut.Handle(query, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("c d", result.Payload.CustomerFullName);
        }

        [Fact]
        public async void GetOrderById_should_return_null_if_not_existing()
        {
            //Arrange
            var query = new Events.Queries.GetOrderById.Query
            {
                Id = Guid.NewGuid()
            };
            var sut = new Events.Queries.GetOrderById.QueryHandler(_repository, _mapper);

            //Act
            var result = await sut.Handle(query, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Payload);
        }
    }
}
