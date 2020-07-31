
namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Queries
{
    using bbv_MicroserviceDemo.Order.API.Events.Queries;
    using FluentValidation.TestHelper;
    using System;
    using Xunit;

    public class GetOrderByCustomerIdValidatorTests
    {
        private readonly GetOrderByCustomerId.QueryValidator _validator;

        public GetOrderByCustomerIdValidatorTests()
        {
            _validator = new GetOrderByCustomerId.QueryValidator();
        }

        [Fact]
        public void Id_must_not_be_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.CustomerId, Guid.Empty).WithErrorMessage("Id must not be empty");
        }
    }
}
