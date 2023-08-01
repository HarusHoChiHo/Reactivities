using AutoMapper;
using Domain;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest<Unit> 
        { 
            public Activity Activity {  get; set; }
        }

        public class Hanlder : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Hanlder(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                _mapper.Map(request.Activity, activity);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;

            }
        }
    }
}
