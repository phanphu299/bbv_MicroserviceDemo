
namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Commands
{
    using bbv_MicroserviceDemo.Common.Enums;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class PayOrderTests : TestWithSqlite
    {
        private readonly Repository<Order> _repository;
        public PayOrderTests()
        {
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
        public async void PayOrder_should_change_order_state()
        {
            //Arrange
            var command = new Events.Commands.PayOrder.Command
            {
                Id = Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066")
            };
            var sut = new Events.Commands.PayOrder.CommandHandler(_repository);

            //Act
            var result = await sut.Handle(command, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)OrderStatus.Paid, _repository.GetAll().First().OrderState);
        }
    }
}
