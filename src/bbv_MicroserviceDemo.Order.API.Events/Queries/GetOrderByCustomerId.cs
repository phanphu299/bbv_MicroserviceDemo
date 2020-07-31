﻿
namespace bbv_MicroserviceDemo.Order.API.Events.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using bbv_MicroserviceDemo.Common;
    using bbv_MicroserviceDemo.Domains.Entities;
    using bbv_MicroserviceDemo.Repositories;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetOrderByCustomerId
    {
        public class Query : IRequest<ApiResult<Result>>
        {
            public Guid CustomerId { get; set; }
        }

        public class Result
        {
            public List<Order> Orders { get; set; }
            public class Order
            {
                public Guid Id { get; set; }
                public int OrderState { get; set; }
                public Guid CustomerGuid { get; set; }
                public string CustomerFullName { get; set; }
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(p => p.CustomerId)
                    .NotNull().WithMessage("Id must not be null")
                    .NotEmpty().WithMessage("Id must not be empty");
            }
        }

        public class QueryHandler : IRequestHandler<Query, ApiResult<Result>>
        {
            private readonly IRepository<Order> _repository;
            private readonly IMapper _mapper;

            public QueryHandler(IRepository<Order> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<ApiResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var orders = await _repository.GetAll()
                    .AsNoTracking()
                    .Where(x => x.CustomerGuid == request.CustomerId)
                    .ProjectTo<Result.Order>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return ApiResult<Result>.Success(new Result
                {
                    Orders = orders
                });
            }
        }

        public class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Order, Result.Order>();

            }
        }
    }
}
