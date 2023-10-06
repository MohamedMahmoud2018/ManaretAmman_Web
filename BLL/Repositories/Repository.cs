using BusinessLogicLayer.Common;
using BusinessLogicLayer.Services.ProjectProvider;
using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private IProjectProvider _projectProvider;
        private DbContext _context;
        private DbSet<TEntity> dbSet;

        public Repository(DbContext context, IProjectProvider projectProvider)
        {
            _projectProvider = projectProvider;
            this._context    = context;
            this.dbSet       = context.Set<TEntity>();
        }
        public Repository(DbContext context)
        {
            this._context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query.AsNoTracking();
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query.AsNoTracking(); ;
        }

        public virtual IEnumerable<TEntity> ExecStoreProcedure(string sql, params object[] parameters)
        {
            var result = _context.Set<TEntity>().FromSqlRaw(sql, parameters);
            // _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return result;
        }
        public virtual IEnumerable<TEntity> ExecQuery(string query)
        {
            var result = _context.Set<TEntity>().FromSqlRaw(query);
            return result;
        }

        public virtual TEntity GetById(object id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
                _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return query.AsNoTracking().FirstOrDefault(filter);
        }

        public virtual TEntity GetLastOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return query.LastOrDefault(filter);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public virtual void Insert(ICollection<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Update(entity);
            //_context.Entry(entity).State = EntityState.Modified;
        }
        
        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity is IBaseEntity entityBaseEntity)
            {
                entityBaseEntity.ModificationDate = DateTime.Now;
            }
            dbSet.Update(entity);
        }
        public virtual void Update(ICollection<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(string code)
        {
            TEntity entityToDelete = dbSet.Find(code);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public virtual void DeleteRange(List<TEntity> entity)
        {
            //_context.RemoveRange(entity);
            dbSet.RemoveRange(entity);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        #region AccordingProject
        public virtual IQueryable<TEntity> PQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            if (!typeof(IMustHaveProject).IsAssignableFrom(typeof(TEntity)))
            {
                throw new InvalidOperationException("TEntity must implement IMustHaveProject.");
            }

            var projectId = _projectProvider.GetProjectId();
            Expression<Func<TEntity, bool>> projectFilter = e => ((IMustHaveProject)e).ProjectID == projectId;

            IQueryable<TEntity> query = _context.Set<TEntity>().Where(projectFilter);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includes is not null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);
            }

            return query;
        }

        public virtual async Task PInsertAsync(TEntity entity)
        {
            if (!(entity is IMustHaveProject projectEntity))
            {
                throw new InvalidOperationException("TEntity must implement IMustHaveProject.");
            }
            if(entity is IBaseEntity entityBaseEntity)
            {
                entityBaseEntity.CreationDate =  DateTime.Now;
                entityBaseEntity.CreatedBy = _projectProvider.UserId();
            }
            var projectId = _projectProvider.GetProjectId();

            projectEntity.ProjectID = projectId;

            await InsertAsync(entity);
        }

        public virtual async Task PUpdateAsync(TEntity entity)
        {
            if (!(entity is IMustHaveProject projectEntity))
            {
                throw new InvalidOperationException("TEntity must implement IMustHaveProject.");
            }
            if (entity is IBaseEntity entityBaseEntity)
            {
                entityBaseEntity.ModificationDate = DateTime.Now;
                entityBaseEntity.ModifiedBy = _projectProvider.UserId();
            }
            var projectId = _projectProvider.GetProjectId();

            projectEntity.ProjectID = projectId;

            await UpdateAsync(entity);
        }
        #endregion
    }

}