# ContestService Microservice - Implementation Plan

## Overview
This document outlines a comprehensive step-by-step implementation plan for the ContestService microservice following Clean Architecture principles, SOLID design patterns, REST API conventions, and modern .NET best practices.

---

## Architecture Overview

### Clean Architecture Layers
```
┌─────────────────────────────────────┐
│   Presentation Layer (API)           │  ← Controllers, Middleware, Filters
├─────────────────────────────────────┤
│   Application Layer                  │  ← Use Cases, DTOs, Interfaces, Mediator
├─────────────────────────────────────┤
│   Domain Layer                       │  ← Entities, Value Objects, Domain Events
├─────────────────────────────────────┤
│   Infrastructure Layer               │  ← EF Core, Repositories, External Services
└─────────────────────────────────────┘
```

### Technology Stack
- **.NET 8** (or latest LTS)
- **ASP.NET Core Web API**
- **Entity Framework Core 8** with PostgreSQL
- **MediatR** (Mediator pattern implementation)
- **FluentValidation** (for validation)
- **AutoMapper** (or Mapster) for mapping
- **Serilog** (for logging)
- **Swagger/OpenAPI** (API documentation)

---

## Step-by-Step Implementation Plan

### Phase 1: Project Structure & Solution Setup

#### Step 1.1: Create Solution Structure
```
ContestManagementPortal/
├── src/
│   └── Services/
│       └── ContestService/
│           ├── ContestService.API/              (Presentation)
│           ├── ContestService.Application/      (Application)
│           ├── ContestService.Domain/           (Domain)
│           └── ContestService.Infrastructure/   (Infrastructure)
└── tests/
    └── ContestService.Tests/
        ├── ContestService.UnitTests/
        └── ContestService.IntegrationTests/
```

**Tasks:**
1. Create solution file: `ContestManagementPortal.sln`
2. Create project structure following Clean Architecture layers
3. Set up project references:
   - API → Application → Domain
   - API → Infrastructure (only for DI registration)
   - Infrastructure → Application → Domain
   - Application → Domain (no other dependencies)

#### Step 1.2: Configure Projects (.csproj files)
- Set target framework: `net8.0`
- Enable nullable reference types: `<Nullable>enable</Nullable>`
- Enable implicit usings
- Configure package references (add later as needed)

---

### Phase 2: Domain Layer (Core Business Logic)

#### Step 2.1: Domain Entities
**Location:** `ContestService.Domain/Entities/`

**Entities to Create:**
1. **Contest**
   - Properties: `Id`, `Name`, `Description`, `StartDate`, `EndDate`, `Status`, `MaxParticipants`, `CreatedBy`, `CreatedAt`, `UpdatedAt`
   - Domain events: `ContestCreated`, `ContestUpdated`, `ContestStatusChanged`
   - Validation: Business rules for dates, status transitions

2. **ContestParticipant** (if needed)
   - Properties: `Id`, `ContestId`, `UserId`, `JoinedAt`, `Status`
   - Relationship with Contest (many-to-one)

3. **ContestProblem** (if contests have problems)
   - Properties: `Id`, `ContestId`, `ProblemId`, `Order`, `Points`

**Key Principles:**
- Entities are rich domain models (not anemic)
- Include domain methods, not just properties
- Encapsulate business logic within entities
- Use private setters where appropriate

**Example Structure:**
```csharp
namespace ContestService.Domain.Entities;

public class Contest : EntityBase, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public ContestStatus Status { get; private set; }
    public int MaxParticipants { get; private set; }
    public Guid CreatedBy { get; private set; }
    
    // Navigation properties
    private readonly List<ContestParticipant> _participants = new();
    public IReadOnlyCollection<ContestParticipant> Participants => _participants.AsReadOnly();
    
    // Domain methods
    public void UpdateStatus(ContestStatus newStatus);
    public void AddParticipant(Guid userId);
    public bool CanAddParticipant();
}
```

