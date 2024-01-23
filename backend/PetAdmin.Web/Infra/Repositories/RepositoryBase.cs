using Microsoft.EntityFrameworkCore;
using PetAdmin.Web.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PetAdmin.Web.Infra.Repositories
{
    public class RepositoryBase<TEntity>  where TEntity : EntityGuid
    {
        private readonly PetContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(PetContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public PetContext GetContext()
        {
            return _context;
        }

        #region GetById
        public TEntity GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public TEntity GetById(long id, IList<Expression<Func<TEntity, object>>> includes)
        {
            var query = _dbSet.Where(e => e.Id == id);

            foreach (var include in includes)
                query = query.Include(include);

            return query.FirstOrDefault();
        }
        #endregion  

        #region GetAll
        public IEnumerable<TEntity> GetAll
        (
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy,
            bool asNoTracking = true
        )
        {
            if (asNoTracking)
                return new List<TEntity>
                (
                    _dbSet.AsNoTracking().OrderBy(orderBy).Where(where).ToList()
                );

            return new List<TEntity>
            (
                _dbSet.OrderBy(orderBy).Where(where).ToList()
            );
        }

        public IEnumerable<TEntity> GetAll
        (
            Expression<Func<TEntity, bool>> where,
            bool asNoTracking = true
        )
        {
            if (asNoTracking)
                return new List<TEntity>
                (
                    _dbSet.AsNoTracking().Where(where).ToList()
                );

            return new List<TEntity>
            (
                _dbSet.Where(where).ToList()
            );
        }

        public IEnumerable<TEntity> GetAll
        (
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy,
            IList<Expression<Func<TEntity, object>>> includes,
            bool asNoTracking = true
        )
        {
            IQueryable<TEntity> query;

            if (asNoTracking)
                query = _dbSet.AsNoTracking().OrderBy(orderBy).Where(where);
            else
                query = _dbSet.OrderBy(orderBy).Where(where);

            foreach (var include in includes)
                query = query.Include(include);

            return query.ToList();
        }

        public BaseListDto<TEntity> GetAll
        (
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy,
            int skip,
            int take,
            bool asNoTracking = true
        )
        {
            var find = _dbSet.Where(where);
            var databaseCount = find.Count();
            if (asNoTracking)
                return new BaseListDto<TEntity>
                {
                    Result = find.AsNoTracking().OrderBy(orderBy).Skip(skip).Take(take).ToList(),
                    Total = databaseCount
                };

            return new BaseListDto<TEntity>
            {
                Result = find.OrderBy(orderBy).Skip(skip).Take(take).ToList(),
                Total = databaseCount
            };
        }
        #endregion  

        public TEntity Add(TEntity entity)
        {
            if (entity.Uid == null || entity.Uid == Guid.Empty)
                entity.Uid = Guid.NewGuid();

            _context.Add(entity);
            return entity;
        }

        public void AddRange(List<TEntity> entityList)
        {
            entityList.ForEach(e => e.Uid = Guid.NewGuid());
            _context.AddRange(entityList);
        }

        public TEntity Update(TEntity entity)
        {
            _context.Update(entity);
            return entity;
        }

        public void UpdateRange(List<TEntity> entityList)
        {
            _context.UpdateRange(entityList);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public void DeleteRange(List<TEntity> entity)
        {
            _context.RemoveRange(entity);
        }
    }
}
