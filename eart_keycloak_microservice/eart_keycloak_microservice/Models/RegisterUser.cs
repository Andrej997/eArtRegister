namespace eart_keycloak_microservice.Models
{
    public class RegisterUser
    {
        public RegisterUser(List<Guid> ids)
        {
            Ids = ids ?? throw new ArgumentNullException(nameof(ids));
        }

        public List<Guid> Ids { get; set; }
    }
}