#### Step 2.2: Value Objects
**Location:** `ContestService.Domain/ValueObjects/`

**Examples:**
- `ContestPeriod` (StartDate, EndDate validation)
- `ContestName` (with validation rules)

#### Step 2.3: Domain Enums
**Location:** `ContestService.Domain/Enums/`

- `ContestStatus`: Draft, Scheduled, InProgress, Completed, Cancelled
- `ParticipantStatus`: Registered, Active, Disqualified, Completed

#### Step 2.4: Domain Events
**Location:** `ContestService.Domain/Events/`

**Implement:**
- `IDomainEvent` interface
- `ContestCreatedEvent`
- `ContestUpdatedEvent`
- `ContestStatusChangedEvent`

#### Step 2.5: Domain Interfaces (Repository Contracts)
**Location:** `ContestService.Domain/Interfaces/`

```csharp
public interface IContestRepository
{
    Task<Contest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Contest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Contest> AddAsync(Contest contest, CancellationToken cancellationToken = default);
    Task UpdateAsync(Contest contest, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
```

#### Step 2.6: Base Classes
**Location:** `ContestService.Domain/Common/`

- `EntityBase`: Base entity with `Id`, `CreatedAt`, `UpdatedAt`
- `IAggregateRoot`: Marker interface for aggregate roots
- `ValueObject`: Base class for value objects

---

### Phase 3: Application Layer (Use Cases & Business Logic)

#### Step 3.1: DTOs (Data Transfer Objects)
**Location:** `ContestService.Application/DTOs/`

**DTOs to Create:**
- `CreateContestDto`
- `UpdateContestDto`
- `ContestDto` (response)
- `ContestListDto` (for list views)
- `ContestQueryDto` (for filtering/paging)

#### Step 3.2: Commands (CQRS - Write Operations)
**Location:** `ContestService.Application/Commands/`

**Structure:**
```
Commands/
├── CreateContest/
│   ├── CreateContestCommand.cs
│   ├── CreateContestCommandHandler.cs
│   └── CreateContestCommandValidator.cs
├── UpdateContest/
│   ├── UpdateContestCommand.cs
│   ├── UpdateContestCommandHandler.cs
│   └── UpdateContestCommandValidator.cs
└── DeleteContest/
    ├── DeleteContestCommand.cs
    └── DeleteContestCommandHandler.cs
```

**Example:**
```csharp
namespace ContestService.Application.Commands.CreateContest;

public record CreateContestCommand(
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    int MaxParticipants,
    Guid CreatedBy
) : IRequest<ContestDto>;

public class CreateContestCommandHandler 
    : IRequestHandler<CreateContestCommand, ContestDto>
{
    private readonly IContestRepository _repository;
    private readonly IMapper _mapper;
    
    // Implementation...
}
```

#### Step 3.3: Queries (CQRS - Read Operations)
**Location:** `ContestService.Application/Queries/`

**Structure:**
```
Queries/
├── GetContestById/
│   ├── GetContestByIdQuery.cs
│   └── GetContestByIdQueryHandler.cs
├── GetContests/
│   ├── GetContestsQuery.cs
│   └── GetContestsQueryHandler.cs
└── GetContestParticipants/
    ├── GetContestParticipantsQuery.cs
    └── GetContestParticipantsQueryHandler.cs
```

#### Step 3.4: Mapping Profiles
**Location:** `ContestService.Application/Mappings/`

- Use AutoMapper or Mapster
- Create profiles: `ContestMappingProfile`

#### Step 3.5: Application Interfaces
**Location:** `ContestService.Application/Interfaces/`

- Extend domain interfaces if needed
- Service interfaces for external dependencies

#### Step 3.6: Application Behaviors (MediatR Pipeline)
**Location:** `ContestService.Application/Behaviors/`

- `ValidationBehavior<TRequest, TResponse>`: FluentValidation pipeline
- `LoggingBehavior<TRequest, TResponse>`: Request/Response logging
- `TransactionBehavior<TRequest, TResponse>`: Unit of Work pattern

