# Команды для запуска микросервисов

## Быстрый запуск

### PowerShell команды

#### Запуск ContestService
```powershell
dotnet run --project src/Services/ContestService/ContestService.API
```

**Или используйте скрипт:**
```powershell
.\scripts\start-contest-service.ps1
```

**Swagger UI:**
- HTTP: http://localhost:5146
- HTTPS: https://localhost:7282

---

#### Запуск SubmissionService
```powershell
dotnet run --project src/Services/SubmissionService/SubmissionService.API
```

**Или используйте скрипт:**
```powershell
.\scripts\start-submission-service.ps1
```

**Swagger UI:**
- HTTP: http://localhost:5118
- HTTPS: https://localhost:7291

---

#### Запуск обоих сервисов одновременно (в отдельных окнах)
```powershell
.\scripts\start-all-services.ps1
```

**Или вручную в двух терминалах:**
```powershell
# Терминал 1
dotnet run --project src/Services/ContestService/ContestService.API

# Терминал 2
dotnet run --project src/Services/SubmissionService/SubmissionService.API
```

---

## Запуск в фоновом режиме (Windows)

### ContestService
```powershell
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD'; dotnet run --project src/Services/ContestService/ContestService.API"
```

### SubmissionService
```powershell
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD'; dotnet run --project src/Services/SubmissionService/SubmissionService.API"
```

---

## Отладка в VS Code

1. Откройте панель отладки (F5 или Ctrl+Shift+D)
2. Выберите конфигурацию:
   - **".NET Core Launch (ContestService)"** - для отладки только ContestService
   - **".NET Core Launch (SubmissionService)"** - для отладки только SubmissionService
   - **"Launch Both Services"** - для отладки обоих сервисов одновременно

---

## Проверка статуса сервисов

### Проверить, запущены ли сервисы
```powershell
# Проверить ContestService
Get-Process -Name "ContestService.API" -ErrorAction SilentlyContinue

# Проверить SubmissionService
Get-Process -Name "SubmissionService.API" -ErrorAction SilentlyContinue

# Проверить оба
Get-Process | Where-Object {$_.ProcessName -like "*Service.API"}
```

### Остановить сервисы
```powershell
# Остановить ContestService
Stop-Process -Name "ContestService.API" -Force -ErrorAction SilentlyContinue

# Остановить SubmissionService
Stop-Process -Name "SubmissionService.API" -Force -ErrorAction SilentlyContinue

# Остановить оба
Stop-Process -Name "ContestService.API","SubmissionService.API" -Force -ErrorAction SilentlyContinue
```

---

## Полезные алиасы (добавьте в PowerShell профиль)

Откройте профиль: `notepad $PROFILE`

Добавьте:
```powershell
# Запуск сервисов
function Start-ContestService {
    dotnet run --project src/Services/ContestService/ContestService.API
}

function Start-SubmissionService {
    dotnet run --project src/Services/SubmissionService/SubmissionService.API
}

function Start-AllServices {
    & .\scripts\start-all-services.ps1
}

# Остановка сервисов
function Stop-ContestService {
    Stop-Process -Name "ContestService.API" -Force -ErrorAction SilentlyContinue
}

function Stop-SubmissionService {
    Stop-Process -Name "SubmissionService.API" -Force -ErrorAction SilentlyContinue
}

function Stop-AllServices {
    Stop-Process -Name "ContestService.API","SubmissionService.API" -Force -ErrorAction SilentlyContinue
}
```

Теперь можно использовать:
```powershell
Start-ContestService
Start-SubmissionService
Start-AllServices
Stop-AllServices
```

