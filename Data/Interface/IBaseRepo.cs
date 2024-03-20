using textileManagment.Domain.Helper;
using textileManagment.Entities.Base.IBase;

namespace textileManagment.Data.Interface
{
    public interface IBaseRepo<TEntity> : IDisposable where TEntity : IMinBase
    {

        public Task<(Pagination, IList<TEntity>)> GetAll(Pagination pagination,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null);
        public Task<TEntity> Get(long id);
        public Task<TEntity> Add(TEntity model);
        public Task<TEntity> Update(TEntity model, Func<TEntity, TEntity>? func);
        public Task<bool> Delete(long id);
    }
}