---

### Phase 4: Infrastructure Layer (Data Access & External Services)

#### Step 4.1: DbContext Configuration
**Location:** `ContestService.Infrastructure/Data/`

```csharp
namespace ContestService.Infrastructure.Data;

public class ContestDbContext : DbContext
{
    public ContestDbContext(DbContextOptions<ContestDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contest> Contests { get; set; }
    public DbSet<ContestParticipant> ContestParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContestDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
```

#### Step 4.2: Entity Configurations (Fluent API)
**Location:** `ContestService.Infrastructure/Data/Configurations/`

- `ContestConfiguration.cs`: Table name, constraints, relationships
- `ContestParticipantConfiguration.cs`

**Example:**
```csharp
namespace ContestService.Infrastructure.Data.Configurations;

public class ContestConfiguration : IEntityTypeConfiguration<Contest>
{
    public void Configure(EntityTypeBuilder<Contest> builder)
    {
        builder.ToTable("Contests");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(2000);
        // Relationships, indexes, etc.
    }
}
```

#### Step 4.3: Repository Implementations
**Location:** `ContestService.Infrastructure/Repositories/`

```csharp
namespace ContestService.Infrastructure.Repositories;

public class ContestRepository : IContestRepository
{
    private readonly ContestDbContext _context;

    public ContestRepository(ContestDbContext context)
    {
        _context = context;
    }

    public async Task<Contest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Contests
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    // Implement other methods...
}
```

#### Step 4.4: Unit of Work Pattern
**Location:** `ContestService.Infrastructure/Data/`

- `IUnitOfWork` interface (in Application layer)
- `UnitOfWork` implementation (in Infrastructure layer)

#### Step 4.5: PostgreSQL Connection Configuration
**Location:** `ContestService.Infrastructure/Data/`

- Connection string configuration
- DbContext factory if needed
- Migrations setup

#### Step 4.6: Dependency Injection Configuration
**Location:** `ContestService.Infrastructure/DependencyInjection.cs`

```csharp
namespace ContestService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ContestDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ContestDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IContestRepository, ContestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
```

---

### Phase 5: Presentation Layer (API)

#### Step 5.1: Controllers
**Location:** `ContestService.API/Controllers/`

**Create RESTful Controllers:**
- `ContestsController.cs`: CRUD operations
- Follow REST conventions:
  - GET `/api/contests` - Get all
  - GET `/api/contests/{id}` - Get by ID
  - POST `/api/contests` - Create
  - PUT `/api/contests/{id}` - Update
  - DELETE `/api/contests/{id}` - Delete

**Example:**
```csharp
namespace ContestService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContestDto>>> GetContests(
        [FromQuery] GetContestsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ContestDto>> CreateContest(
        [FromBody] CreateContestCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetContest), new { id = result.Id }, result);
    }
}
```

#### Step 5.2: Middleware
**Location:** `ContestService.API/Middleware/`

- `ExceptionHandlingMiddleware`: Global exception handling
- `RequestLoggingMiddleware`: Request/response logging

#### Step 5.3: Filters
**Location:** `ContestService.API/Filters/`

- `ValidationFilter`: Model validation
- `ApiExceptionFilter`: API-specific exceptions

#### Step 5.4: Program.cs / Startup Configuration
**Location:** `ContestService.API/`

**Configure:**
- Services registration (DI)
- MediatR
- AutoMapper
- FluentValidation
- Swagger/OpenAPI
- CORS (if needed)
- Authentication/Authorization (if needed)
- Database migrations (on startup or separate process)

**Example Structure:**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application layer
builder.Services.AddApplication();

// Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

// Presentation layer
builder.Services.AddPresentation();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

#### Step 5.5: appsettings.json Configuration
- Connection strings
- Logging configuration
- API settings
- Feature flags

---

### Phase 6: Database Migrations

