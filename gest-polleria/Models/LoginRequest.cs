namespace gest_polleria.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; } = default!;
        public string ClaveHash { get; set; } = default!;
    }
}