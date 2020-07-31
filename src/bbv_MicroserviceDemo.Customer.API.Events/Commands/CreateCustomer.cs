namespace bbv_MicroserviceDemo.Customer.API.Events.Commands
{
    using AutoMapper;
    using bbv_MicroserviceDemo.Common;
    using bbv_MicroserviceDemo.Repositories;
    using bbv_MicroserviceDemo.Domains.Entities;
    using FluentValidation;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateCustomer
    {
        public class Command : IRequest<ApiResult<Result>>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? Birthday { get; set; }
            public int? Age { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.FirstName)
                    .NotNull().WithMessage("FirstName must not be null")
                    .NotEmpty().WithMessage("FirstName must not be empty")
                    .MaximumLength(50).WithMessage("FirstName must be 50 characters or fewer");

                RuleFor(p => p.LastName)
                    .NotNull().WithMessage("LastName must not be null")
                    .NotEmpty().WithMessage("LastName must not be empty")
                    .MaximumLength(50).WithMessage("LastName must be 50 characters or fewer");

                RuleFor(p => p.Age).GreaterThan(0).WithMessage("Age must be greater than 0");
            }
        }

        public class CommandHandler : IRequestHandler<Command, ApiResult<Result>>
        {
            private readonly IRepository<Customer> _repository;
            private readonly IMapper _mapper;

            public CommandHandler(IRepository<Customer> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ApiResult<Result>> Handle(Command command, CancellationToken cancellationToken)
            {
                Customer customer = _mapper.Map<Command, Customer>(command, opt => opt.AfterMap((src, dest) => dest.Id = new Guid()));
                await _repository.AddAsync(customer);

                return ApiResult<Result>.Success(new Result
                {
                    Id = customer.Id,
                });
            }
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Command, Customer>();
            }
        }
    }
}
