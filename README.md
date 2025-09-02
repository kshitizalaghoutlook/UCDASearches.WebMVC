# UCDASearches.WebMVC

## Configuration

The application retrieves its database connection string from configuration sources such as environment variables or [ASP.NET Core user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets).

### Environment variable

Set `ConnectionStrings__DefaultConnection` before running the app:

```bash
export ConnectionStrings__DefaultConnection="Server=SQL;Database=Searches;Trusted_Connection=True;MultipleActiveResultSets=true"
```

On Windows PowerShell use:

```powershell
$env:ConnectionStrings__DefaultConnection="Server=SQL;Database=Searches;Trusted_Connection=True;MultipleActiveResultSets=true"
```

### User secrets (development)

From the project directory run:

```bash
# Initialize if this is the first time
 dotnet user-secrets init
 dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=SQL;Database=Searches;Trusted_Connection=True;MultipleActiveResultSets=true"
```

The connection string is retrieved in code with:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
```

This value is consumed, for example, by `PreviousSearchesController` when querying the database.

