
namespace bbv_MicroserviceDemo.Order.API.Events.Tests.Queries
{
    using bbv_MicroserviceDemo.Order.API.Events.Queries;
    using FluentValidation.TestHelper;
    using System;
    using Xunit;

    public class GetOrderByIdValidatorTests
    {
        private readonly GetOrderById.QueryValidator _validator;

        public GetOrderByIdValidatorTests()
        {
            _validator = new GetOrderById.QueryValidator();
        }

        [Fact]
        public void Id_must_not_be_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Id, Guid.Empty).WithErrorMessage("Id must not be empty");
        }
    }
}
