namespace textileManagment.Bussiness.Dtos.Request
{
    public record LoginReq
    {
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }

    }
}
