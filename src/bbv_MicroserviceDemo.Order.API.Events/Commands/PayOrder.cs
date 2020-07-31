
namespace bbv_MicroserviceDemo.Order.API.Events.Commands
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Common;
    using bbv_MicroserviceDemo.Common.Contants;
    using bbv_MicroserviceDemo.Common.Enums;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class PayOrder
    {
        public class Command : IRequest<ApiResult<Result>>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public bool IsSuccess { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id)
                    .NotNull().WithMessage("Id must not be null")
                    .NotEmpty().WithMessage("Id must not be empty");
            }
        }

        public class CommandHandler : IRequestHandler<Command, ApiResult<Result>>
        {
            private readonly IRepository<Order> _repository;

            public CommandHandler(IRepository<Order> repository)
            {
                _repository = repository;
            }

            public async Task<ApiResult<Result>> Handle(Command command, CancellationToken cancellationToken)
            {
                var order = _repository
                    .GetAll()
                    .FirstOrDefault(x => x.Id == command.Id);

                if (order == null)
                    return ApiResult<Result>.Fail(MessageContants.NotFound);

                order.OrderState = (int)OrderStatus.Paid;
                var result = await _repository.UpdateAsync(order);

                return result != null ?
                    ApiResult<Result>.Success(new Result { IsSuccess = true })
                    : ApiResult<Result>.Fail(MessageContants.UpdateFailed);
            }
        }
    }
}
