# Services - Commands Reference

Quick reference guide for common development tasks for both ContestService and SubmissionService.

## Application Commands

### Run ContestService
```powershell
dotnet run --project src/Services/ContestService/ContestService.API
```

**Note**: Once running, access Swagger UI at the root URL (http://localhost:5000 or https://localhost:5001)

### Run SubmissionService
```powershell
dotnet run --project src/Services/SubmissionService/SubmissionService.API
```

**Note**: Once running, access Swagger UI at the root URL (http://localhost:5000 or https://localhost:5001)

### Stop Application
Press `Ctrl + C` in the terminal where the application is running.

### Run Application in Background (Development)
```powershell
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD'; dotnet run --project src/Services/ContestService/ContestService.API"
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

### ContestService Migrations

#### Add New Migration
```powershell
dotnet ef migrations add MigrationName --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
```

**Example:**
```powershell
dotnet ef migrations add AddUserTable --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
```

#### List All Migrations
```powershell
dotnet ef migrations list --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
```

#### Remove Last Migration (if not applied to database)
```powershell
dotnet ef migrations remove --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
```

#### Update Database (Apply All Pending Migrations)
```powershell
dotnet ef database update --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
```

#### Update Database to Specific Migration
```powershell
dotnet ef database update MigrationName --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
```

#### Drop Database (⚠️ WARNING: Deletes all data)
```powershell
dotnet ef database drop --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
```

### SubmissionService Migrations

#### Add New Migration
```powershell
dotnet ef migrations add MigrationName --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
```

**Example:**
```powershell
dotnet ef migrations add InitialCreate --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
```

#### List All Migrations
```powershell
dotnet ef migrations list --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
```

#### Remove Last Migration (if not applied to database)
```powershell
dotnet ef migrations remove --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
```

#### Update Database (Apply All Pending Migrations)
```powershell
dotnet ef database update --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
```

#### Update Database to Specific Migration
```powershell
dotnet ef database update MigrationName --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
```

#### Drop Database (⚠️ WARNING: Deletes all data)
```powershell
dotnet ef database drop --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
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

### ContestService - Full Reset (Drop, Create, Migrate, Seed)
```powershell
# Drop database
dotnet ef database drop --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API

# Create new migration
dotnet ef migrations add InitialCreate --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API

# Apply migration (seed data runs automatically on startup)
dotnet ef database update --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API

# Run application
dotnet run --project src/Services/ContestService/ContestService.API
```

### SubmissionService - Full Reset (Drop, Create, Migrate)
```powershell
# Drop database
dotnet ef database drop --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API

# Create new migration
dotnet ef migrations add InitialCreate --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API

# Apply migration
dotnet ef database update --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API

# Run application
dotnet run --project src/Services/SubmissionService/SubmissionService.API
```

### Quick Restart Workflow
```powershell
# ContestService: Stop: Ctrl+C, Then run again:
dotnet run --project src/Services/ContestService/ContestService.API

# SubmissionService: Stop: Ctrl+C, Then run again:
dotnet run --project src/Services/SubmissionService/SubmissionService.API
```

## Useful Aliases (Optional - Add to PowerShell Profile)

You can add these to your PowerShell profile for convenience:

```powershell
# Add to PowerShell profile: $PROFILE

# ContestService functions
function Start-ContestService {
    dotnet run --project src/Services/ContestService/ContestService.API
}

function Add-ContestMigration {
    param($Name)
    dotnet ef migrations add $Name --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
}

function Update-ContestDatabase {
    dotnet ef database update --project src/Services/ContestService/ContestService.Infrastructure --startup-project src/Services/ContestService/ContestService.API
}

# SubmissionService functions
function Start-SubmissionService {
    dotnet run --project src/Services/SubmissionService/SubmissionService.API
}

function Add-SubmissionMigration {
    param($Name)
    dotnet ef migrations add $Name --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
}

function Update-SubmissionDatabase {
    dotnet ef database update --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
}
```

Then use:
```powershell
# ContestService
Start-ContestService
Add-ContestMigration "MyMigrationName"
Update-ContestDatabase

# SubmissionService
Start-SubmissionService
Add-SubmissionMigration "MyMigrationName"
Update-SubmissionDatabase
```

