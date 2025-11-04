# Start Both Services
Write-Host "========================================" -ForegroundColor Green
Write-Host "Starting Both Microservices" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Start ContestService in new window
Write-Host "Starting ContestService in new window..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD'; Write-Host 'Starting ContestService...' -ForegroundColor Green; dotnet run --project src/Services/ContestService/ContestService.API"

# Wait a bit before starting second service
Start-Sleep -Seconds 2

# Start SubmissionService in new window
Write-Host "Starting SubmissionService in new window..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD'; Write-Host 'Starting SubmissionService...' -ForegroundColor Green; dotnet run --project src/Services/SubmissionService/SubmissionService.API"

Write-Host ""
Write-Host "Both services are starting in separate windows." -ForegroundColor Green
Write-Host ""
Write-Host "ContestService Swagger UI:" -ForegroundColor Yellow
Write-Host "  HTTP:  http://localhost:5146" -ForegroundColor Cyan
Write-Host "  HTTPS: https://localhost:7282" -ForegroundColor Cyan
Write-Host ""
Write-Host "SubmissionService Swagger UI:" -ForegroundColor Yellow
Write-Host "  HTTP:  http://localhost:5118" -ForegroundColor Cyan
Write-Host "  HTTPS: https://localhost:7291" -ForegroundColor Cyan
Write-Host ""

