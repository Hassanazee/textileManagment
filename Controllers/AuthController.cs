using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using textileManagment.Bussiness.Dtos.Request;
using textileManagment.Bussiness.Dtos.Responce;
using textileManagment.Bussiness.Interface;
using textileManagment.Controllers.Helper;
using textileManagment.Domain;

namespace textileManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
        //BaseController<AuthController, IAppUserService, LoginReq, LoginRes >
    {

        private readonly IAppUserService _appUserService;

        /* public AuthController(ILogger<AuthController> logger, IAppUserService appUserService) : base(logger,
          appUserService)
         {
         }*/

        public AuthController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginReq)
        {
            var result = await _appUserService.LoginUser(loginReq);
            return result;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginReq registerReq)
        {
            var result = await _appUserService.RegisterUser(registerReq);
            return result;
        }

        [Authorize(Roles = "SuperAdmin")] 
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            var result = await _appUserService.DeleteUser(HttpContext);
            return result;
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AppRole role , string userEmail)
        {
            var res = await _appUserService.AssignRole(role , userEmail);
            return res;
        }
    }
}

