using CurlCode.Application.Common.Mappings;
using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.Services.Auth;
using CurlCode.Application.Services.Problems;
using CurlCode.Application.Services.Profiles;
using CurlCode.Application.Services.Solutions;
using CurlCode.Application.Services.StudyPlans;
using CurlCode.Application.Services.Submissions;
using CurlCode.Application.Services.Topics;
using CurlCode.Infrastructure.Identity;
using CurlCode.Infrastructure.Persistence.Contexts;
using CurlCode.Infrastructure.Persistence.Repositories;
using CurlCode.Infrastructure.Services;
using CurlCode.API.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Serilog;
using StackExchange.Redis;
using FluentValidation;
using FluentValidation.AspNetCore;
using CurlCode.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity & JWT
builder.Services.AddIdentityServices(builder.Configuration);

// Email
builder.Services.AddScoped<IEmailSender, EmailSender>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Stack Exchange Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "CurlCode:";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// Application Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProblemService, ProblemService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<ISolutionService, SolutionService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IStudyPlanService, StudyPlanService>();


// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // context.Database.EnsureCreated(); // Replaced by Migrations
    if (context.Database.GetPendingMigrations().Any())
    {
        await context.Database.MigrateAsync();
    }
    await DataSeeder.SeedDataAsync(scope.ServiceProvider);
}

app.Run();
