using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PetAdmin.Web.Infra.Repositories
{
    public class PetLoverLocationClientRepository
    {
        private readonly PetContext _context;
        private readonly DbSet<PetLoverLocationClient> _dbSet;

        public PetLoverLocationClientRepository(PetContext context)
        {
            _context = context;
            _dbSet = _context.Set<PetLoverLocationClient>();
        }

        public IEnumerable<PetLoverLocationClient> GetAll
        (
            Expression<Func<PetLoverLocationClient, bool>> where
        )
        {
            return new List<PetLoverLocationClient>
            (
                _dbSet.Where(where).ToList()
            );
        }

        public void AddRange(List<PetLoverLocationClient> entityList)
        {
            _context.BulkInsert(entityList);
        }

        public void DeleteRange(List<PetLoverLocationClient> entity)
        {
            _context.RemoveRange(entity);
        }
    }
}
