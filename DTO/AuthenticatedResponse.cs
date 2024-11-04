namespace WebApplication4.DTO
{
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
