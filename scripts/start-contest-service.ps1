# Start ContestService
Write-Host "Starting ContestService..." -ForegroundColor Green
Write-Host "Swagger UI will be available at:" -ForegroundColor Yellow
Write-Host "  HTTP:  http://localhost:5146" -ForegroundColor Cyan
Write-Host "  HTTPS: https://localhost:7282" -ForegroundColor Cyan
Write-Host ""

dotnet run --project src/Services/ContestService/ContestService.API

