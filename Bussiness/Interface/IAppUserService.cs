using Microsoft.AspNetCore.Mvc;
using textileManagment.Bussiness.Dtos.Request;
using textileManagment.Bussiness.Dtos.Responce;
using textileManagment.Domain;

namespace textileManagment.Bussiness.Interface
{
    public interface IAppUserService : IBaseService<LoginReq , LoginRes>
    {
        Task<IActionResult> LoginUser(LoginRequest req);
        Task<IActionResult> RegisterUser(LoginReq req);
        Task<IActionResult> DeleteUser(HttpContext context);
        Task<IActionResult> AssignRole(AppRole role, string userEmail);
    }
}
