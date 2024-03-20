using Microsoft.AspNetCore.Mvc;
using textileManagment.Bussiness.Interface;
using textileManagment.Data.Interface;
using textileManagment.Domain.Helper;
using textileManagment.Entities.Base.IBase;

namespace textileManagment.Bussiness.Implementations
{
    public class BaseService<TReq, TRes, TRepository, T> : IBaseService<TReq, TRes>
     where TRepository : class, IBaseRepo<T> where T : IMinBase
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly TRepository Repository;

        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Repository = UnitOfWork.GetRepository<TRepository>();
        }

        public virtual async Task<IActionResult> GetAll(Pagination pagination)
        {
            try
            {
                var (pag, data) = await Repository.GetAll(pagination);
                return data.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
              
                return e.GetBaseException().Message.BadRequest();
            }
        }

        public virtual async Task<IActionResult> Get(long id)
        {
            try
            {
                var entity = await Repository.Get(id);
                return entity.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError();
            }
        }

        public virtual async Task<IActionResult> Add(TReq reqModel)
        {
            try
            {
                var ss = await Repository.Add((T)(reqModel as IMinBase ??
                 throw new InvalidOperationException("Conversion to IMinBase Failed. Make sure there's Id and CreatedDate properties.")));

                return ss.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError();
            }
        }

        public virtual async Task<IActionResult> Update(TReq reqModel)
        {
            try
            {
                var res = await Repository.Update((T)(reqModel as IMinBase ??
                                                       throw new InvalidOperationException(
                                                           "Conversion to IMinBase Failed. Make sure there's Id and CreatedDate properties.")),
                    null);

                return res.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError();
            }
        }

        public virtual async Task<IActionResult> Delete(long id)
        {
            try
            {
                var res = await Repository.Delete(id);
                return res.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError();
            }
        }


    }
}
