using CurlCode.Application.Common.Mappings;
using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.Services.Auth;
using CurlCode.Application.Services.Contests;
using CurlCode.Application.Services.Problems;
using CurlCode.Application.Services.Profiles;
using CurlCode.Application.Services.Solutions;
using CurlCode.Application.Services.StudyPlans;
using CurlCode.Application.Services.Submissions;
using CurlCode.Application.Services.Topics;
using CurlCode.Application.Validators;
using CurlCode.Infrastructure.Identity;
using CurlCode.Infrastructure.Persistence.Contexts;
using CurlCode.Infrastructure.Persistence.Repositories;
using CurlCode.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace CurlCode.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // FluentValidation
        services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Application Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProblemService, ProblemService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<ISolutionService, SolutionService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IStudyPlanService, StudyPlanService>();
        services.AddScoped<IContestService, ContestService>();

        // Current User
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Identity & JWT
        services.AddIdentityServices(configuration);

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Email
        services.AddScoped<IEmailSender, EmailSender>();

        // Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "CurlCode:";
        });
        
        services.AddSingleton<IConnectionMultiplexer>(sp => 
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
        
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
