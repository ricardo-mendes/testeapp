using Microsoft.EntityFrameworkCore;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetAdmin.Web.Infra.Repositories
{
    public class ScheduleItemClientRepository : RepositoryBase<ScheduleItemClient>
    {
        private readonly PetContext _context;

        public ScheduleItemClientRepository(PetContext context)
            : base(context)
        {
            _context = context;
        }

        public IList<ScheduleItemClient> GetAllByScheduleItemId(long scheduleItemId)
        {
            var list = _context.ScheduleItemClients
                .Include(s => s.Client)
                .ThenInclude(c => c.EmployeeList)
                .Include(c => c.Client)
                .ThenInclude(c => c.Location)
                .Where(s => s.ScheduleItemId == scheduleItemId && s.IsActive)
                .ToList();

            return list;
        }
    }
}
