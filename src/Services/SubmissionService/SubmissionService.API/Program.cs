using SubmissionService.Application;
using SubmissionService.Infrastructure;
using SubmissionService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Submission Service API",
        Version = "v1",
        Description = "RESTful API for managing contest submissions and appeals. " +
                     "This API provides comprehensive endpoints for submission management operations.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Submission Service Team",
            Email = "support@submissionservice.com"
        }
    });

    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    // Enable annotations
    options.EnableAnnotations();

    // Configure OpenAPI 3.0
    options.UseAllOfToExtendReferenceSchemas();
    options.SupportNonNullableReferenceTypes();
});

// Add Application layer
builder.Services.AddApplication();

// Add Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Submission Service API v1");
    options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    options.DisplayRequestDuration();
    options.EnableDeepLinking();
    options.EnableFilter();
    options.EnableValidator();
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    options.DefaultModelsExpandDepth(2);
    options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Apply database migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<SubmissionDbContext>();
        logger.LogInformation("Applying database migrations...");
        context.Database.Migrate(); // Apply pending migrations
        logger.LogInformation("Starting database seeding...");
        context.SeedData(logger); // Seed initial data if tables are empty
        logger.LogInformation("Database seeding completed.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations or seeding data.");
    }
}

app.Run();
