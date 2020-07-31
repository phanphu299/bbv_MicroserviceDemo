namespace bbv_MicroserviceDemo.Order.API.Events.Commands
{
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using bbv_MicroserviceDemo.Common;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;

    public class CreateOrder
    {
        public class Command : IRequest<ApiResult<Result>>
        {
            public Guid CustomerGuid { get; set; }
            public string CustomerFullName { get; set; }
            public int OrderState { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.OrderState)
                    .InclusiveBetween(1, 2)
                    .WithMessage("OrderState must be 1 or 2");

                RuleFor(p => p.CustomerFullName)
                    .NotNull().WithMessage("CustomerFullName must not be null")
                    .NotEmpty().WithMessage("CustomerFullName must not be empty")
                    .MaximumLength(50).WithMessage("CustomerFullName must be 50 characters or fewer");

                RuleFor(p => p.CustomerGuid)
                    .NotNull().WithMessage("CustomerGuid must not be null")
                    .NotEmpty().WithMessage("CustomerGuid must not be empty");
            }
        }

        public class CommandHandler : IRequestHandler<Command, ApiResult<Result>>
        {
            private readonly IRepository<Order> _repository;
            private readonly IMapper _mapper;

            public CommandHandler(IRepository<Order> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ApiResult<Result>> Handle(Command command, CancellationToken cancellationToken)
            {
                Order order = _mapper.Map<Command, Order>(command, opt => opt.AfterMap((src, dest) => dest.Id = new Guid()));
                await _repository.AddAsync(order);

                return ApiResult<Result>.Success(new Result
                {
                    Id = order.Id,
                });
            }
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Command, Order>();
            }
        }
    }
}
