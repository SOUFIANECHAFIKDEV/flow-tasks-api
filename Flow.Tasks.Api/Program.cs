using Flow.Tasks.Api.Endpoints;
using Flow.Tasks.Application.Abstractions;
using Flow.Tasks.Application.Tasks;
using Flow.Tasks.Application.Users;
using Flow.Tasks.Infrastructure.Data;
using Flow.Tasks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string DevCors = "DevCors";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(DevCors, p =>
        p.WithOrigins(
             "http://localhost:4200",   // Angular dev
             "https://localhost:4200")
         .AllowAnyHeader()              // Content-Type, Authorization, etc.
         .AllowAnyMethod()              // GET, POST, PATCH, DELETE...
         .AllowCredentials()            // si tu utilises des cookies / auth
         .WithExposedHeaders("ETag"));  // si tu renvoies des ETag (optionnel)
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Apply pending migrations on startup (with retries)
await app.ApplyMigrationsAsync(seed: true);

app.UseHttpsRedirection();

app.UseCors(DevCors);

app.UseSwagger();
app.UseSwaggerUI();

//Endpoints
app.MapTasks();
app.MapUsers();


app.Run();

public partial class Program { } // pour tests e2e si besoin