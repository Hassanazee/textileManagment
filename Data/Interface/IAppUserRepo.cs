using textileManagment.Domain.Helper;
using textileManagment.Entities;

namespace textileManagment.Data.Interface
{
    public interface IAppUserRepo : IBaseRepo<AppUser>
    {
        public Task<AppUser> Add(AppUser model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task<(Pagination, IList<AppUser>)> GetAll(Pagination pagination, Func<IQueryable<AppUser>, IQueryable<AppUser>>? func = null)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> Update(AppUser model, Func<AppUser, AppUser>? func)
        {
            throw new NotImplementedException();
        }
    }
}
