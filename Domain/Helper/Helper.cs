using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using textileManagment.Domain.Context;
using textileManagment.Entities;
using static textileManagment.Domain.Helper.Helper;
using Microsoft.AspNetCore.Builder;
using System.Data;

namespace textileManagment.Domain.Helper
{
    public class Helper
    {
        public static class Roles
        {
            public const string SuperAdmin = "SuperAdmin";
            public const string OrgAdmin = "OrgAdmin";
            public const string OrgStaff = "OrgStaff";
            public const string Supplier = "Supplier";
            public const string User = "User";
            public static string Multiple(params string[] roles) => string.Join(",", roles);
        }
    }

    public static class DomainExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = 1,
                FirstName = "Super",
                LastName = "Admin",
                Role = Roles.SuperAdmin,
                PhoneNumber = "03024340220",
                Email = "superadmin@textile.com",
                NormalizedEmail = "SUPERADMIN@TEXTILE.COM",
                EmailConfirmed = true,
                UserName = "superadmin",
                NormalizedUserName = "SUPERADMIN",
                IsActive = true,
                IsDelete = false,
                CreatedDate = DateTime.Parse("2024-01-29 11:20:58Z"),
                //password = admin@KK@i10
                PasswordHash = "AQAAAAEAACcQAAAAEN7PfOcjhhSXy4TS6annDjtXSJ/wwgHnDfp3IHtakYHvUyVmkmKLkVf5+1dtRfW9ww==",
                SecurityStamp = "HG43M3DRHH5JS5Y3EIU5Y6OFOUVX4KZO"
            });
            modelBuilder.Entity<AppRole>().HasData(
            new AppRole { Id = 1, Name = Roles.SuperAdmin, NormalizedName = Roles.SuperAdmin.ToUpper()},
            new AppRole { Id = 2, Name = Roles.OrgAdmin, NormalizedName = Roles.OrgAdmin.ToUpper() },
            new AppRole { Id = 3, Name = Roles.OrgStaff, NormalizedName = Roles.OrgStaff.ToUpper() },
            new AppRole { Id = 4, Name = Roles.Supplier, NormalizedName = Roles.Supplier.ToUpper() },
            new AppRole { Id = 5, Name = Roles.User, NormalizedName = Roles.User.ToUpper() }

            );
        }
             public static void LoadDb(this IServiceCollection services, IConfiguration configuration)
             {
                 services.AddDbContext<ApplicationDbContext>(options =>
                 {
                     options.UseSqlServer(configuration.GetConnectionString("TextileDb"));
                 });
             }

        public static long GetUserId(this HttpContext context)
        {
            var userData = long.Parse(context.User.Claims
               ?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            return userData;
        }

        public static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        public static int DateTimeToUnixTimestamp(this DateTime dateTime) =>
            (int)(dateTime - epoch).TotalSeconds;
    }

}
