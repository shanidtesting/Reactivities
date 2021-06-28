using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class ActivityHandler
    {
        public class List : IRequest<List<Activity>> { }
        public class Delete : IRequest
        {
            public Guid Id { get; set; }
        }

        public class DeleteHandler : IRequestHandler<Delete>
        {
            private readonly DataContext _context;

            public DeleteHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Delete request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);
                _context.Remove(activity);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }

        public class ListHandler : IRequestHandler<List, List<Activity>>
        {
            private readonly DataContext _context;
            
            public ListHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<Activity>> Handle(List request, CancellationToken cancellationToken)
            {
                return await _context.Activities.ToListAsync();
            }
        }
    }
}