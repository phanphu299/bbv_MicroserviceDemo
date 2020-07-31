
namespace bbv_MicroserviceDemo.Customer.API.Events.Tests.Commands
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Message.Sender.Sender;
    using bbv_MicroserviceDemo.Repositories;
    using Moq;
    using System;
    using System.Linq;
    using Xunit;

    public class UpdateCustomerTests : TestWithSqlite
    {
        private readonly IMapper _mapper;
        private readonly Repository<Customer> _repository;

        public UpdateCustomerTests()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(typeof(Events.Commands.UpdateCustomer.Profile));
            });

            _mapper = config.CreateMapper();
            _repository = new Repository<Customer>(_context);

            var customer = new Customer
            {
                Id = Guid.Parse("9f35b48d-cb87-4783-bfdb-21e36012930a"),
                Age = 10,
                Birthday = DateTime.Now,
                FirstName = "a",
                LastName = "b"
            };

            _context.Add(customer);
            _context.SaveChanges();
        }

        [Fact]
        public async void UpdateCustomer_should_update_customer()
        {
            //Arrange
            var command = new Events.Commands.UpdateCustomer.Command
            {
                Age = 20,
                Birthday = DateTime.Now,
                FirstName = "john",
                LastName = "doe",
                Id = Guid.Parse("9f35b48d-cb87-4783-bfdb-21e36012930a")
            };

            var customerUpdateSender = new Mock<ICustomerUpdateSender>();

            var sut = new Events.Commands.UpdateCustomer.CommandHandler(_repository, _mapper, customerUpdateSender.Object);
            Assert.Equal(1, _repository.GetAll().Count());
            Assert.Equal("a", _repository.GetAll().First().FirstName);
            Assert.Equal("b", _repository.GetAll().First().LastName);
            Assert.Equal(10, _repository.GetAll().First().Age);

            //Act
            var result = await sut.Handle(command, default);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, _repository.GetAll().Count());
            Assert.Equal("john", _repository.GetAll().First().FirstName);
            Assert.Equal("doe", _repository.GetAll().First().LastName);
            Assert.Equal(20, _repository.GetAll().First().Age);
        }
    }
}
