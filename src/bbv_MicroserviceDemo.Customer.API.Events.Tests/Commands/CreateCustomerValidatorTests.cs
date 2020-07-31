
namespace bbv_MicroserviceDemo.Customer.API.Events.Tests.Commands
{
    using bbv_MicroserviceDemo.Customer.API.Events.Commands;
    using FluentValidation.TestHelper;
    using Xunit;

    public class CreateCustomerValidatorTests
    {
        private readonly CreateCustomer.CommandValidator _validator;

        public CreateCustomerValidatorTests()
        {
            _validator = new CreateCustomer.CommandValidator();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Age_must_greater_than0(int age)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Age, age).WithErrorMessage("Age must be greater than 0");
        }

        [Theory]
        [InlineData(50)]
        [InlineData(10)]
        public void Age_greater_than0_ShouldNotHaveValidationError(int age)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Age, age);
        }

        [Theory]
        [InlineData("phu")]
        [InlineData("john")]
        public void FirstName_WhenLongerThanOneCharacterAndFewerThan50Characters_ShouldNotHaveValidationError(string firstName)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.FirstName, firstName);
        }

        [Theory]
        [InlineData("phan")]
        [InlineData("doe")]
        public void LastName_WhenLongerThanOneCharacterAndFewerThan50Characters_ShouldNotHaveValidationError(string lastName)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.LastName, lastName);
        }

        [Theory]
        [InlineData(null)]
        public void FirstName_must_not_be_null(string firstName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.FirstName, firstName).WithErrorMessage("FirstName must not be null");
        }

        [Theory]
        [InlineData("")]
        public void FirstName_must_not_be_empty(string firstName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.FirstName, firstName).WithErrorMessage("FirstName must not be empty");
        }

        [Theory]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus feugiat lacinia metus")]
        public void FirstName_must_be_50characters_or_fewer(string firstName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.FirstName, firstName).WithErrorMessage("FirstName must be 50 characters or fewer");
        }

        [Theory]
        [InlineData(null)]
        public void LastName_must_not_be_null(string lastName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.LastName, lastName).WithErrorMessage("LastName must not be null");
        }

        [Theory]
        [InlineData("")]
        public void LastName_must_not_be_empty(string lastName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.LastName, lastName).WithErrorMessage("LastName must not be empty");
        }

        [Theory]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus feugiat lacinia metus")]
        public void LastName_must_be_50characters_or_fewer(string lastName)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.LastName, lastName).WithErrorMessage("LastName must be 50 characters or fewer");
        }
    }
}
