using NotesBackend.Endpoints;
using NotesBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();

// NSwag/OpenAPI configuration
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Notes API";
    config.Description = "A minimal REST API for managing notes (create, read, update, delete).";
    config.Version = "v1";
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Dependency Injection: In-memory repository (swappable in future)
builder.Services.AddSingleton<INoteRepository, InMemoryNoteRepository>();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

// Health check endpoint
// PUBLIC_INTERFACE
app.MapGet("/", () => new { message = "Healthy" })
   .WithName("HealthCheck")
   .WithSummary("Health check")
   .WithDescription("Simple endpoint to verify the service is up.")
   .WithTags("Health");

// Map Notes endpoints
app.MapNotesEndpoints();

app.Run();