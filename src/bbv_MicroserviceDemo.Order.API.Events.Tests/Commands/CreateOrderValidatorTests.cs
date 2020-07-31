
namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Commands
{
    using bbv_MicroserviceDemo.Order.API.Events.Commands;
    using FluentValidation.TestHelper;
    using System;
    using Xunit;

    public class CreateOrderValidatorTests
    {
        private readonly CreateOrder.CommandValidator _validator;

        public CreateOrderValidatorTests()
        {
            _validator = new CreateOrder.CommandValidator();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(110)]
        public void OrderState_must_be_1_or_2(int state)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.OrderState, state).WithErrorMessage("OrderState must be 1 or 2");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void OrderState_WhenIn1Or2_ShouldNotHaveValidationError(int state)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.OrderState, state);
        }

        [Theory]
        [InlineData("phu")]
        [InlineData("john")]
        public void CustomerFullName_WhenLongerThanOneCharacterAndFewerThan50Characters_ShouldNotHaveValidationError(string customerFullName)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.CustomerFullName, customerFullName);
        }

        [Theory]
        [InlineData(null)]
        public void CustomerFullName_must_not_be_null(string customerFullName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.CustomerFullName, customerFullName).WithErrorMessage("CustomerFullName must not be null");
        }

        [Theory]
        [InlineData("")]
        public void CustomerFullName_must_not_be_empty(string customerFullName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.CustomerFullName, customerFullName).WithErrorMessage("CustomerFullName must not be empty");
        }

        [Theory]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus feugiat lacinia metus")]
        public void CustomerFullName_must_be_50characters_or_fewer(string customerFullName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.CustomerFullName, customerFullName).WithErrorMessage("CustomerFullName must be 50 characters or fewer");
        }

        [Fact]
        public void CustomerGuid_must_not_be_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.CustomerGuid, Guid.Empty).WithErrorMessage("CustomerGuid must not be empty");
        }
    }
}
