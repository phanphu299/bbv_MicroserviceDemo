
namespace bbv_MicroserviceDemo.Customer.API.Events.Tests
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;
    using System;
    using System.Linq;
    using Xunit;

    public class CreateCustomerTests : TestWithSqlite
    {
        private readonly IMapper _mapper;
        private readonly Repository<Customer> _repository;

        public CreateCustomerTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(typeof(Events.Commands.CreateCustomer.Profile));
            });

            _mapper = config.CreateMapper();
            _repository = new Repository<Customer>(_context);
        }

        [Fact]
        public async void CreateCustomer_should_create_new_customer()
        {
            //Arrange
            var command = new Events.Commands.CreateCustomer.Command 
            { 
                Age = 10,
                Birthday = DateTime.Now,
                FirstName = "john",
                LastName = "doe",
            };
            var sut = new Events.Commands.CreateCustomer.CommandHandler(_repository, _mapper);
            Assert.Equal(0, _repository.GetAll().Count());

            //Act
            var result = await sut.Handle(command, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, _repository.GetAll().Count());
            Assert.Equal("john", _repository.GetAll().First().FirstName);
            Assert.IsType<Guid>(result.Payload.Id);
        }

    }
}
