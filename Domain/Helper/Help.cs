using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace textileManagment.Domain.Helper
{
    public static class Help
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int page, int perPage, ref int total,
            ref int pages)
        {
            if (pages < 0) throw new ArgumentOutOfRangeException(nameof(pages));

            total = source.Count();
            pages = (int)Math.Ceiling(total / (double)perPage);
            return source.Skip((page - 1) * perPage).Take(perPage);
        }

        public static string CreateMd5(string input)
        {
            
            using var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static Pagination Combine(this Pagination pagination, int total, int totalPages)
        {
            pagination.Total = total < 0 ? 0 : total;
            pagination.TotalPages = totalPages < 0 ? 0 : totalPages;
            return pagination;
        }
        public static OkObjectResult Ok<T>(this T obj, string message = "Executed Successfully!")
        {
            // return new OkObjectResult((miscD, obj, message));
            return new OkObjectResult(Response<T>.Ok(message, obj));
        }


        public static BadRequestObjectResult BadRequest<T>(this T obj)
        {
            // return new BadRequestObjectResult((obj));
            return new BadRequestObjectResult(Response<T>.BadRequest(obj?.ToString()));
        }

        public static IActionResult HandleError(this Exception e)
        {
            if (e is EntityNotFoundException)
            {
                return e.GetBaseException().HandleError();

                //  return e.GetBaseException().Message.NotFound()

            }

            return e.GetBaseException().Message.BadRequest();
        }
        public static long GetUserId(this IHttpContextAccessor httpContext)
        {
            return long.Parse((httpContext.HttpContext?.User.Claims).FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name).Value);

            /* return long.Parse(httpContext.HttpContext?.User.Claims
                 .FirstOrDefault(c => c.Type ==
                 //JwtRegisteredClaimNames.
                 Name)?.Value ?? "0");*/
        }

        public static (string, long) GetRoleAndId(this HttpContext context)
        {
            var roleClaim = context.User.Claims.FirstOrDefault(f => f.Type == ClaimsIdentity.DefaultRoleClaimType);
            var userIdClaim = context.User.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);
            if (roleClaim != null && userIdClaim != null)
            {
                return (roleClaim.Value, long.Parse(userIdClaim.Value));
            }

            return ("", 0);
        }



        public class AuthResponsesOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var authAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true));

                var attributes = authAttributes?.ToList() ?? new List<object>();
                if (!attributes.Any()) return;
                if (attributes.FirstOrDefault(f => f.GetType() == typeof(AllowAnonymousAttribute)) != null) return;

                if (attributes.FirstOrDefault(f => f.GetType() == typeof(AuthorizeAttribute)) == null) return;
                var securityRequirement = new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new List<string>()
            }
        };
                operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            }
        }
    }
}
