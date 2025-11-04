# SubmissionService Database Migration

## Проблема

Таблицы `Submission` и `Appeal` не существуют в базе данных, что вызывает ошибки:
- `42P01: отношение "Appeal" не существует`
- `42P01: отношение "Submission" не существует`

## Решение

### Вариант 1: Автоматическое применение (через PowerShell скрипт)

```powershell
.\scripts\apply-submission-migration.ps1
```

### Вариант 2: Ручное применение через psql

```bash
psql -h localhost -U postgres -d ContestManagementDb -f src/Services/SubmissionService/SubmissionService.Infrastructure/Migrations/apply_migration.sql
```

### Вариант 3: Через EF Core (после остановки приложения)

```powershell
# Остановите запущенное приложение SubmissionService (Ctrl+C)

# Затем примените миграцию:
dotnet ef database update --project src/Services/SubmissionService/SubmissionService.Infrastructure --startup-project src/Services/SubmissionService/SubmissionService.API
```

### Вариант 4: Ручное выполнение SQL

Откройте SQL скрипт:
```
src/Services/SubmissionService/SubmissionService.Infrastructure/Migrations/apply_migration.sql
```

Скопируйте содержимое и выполните в вашем PostgreSQL клиенте (pgAdmin, DBeaver, DataGrip и т.д.)

## SQL Скрипт

SQL скрипт создает:
1. Таблицу `Submission` с полями:
   - Id (PK, auto-increment)
   - ContestNoticeId (FK to ContestNotice)
   - ParticipantId (FK to User)
   - DateTime (timestamp, default CURRENT_TIMESTAMP)
   - CoverLetter, Comment, DocsPackageIsValid, SubmissionState

2. Таблицу `Appeal` с полями:
   - Id (PK, auto-increment)
   - SubmissionId (FK to Submission, unique index for 1:1 relationship)
   - DateTime (timestamp, default CURRENT_TIMESTAMP)
   - CoverLetter, AppealState, Comment

3. Уникальный индекс `IX_Appeal_SubmissionId` для обеспечения 1:1 связи

## После применения миграции

После успешного применения миграции перезапустите SubmissionService:

```powershell
dotnet run --project src/Services/SubmissionService/SubmissionService.API
```

Таблицы должны быть созданы, и ошибки должны исчезнуть.

