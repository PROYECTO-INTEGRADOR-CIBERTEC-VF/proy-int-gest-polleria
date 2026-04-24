namespace appPolleria.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string ClaveHash { get; set; } = string.Empty;
    }
}