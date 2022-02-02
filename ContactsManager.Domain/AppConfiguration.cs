
using ContactsManager.Domain.Dtos;

namespace ContactsManager.Domain
{
    public class AppConfiguration
    {
        public TokenProviderOptionsDto TokenProviderOptions { get; set; }
        public UserDto AdminUser { get; set; }
        public bool DbInitializer { get; set; }
        public bool SeedDataUser { get; set; }
    }
}