#### Step 6.1: Initial Migration
```bash
dotnet ef migrations add InitialCreate --project ContestService.Infrastructure --startup-project ContestService.API
```

#### Step 6.2: Apply Migration
- Auto-apply on startup (development)
- Or use migration script (production)

---

### Phase 7: Testing

#### Step 7.1: Unit Tests
**Location:** `tests/ContestService.Tests/ContestService.UnitTests/`

- Domain entity tests
- Command/Query handler tests
- Repository tests (mocked)
- Value object tests

#### Step 7.2: Integration Tests
**Location:** `tests/ContestService.Tests/ContestService.IntegrationTests/`

- API endpoint tests
- Database integration tests
- End-to-end workflow tests

---

### Phase 8: Additional Features

#### Step 8.1: Logging
- Configure Serilog
- Structured logging
- Request/Response logging

#### Step 8.2: Health Checks
- Database health check
- API health check endpoint

#### Step 8.3: API Documentation
- Swagger configuration
- XML comments for controllers
- Example requests/responses

#### Step 8.4: Error Handling
- Global exception handler
- Custom exception types
- Standardized error responses

#### Step 8.5: Validation
- FluentValidation rules
- Model validation
- Business rule validation

---

## Implementation Order (Recommended)

1. **Phase 1**: Solution structure ✅
2. **Phase 2**: Domain layer (core entities, interfaces)
3. **Phase 3**: Application layer (one command/query at a time)
4. **Phase 4**: Infrastructure layer (DbContext, repositories)
5. **Phase 5**: Presentation layer (controllers, API)
6. **Phase 6**: Database migrations
7. **Phase 7**: Testing
8. **Phase 8**: Polish (logging, health checks, etc.)

---

## Key Principles to Follow

### SOLID Principles
- **S**ingle Responsibility: Each class has one reason to change
- **O**pen/Closed: Open for extension, closed for modification
- **L**iskov Substitution: Derived classes must be substitutable
- **I**nterface Segregation: Many specific interfaces > one general
- **D**ependency Inversion: Depend on abstractions, not concretions

### Clean Architecture
- Dependency rule: Dependencies point inward
- Domain layer has no dependencies
- Application layer depends only on Domain
- Infrastructure and Presentation depend on Application and Domain

### REST Principles
- Use HTTP methods correctly (GET, POST, PUT, DELETE, PATCH)
- Resource-based URLs
- Proper HTTP status codes
- Stateless operations
- HATEOAS (optional, but good practice)

### Best Practices
- Async/await for all I/O operations
- Use CancellationToken for cancellation support
- Proper error handling and logging
- Input validation at multiple layers
- Use records for DTOs (immutability)
- Use Value Objects for domain concepts

---

## Next Steps

1. Review and approve this plan
2. Set up the solution structure
3. Begin with Domain layer implementation
4. Iterate through each phase
5. Test continuously as you build

---

## Notes

- This is the first microservice, so consider future inter-service communication patterns
- Consider implementing API Gateway pattern later
- Plan for eventual distributed transaction handling
- Consider event-driven architecture for microservice communication

---

# SubmissionService Microservice - Implementation Plan

## Overview
This document outlines the implementation of the SubmissionService microservice following Clean Architecture principles, SOLID design patterns, REST API conventions, and modern .NET best practices. This microservice handles contest submissions and appeals.

## Architecture Overview

### Clean Architecture Layers
```
┌─────────────────────────────────────┐
│   Presentation Layer (API)           │  ← Controllers, Middleware, Filters
├─────────────────────────────────────┤
│   Application Layer                  │  ← Use Cases, DTOs, Interfaces
├─────────────────────────────────────┤
│   Domain Layer                       │  ← Entities, Value Objects, Domain Events
├─────────────────────────────────────┤
│   Infrastructure Layer               │  ← EF Core, Repositories, External Services
└─────────────────────────────────────┘
```

