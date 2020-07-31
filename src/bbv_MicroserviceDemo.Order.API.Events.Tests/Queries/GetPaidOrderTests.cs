
namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Queries
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Common.Enums;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class GetPaidOrderTests : TestWithSqlite
    {
        private readonly IMapper _mapper;
        private readonly Repository<Order> _repository;
        public GetPaidOrderTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(typeof(Events.Queries.GetPaidOrder.Profile));
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
        public async void GetPaidOrder_should_return_paid_orders()
        {
            //Arrange
            var query = new Events.Queries.GetPaidOrder.Query();
            var sut = new Events.Queries.GetPaidOrder.QueryHandler(_repository, _mapper);

            //Act
            var result = await sut.Handle(query, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("c d", result.Payload.Orders.First().CustomerFullName);
        }
    }
}
