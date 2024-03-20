using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using textileManagment.Bussiness.Interface;

namespace textileManagment.Bussiness.Dtos.Responce
{
    public class LoginRes
    {
        public long Id { get; set; }    
        public string? Username { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }
    }
}
