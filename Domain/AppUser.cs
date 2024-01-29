using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using textileManagment.Entities.Base.IBase;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace textileManagment.Entities
{
    public class AppUser : IdentityUser<long> , IGeneralBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public string? ProfilePic { get; set; }
        public string? Permission { get; set; }
        public string? DisabledComments { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedById { get; set; }
        public bool IsActive { get; set; }
        public long? CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }

        [NotMapped]
        public string Name => $"{FirstName} {LastName}";
        public long? AppUserConfigurationId { get; set; }
        public AppUserConfiguration? AppUserConfiguration { get; set; }


    }
}
