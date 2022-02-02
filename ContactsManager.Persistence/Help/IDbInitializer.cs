namespace ContactsManager.Persistence.Help
{
    public interface IDbInitializer
    {
        void Initialize();
        void SeedData();
        void SeedDataUser();
    }
}