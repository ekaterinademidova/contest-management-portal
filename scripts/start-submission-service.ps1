# Start SubmissionService
Write-Host "Starting SubmissionService..." -ForegroundColor Green
Write-Host "Swagger UI will be available at:" -ForegroundColor Yellow
Write-Host "  HTTP:  http://localhost:5118" -ForegroundColor Cyan
Write-Host "  HTTPS: https://localhost:7291" -ForegroundColor Cyan
Write-Host ""

dotnet run --project src/Services/SubmissionService/SubmissionService.API

