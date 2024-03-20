using Microsoft.AspNetCore.Mvc;
using textileManagment.Domain.Helper;

namespace textileManagment.Bussiness.Interface
{
    public interface IBaseService<in TReq, out TRes>
    {
        public Task<IActionResult> GetAll(Pagination pagination);
        public Task<IActionResult> Get(long id);
        public Task<IActionResult> Add(TReq reqModel);
        public Task<IActionResult> Update(TReq reqModel);
        public Task<IActionResult> Delete(long id);

    }
}
