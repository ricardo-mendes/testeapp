using Microsoft.EntityFrameworkCore;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Dto.PetLover;
using PetAdmin.Web.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace PetAdmin.Web.Infra.Repositories
{
    public class PetLoverRepository : RepositoryBase<PetLover>
    {
        private readonly PetContext _context;

        public PetLoverRepository(PetContext context)
            : base(context)
        {
            _context = context;
        }

        public ICollection<MyPetLoverResultDto> GetAllMyPetLoverResultDtoByClientId(long clientId)
        {
            var dbBaseQuery = from petLover in _context.PetLovers.AsNoTracking()
                              join pet in _context.Pets.AsNoTracking() on petLover.Id equals pet.PetLoverId
                              join location in _context.Locations.AsNoTracking() on petLover.LocationId equals location.Id
                              where petLover.ClientId == clientId
                              orderby petLover.Id, pet.Name
                              select new MyPetLoverResultDto
                              {
                                  PetLoverId = petLover.Id,
                                  PetId = pet.Id,
                                  PetName = pet.Name,
                                  PetLoverName = petLover.Name,
                                  PhoneNumber = petLover.PhoneNumber,
                                  Address = location.StreetName + ", " + location.StreetNumber,
                                  Complement = location.Complement,
                                  Neighborhood = location.Neighborhood,
                                  IsClub = pet.IsClub
                              };

            return dbBaseQuery.ToList();
        }

        public ICollection<PetLoverLocationDto> GetAllWithLocation()
        {
            var dbBaseQuery = from petLover in _context.PetLovers.AsNoTracking()
                              join location in _context.Locations.AsNoTracking() on petLover.LocationId equals location.Id
                              select new PetLoverLocationDto
                              {
                                  PetLoverId = petLover.Id,
                                  Latitude = location.Latitude,
                                  Longitue = location.Longitue
                              };

            return dbBaseQuery.ToList();
        }

        public PetLoverLocationDto GetWithLocationById(long petLoverId)
        {
            var dbBaseQuery = from petLover in _context.PetLovers.AsNoTracking()
                              join location in _context.Locations.AsNoTracking() on petLover.LocationId equals location.Id
                              where petLover.Id == petLoverId
                              select new PetLoverLocationDto
                              {
                                  PetLoverId = petLover.Id,
                                  Latitude = location.Latitude,
                                  Longitue = location.Longitue
                              };

            return dbBaseQuery.FirstOrDefault();
        }
    }
}
