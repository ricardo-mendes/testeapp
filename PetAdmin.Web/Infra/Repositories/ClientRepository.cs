using Microsoft.EntityFrameworkCore;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace PetAdmin.Web.Infra.Repositories
{
    public class ClientRepository : RepositoryBase<Client>
    {
        private readonly PetContext _context;

        public ClientRepository(PetContext context)
            : base(context)
        {
            _context = context;
        }

        public ICollection<ClientLocationDto> GetAllWithLocation()
        {
            var dbBaseQuery = from client in _context.Clients.AsNoTracking()
                              join location in _context.Locations.AsNoTracking() on client.LocationId equals location.Id
                              select new ClientLocationDto
                              {
                                  ClientId = client.Id,
                                  Latitude = location.Latitude,
                                  Longitue = location.Longitue
                              };

            return dbBaseQuery.ToList();
        }
    }
}
