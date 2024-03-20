using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using textileManagment.Bussiness.Dtos.Request;
using textileManagment.Bussiness.Dtos.Responce;
using textileManagment.Bussiness.Interface;
using textileManagment.Data.Implementations;
using textileManagment.Domain;
using textileManagment.Domain.Helper;
using textileManagment.Entities;

namespace textileManagment.Bussiness.Implementations
{
    public class AppUserService : BaseService<LoginReq, LoginRes, AppUserRepo, AppUser>, IAppUserService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly RoleManager<AppRole> _roleManager;
        public AppUserService(IUnitOfWork unitOfWork, IConfiguration configuration, SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor,
            IWebHostEnvironment environment,
            UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
            : base(unitOfWork)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _environment = webHostEnvironment;
            _contextAccessor = contextAccessor;
            _roleManager = roleManager;
            _environment = environment;
            _userManager = userManager;
        }

        public async Task<IActionResult> DeleteUser(HttpContext context)
        {

            /*var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                      var result = await _userManager.DeleteAsync(userId);
                return result.Succeeded ? Ok() : BadRequest(result.Errors);*/

            throw new NotImplementedException();
        }


        public async Task<IActionResult> RegisterUser(LoginReq reqModel)
        {
            var transaction = await UnitOfWork.BeginTransactionAsync();
            try
            {
                var user = new AppUser
                {
                    Email = reqModel.Email,
                    UserName = reqModel.Username,
                    FirstName = reqModel.Firstname,
                    LastName = reqModel.Lastname,
                    CreatedDate = DateTime.UtcNow,
                    Role = reqModel.Role,
                    CreatedById = _contextAccessor.HttpContext.GetUserId(),
                };

                var existUser = await UnitOfWork.Context.AppUsers.FirstOrDefaultAsync(c => c.Email == reqModel.Email && c.IsDelete != true);
                if (existUser != null)
                {
                    return ("User Already exist with the same Email.").BadRequest();
                }

                var result = await _signInManager.UserManager.CreateAsync(user, reqModel.Password);
                if (!result.Succeeded)
                {
                    return string.Join(", ", result.Errors.Select(f => f.Description)).BadRequest();
                }


                // await _userManager.AddToRoleAsync(user, "YourRole");

                await UnitOfWork.SaveAsync();
                await UnitOfWork.CommitTransactionAsync(transaction);

                LoginRes res = new LoginRes
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Role = user.Role,
                };

                return res.Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.HandleError();
            }
        }

        public async Task<IActionResult> LoginUser(LoginRequest req)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);

            if (user != null)
            {
              
                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        return new JsonResult(new { Message = "Email not confirmed. Please check your email." });
                    }
                }

                if (await _userManager.CheckPasswordAsync(user, req.Password))
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                    var token = GenerateJwtToken(user);
                    return new JsonResult(new { Token = token });
                }
                else
                {
                    await _userManager.AccessFailedAsync(user);
                    return new JsonResult(new { Message = "Invalid credentials" });
                }
            }
            else
            {
                return new JsonResult(new { Message = "User not found" });
            }
        }
        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Role),
            new Claim(ClaimTypes.Name, user.UserName),

            new Claim(ClaimTypes.Role, "SuperAdmin"),
            new Claim(ClaimTypes.Role, "OrgAdmin"),
            new Claim(ClaimTypes.Role, "OrgStaff"),
            new Claim(ClaimTypes.Role, "Supplier"),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.Role , "Customer"),
        };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IActionResult> AssignRole(AppRole role, string userEmail) 
        {
            var user = await _userManager.FindByEmailAsync(userEmail); 

            if (user == null)
            {
                return new JsonResult(new { Message = "User not found" });
            }
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                return new JsonResult(new { Message = "Role not found" });
            }
            if (!await _userManager.IsInRoleAsync(user, role.Name))
            {
                await _userManager.AddToRoleAsync(user, role.Name);
                return new JsonResult(new { Message = $"Role '{role.Name}' assigned to user '{user.Email}' successfully" });
            }
            else
            {
                return new JsonResult(new { Message = $"User '{user.Email}' is already in role '{role.Name}'" });
            }
        }

    }
}


