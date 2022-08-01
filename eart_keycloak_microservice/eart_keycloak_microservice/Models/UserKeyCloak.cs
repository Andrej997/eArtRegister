namespace eart_keycloak_microservice.Models
{
    public class UserKeyCloak
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool Enabled { get; set; }
        public bool EmailVerified { get; set; }
    }
}
