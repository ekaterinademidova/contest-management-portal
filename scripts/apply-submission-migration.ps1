# Apply SubmissionService database migration
# This script applies the migration SQL directly to the database

Write-Host "Applying SubmissionService migration..." -ForegroundColor Green
Write-Host ""

$connectionString = "Host=localhost;Port=5432;Database=ContestManagementDb;Username=postgres;Password=postgres"
$sqlScript = "src\Services\SubmissionService\SubmissionService.Infrastructure\Migrations\apply_migration.sql"

if (-not (Test-Path $sqlScript)) {
    Write-Host "Error: SQL script not found at $sqlScript" -ForegroundColor Red
    exit 1
}

Write-Host "Executing SQL script..." -ForegroundColor Yellow
Write-Host "SQL Script: $sqlScript" -ForegroundColor Cyan
Write-Host ""

# Extract connection details
$dbName = "ContestManagementDb"
$dbUser = "postgres"
$dbHost = "localhost"
$dbPort = "5432"

# Try to execute with psql
$psqlPath = Get-Command psql -ErrorAction SilentlyContinue

if ($psqlPath) {
    Write-Host "Using psql to execute migration..." -ForegroundColor Green
    $env:PGPASSWORD = "postgres"
    & psql -h $dbHost -p $dbPort -U $dbUser -d $dbName -f $sqlScript
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "Migration applied successfully!" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "Error applying migration. Exit code: $LASTEXITCODE" -ForegroundColor Red
    }
    Remove-Item Env:\PGPASSWORD
} else {
    Write-Host "psql not found in PATH." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Please run the SQL script manually:" -ForegroundColor Yellow
    Write-Host "1. Open PostgreSQL client (pgAdmin, DBeaver, or psql)" -ForegroundColor Cyan
    Write-Host "2. Connect to database: ContestManagementDb" -ForegroundColor Cyan
    Write-Host "3. Execute the SQL from: $sqlScript" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Or use psql command:" -ForegroundColor Yellow
    Write-Host "psql -h localhost -U postgres -d ContestManagementDb -f $sqlScript" -ForegroundColor Cyan
}

