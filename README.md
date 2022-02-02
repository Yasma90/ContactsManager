# ContactsManager
The Web API use JWT bearer token as authorization
Authentication: admin admin

## Further help
`Add Migrations`
dotnet ef migrations add Init --project ContactsManager.Persistence -s ContactsManager.API --context <DbContext: ContactsDbContext | ApplicationDbContext>

`Update Database`
dotnet ef database update --project ContactsManager.Persistence -s ContactsManager.API --context <DbContext: ContactsDbContext | ApplicationDbContext>

`Remove Migrations`
dotnet ef migrations remove --project ContactsManager.Persistence


