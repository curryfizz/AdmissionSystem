using AdmissionSystem.Core.Patterns;
using AdmissionSystem.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Allows all origins
              .AllowAnyMethod()  // Allows all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader(); // Allows all headers
    });
});

// Register singleton AdmissionSystem using a factory method
builder.Services.AddSingleton<AdmissionSystem.Core.Patterns.AdmissionSystem>(sp =>
    AdmissionSystem.Core.Patterns.AdmissionSystem.GetInstance());

// Build the app
var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); // Enable CORS globally

app.UseAuthorization();
app.MapControllers();
app.Run();
