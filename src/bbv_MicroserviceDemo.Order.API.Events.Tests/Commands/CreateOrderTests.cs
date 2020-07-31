
namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Commands
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Common.Enums;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;
    using System;
    using System.Linq;
    using Xunit;

    public class CreateOrderTests : TestWithSqlite
    {
        private readonly IMapper _mapper;
        private readonly Repository<Order> _repository;

        public CreateOrderTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(typeof(Events.Commands.CreateOrder.Profile));
            });

            _mapper = config.CreateMapper();
            _repository = new Repository<Order>(_context);
        }

        [Fact]
        public async void CreateOrder_should_create_new_order()
        {
            //Arrange
            var command = new Events.Commands.CreateOrder.Command
            {
                CustomerGuid = Guid.NewGuid(),
                OrderState = (int)OrderStatus.Unpaid,
                CustomerFullName = "phu phan"
            };
            var sut = new Events.Commands.CreateOrder.CommandHandler(_repository, _mapper);
            Assert.Equal(0, _repository.GetAll().Count());

            //Act
            var result = await sut.Handle(command, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, _repository.GetAll().Count());
            Assert.Equal("phu phan", _repository.GetAll().First().CustomerFullName);
            Assert.Equal((int)OrderStatus.Unpaid, _repository.GetAll().First().OrderState);
            Assert.IsType<Guid>(result.Payload.Id);
        }
    }
}
