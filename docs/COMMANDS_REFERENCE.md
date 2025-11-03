# Contest Service - Commands Reference

Quick reference guide for common development tasks.

## Application Commands

### Run Application
```powershell
dotnet run --project src/ContestService/ContestService.API
```

**Note**: Once running, access Swagger UI at the root URL (http://localhost:5000 or https://localhost:5001)

### Stop Application
Press `Ctrl + C` in the terminal where the application is running.

### Run Application in Background (Development)
```powershell
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD'; dotnet run --project src/ContestService/ContestService.API"
```

### Check if Application is Running
```powershell
Get-Process -Name "ContestService.API" -ErrorAction SilentlyContinue
```

### Kill Application Process (if stuck)
```powershell
Stop-Process -Name "ContestService.API" -Force
```

## Database Migration Commands

### Add New Migration
```powershell
dotnet ef migrations add MigrationName --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
```

**Example:**
```powershell
dotnet ef migrations add AddUserTable --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
```

### List All Migrations
```powershell
dotnet ef migrations list --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
```

### Remove Last Migration (if not applied to database)
```powershell
dotnet ef migrations remove --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
```

### Update Database (Apply All Pending Migrations)
```powershell
dotnet ef database update --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
```

### Update Database to Specific Migration
```powershell
dotnet ef database update MigrationName --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
```

### Drop Database (⚠️ WARNING: Deletes all data)
```powershell
dotnet ef database drop --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
```

## Build Commands

### Build Solution
```powershell
dotnet build
```

### Clean Solution
```powershell
dotnet clean
```

### Restore Packages
```powershell
dotnet restore
```

### Rebuild Solution
```powershell
dotnet clean
dotnet build
```

## Testing Commands

### Run All Tests
```powershell
dotnet test
```

### Run Tests with Verbose Output
```powershell
dotnet test --verbosity detailed
```

## Development Workflow Examples

### Full Reset (Drop, Create, Migrate, Seed)
```powershell
# Drop database
dotnet ef database drop --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API

# Create new migration
dotnet ef migrations add InitialCreate --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API

# Apply migration (seed data runs automatically on startup)
dotnet ef database update --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API

# Run application
dotnet run --project src/ContestService/ContestService.API
```

### Quick Restart Workflow
```powershell
# Stop: Ctrl+C
# Then run again:
dotnet run --project src/ContestService/ContestService.API
```

## Useful Aliases (Optional - Add to PowerShell Profile)

You can add these to your PowerShell profile for convenience:

```powershell
# Add to PowerShell profile: $PROFILE
function Start-ContestService {
    dotnet run --project src/ContestService/ContestService.API
}

function Add-Migration {
    param($Name)
    dotnet ef migrations add $Name --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
}

function Update-Database {
    dotnet ef database update --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
}

function Remove-Migration {
    dotnet ef migrations remove --project src/ContestService/ContestService.Infrastructure --startup-project src/ContestService/ContestService.API
}
```

Then use:
```powershell
Start-ContestService
Add-Migration "MyMigrationName"
Update-Database
Remove-Migration
```