### Technology Stack
- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core 9** with PostgreSQL
- **FluentValidation** (for validation)
- **Swagger/OpenAPI** (API documentation)

---

## Project Structure

### Solution Structure
```
ContestManagementPortal/
├── src/
│   └── Services/
│       └── SubmissionService/
│           ├── SubmissionService.API/              (Presentation)
│           ├── SubmissionService.Application/      (Application)
│           ├── SubmissionService.Domain/           (Domain)
│           └── SubmissionService.Infrastructure/   (Infrastructure)
```

**Project References:**
- API → Application → Domain
- API → Infrastructure (only for DI registration)
- Infrastructure → Application → Domain
- Application → Domain (no other dependencies)

---

## Domain Layer (Core Business Logic)

### Domain Entities

#### Submission Entity
**Location:** `SubmissionService.Domain/Entities/Submission.cs`

**Properties:**
- `Id` (int) - Primary key
- `ContestNoticeId` (int) - Foreign key to ContestNotice (in ContestService)
- `ParticipantId` (int) - Foreign key to User (in UserService)
- `DateTime` (DateTime) - Submission timestamp
- `CoverLetter` (string?) - Cover letter text
- `Comment` (string?) - Additional comments
- `DocsPackageIsValid` (bool?) - Document package validation status
- `SubmissionState` (string?) - State: Pending, Approved, Rejected

**Navigation Properties:**
- `Appeal` - One-to-one relationship with Appeal

#### Appeal Entity
**Location:** `SubmissionService.Domain/Entities/Appeal.cs`

**Properties:**
- `Id` (int) - Primary key
- `SubmissionId` (int) - Foreign key to Submission (1:1 relationship)
- `DateTime` (DateTime) - Appeal timestamp
- `CoverLetter` (string?) - Appeal cover letter
- `AppealState` (string?) - State: Pending, Considered
- `Comment` (string?) - Additional comments

**Navigation Properties:**
- `Submission` - One-to-one relationship with Submission

### Domain Interfaces

#### Repository Interfaces
- `IRepository<T>` - Generic repository interface
- `ISubmissionRepository` - Submission-specific repository
- `IAppealRepository` - Appeal-specific repository

---

## Application Layer (Use Cases & Business Logic)

### DTOs (Data Transfer Objects)

#### Submission DTOs
- `SubmissionDto` - Response DTO
- `CreateSubmissionRequest` - Create request DTO
- `UpdateSubmissionRequest` - Update request DTO
- `SubmissionFilterRequest` - Filtering DTO

#### Appeal DTOs
- `AppealDto` - Response DTO
- `CreateAppealRequest` - Create request DTO
- `UpdateAppealRequest` - Update request DTO

### Validators

All DTOs have FluentValidation validators:
- `CreateSubmissionRequestValidator`
- `UpdateSubmissionRequestValidator`
- `CreateAppealRequestValidator`
- `UpdateAppealRequestValidator`

### Services

#### SubmissionService
**Location:** `SubmissionService.Application/Services/SubmissionService.cs`

**Methods:**
- `GetSubmissionByIdAsync` - Get submission by ID
- `GetAllSubmissionsAsync` - Get all submissions with optional filtering
- `CreateSubmissionAsync` - Create new submission
- `UpdateSubmissionAsync` - Update existing submission
- `DeleteSubmissionAsync` - Delete submission (with appeal check)

**Appeal Methods:**
- `GetAppealByIdAsync` - Get appeal by ID
- `GetAppealBySubmissionIdAsync` - Get appeal by submission ID
- `GetAllAppealsAsync` - Get all appeals
- `CreateAppealAsync` - Create new appeal (ensures 1:1 relationship)
- `UpdateAppealAsync` - Update existing appeal
- `DeleteAppealAsync` - Delete appeal

**Business Logic:**
- Automatic DateTime setting on creation
- Default SubmissionState = "Pending" if not provided
- Validation that Submission exists before creating Appeal
- Enforcement of 1:1 relationship between Submission and Appeal
- Prevention of deletion of Submission with associated Appeal

