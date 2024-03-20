using Microsoft.EntityFrameworkCore;
using textileManagment.Data.Interface;
using textileManagment.Domain.Context;
using textileManagment.Entities;

namespace textileManagment.Data.Implementations
{
    public class AppUserRepo : BaseRepo<AppUser>, IAppUserRepo
    {
        public AppUserRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

    }
}