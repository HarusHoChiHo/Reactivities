using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>> 
        { 
            public Guid Id { get; set; }
        }

        public class Handle : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handle(DataContext context, IMapper mapper)
            {
                this._context = context;
            }
            async Task<Result<Unit>> IRequestHandler<Command, Result<Unit>>.Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(new object[] { request.Id, cancellationToken }, cancellationToken: cancellationToken);

                /*
                if (activity == null) 
                {
                    return null;
                }
                */
                _context.Remove(activity);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete the activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