---

## Infrastructure Layer (Data Access & External Services)

### DbContext Configuration

**Location:** `SubmissionService.Infrastructure/Data/SubmissionDbContext.cs`

**DbSets:**
- `Submissions`
- `Appeals`

**Entity Configurations:**
- Submission: Table "Submission", timestamp defaults, foreign key constraints
- Appeal: Table "Appeal", 1:1 relationship with Submission, unique index on SubmissionId

### Repository Implementations

- `BaseRepository<T>` - Generic base repository with common CRUD operations
- `SubmissionRepository` - Submission-specific repository with:
  - `GetByContestNoticeIdAsync`
  - `GetByParticipantIdAsync`
- `AppealRepository` - Appeal-specific repository with:
  - `GetBySubmissionIdAsync`

---

## Presentation Layer (API)

### Controllers

#### SubmissionsController
**Location:** `SubmissionService.API/Controllers/SubmissionsController.cs`

**Endpoints:**
- `GET /api/submissions` - Get all submissions (with optional filtering)
- `GET /api/submissions/{id}` - Get submission by ID
- `POST /api/submissions` - Create new submission
- `PUT /api/submissions/{id}` - Update submission
- `DELETE /api/submissions/{id}` - Delete submission

#### AppealsController
**Location:** `SubmissionService.API/Controllers/AppealsController.cs`

**Endpoints:**
- `GET /api/appeals` - Get all appeals
- `GET /api/appeals/{id}` - Get appeal by ID
- `GET /api/appeals/submission/{submissionId}` - Get appeal by submission ID
- `POST /api/appeals` - Create new appeal
- `PUT /api/appeals/{id}` - Update appeal
- `DELETE /api/appeals/{id}` - Delete appeal

### Program.cs Configuration

- Swagger/OpenAPI configuration
- Dependency injection (Application and Infrastructure layers)
- Database migration on startup
- HTTPS redirection
- Authorization setup

---

## Database Schema

### Submission Table
```sql
CREATE TABLE Submission (
    Id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    ContestNoticeId INT NOT NULL,
    ParticipantId INT NOT NULL,
    DateTime TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CoverLetter TEXT,
    Comment TEXT,
    DocsPackageIsValid BOOLEAN NULL,
    SubmissionState VARCHAR(100),
    CONSTRAINT FK_Submission_ContestNotice FOREIGN KEY (ContestNoticeId) REFERENCES ContestNotice(Id),
    CONSTRAINT FK_Submission_User FOREIGN KEY (ParticipantId) REFERENCES "User"(Id)
);
```

### Appeal Table
```sql
CREATE TABLE Appeal (
    Id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    SubmissionId INT NOT NULL,
    DateTime TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CoverLetter TEXT,
    AppealState VARCHAR(100),
    Comment TEXT,
    CONSTRAINT FK_Appeal_Submission FOREIGN KEY (SubmissionId) REFERENCES Submission(Id),
    CONSTRAINT UQ_Appeal_SubmissionId UNIQUE (SubmissionId)
);
```

---

## Key Features

1. **Clean Architecture**: Proper separation of concerns across layers
2. **Validation**: FluentValidation for all request DTOs
3. **Swagger Documentation**: Complete API documentation with examples
4. **Business Logic**:
   - Automatic timestamp management
   - State management (default values)
   - Relationship enforcement (1:1 between Submission and Appeal)
   - Referential integrity checks
5. **Error Handling**: Proper HTTP status codes and error messages

---

## Implementation Status

✅ **Completed:**
- Domain layer (entities, interfaces)
- Application layer (DTOs, validators, services)
- Infrastructure layer (DbContext, repositories)
- API layer (controllers, Program.cs)
- Swagger configuration
- Dependency injection setup

---

## Next Steps

1. Database migrations
2. Integration testing
3. Performance optimization
4. Additional validation rules
5. Logging and monitoring

