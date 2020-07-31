
namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Commands
{
    using bbv_MicroserviceDemo.Order.API.Events.Commands;
    using FluentValidation.TestHelper;
    using System;
    using Xunit;

    public class PayOrderValidatorTests
    {
        private readonly PayOrder.CommandValidator _validator;

        public PayOrderValidatorTests()
        {
            _validator = new PayOrder.CommandValidator();
        }

        [Fact]
        public void Id_must_not_be_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty).WithErrorMessage("Id must not be empty");
        }
    }
}
