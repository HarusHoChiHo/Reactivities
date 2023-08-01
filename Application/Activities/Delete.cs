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
        public class Command : IRequest<Unit> 
        { 
            public Guid Id { get; set; }
        }

        public class Handle : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _context;

            public Handle(DataContext context, IMapper mapper)
            {
                this._context = context;
            }
            async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(new object[] { request.Id, cancellationToken }, cancellationToken: cancellationToken);

                if (activity != null) 
                {
                    _context.Remove(activity);

                    _ = await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
        }
    }
}
