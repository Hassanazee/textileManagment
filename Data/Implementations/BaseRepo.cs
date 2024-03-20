using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using textileManagment.Data.Interface;
using textileManagment.Domain.Context;
using textileManagment.Domain.Helper;
using textileManagment.Entities.Base.IBase;

namespace textileManagment.Data.Implementations
{
    public abstract class BaseRepo<TEntity> : IBaseRepo<TEntity> where TEntity : class, IGeneralBase
    {
        protected readonly DbSet<TEntity> DbSet;
        protected readonly ApplicationDbContext DbContext;
        public readonly HttpContext HttpContext;

        protected BaseRepo(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            DbSet = dbContext.Set<TEntity>();
            DbContext = dbContext;
            HttpContext = httpContextAccessor?.HttpContext ??
                          throw new NotImplementedException(
                              "HttpContextAccessor Cannot be Null. Verify whether it's properly Injected or not.");
        }

        public virtual async Task<TEntity> Add(TEntity model)
        {
            await DbSet.AddAsync(model);
            return model;
        }

        public virtual async Task<bool> Delete(long id)
        {
            var found = await DbSet
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDelete != true);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(TEntity)} not found for Id:{id}");
            }

            found.IsDelete = true;
            DbContext.SaveChangesAsync();
            return true;
        }

      /*  public virtual async Task<TEntity> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data?.Adapt<TEntity>() ?? throw new InvalidOperationException();
        }*/

        public virtual async Task<TEntity> Get(long id)
        {
            var data = await DbSet.FirstOrDefaultAsync(c => !c.IsDelete && c.Id == id);
            return data;
        }
        public virtual async Task<(Pagination, IList<TEntity>)> GetAll(Pagination pagination,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc)
        {
            var total = 0;
            var totalPages = 0;

            var res = await (includeFunc?.Invoke(DbSet) ?? DbSet).Where(f => f.IsDelete != true)
                .Paginate(pagination.PageIndex, pagination.PageSize, ref total, ref totalPages).ToListAsync();

            pagination = pagination.Combine(total, totalPages);
            return (pagination, res);
        }

    

        public virtual async Task<TEntity> Update(TEntity model, Func<TEntity, TEntity>? func)
        {
            var found = await DbSet
                .FirstOrDefaultAsync(c => !c.IsDelete && c.Id == model.Id);
            if (found == null)
            {
                throw new EntityNotFoundException($"{nameof(TEntity)} not found with Id: {model.Id}");
            }

            //Execute the Update-EntityMapping Function
            if (func != null)
                found = func(found);

            return found;
        }

        public void Dispose()
        {
            DbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
