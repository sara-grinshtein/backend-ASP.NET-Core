using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Service.service;
using Repository.interfaces;
using Mock;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Repository.Entites;
using Repository.Repositories;
using Common.Dto;
using Service.interfaces;
using Service.Algorithm;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1. Razor + Controllers
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// 3. JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

// 4. CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 5. Dependency Injection setup
builder.Services.AddScoped<Irepository<Message>, MessageRepository>();
builder.Services.AddScoped<IService<VolunteerDto>, VolunteerService>();
builder.Services.AddScoped<IService<MessageDto>, MessageService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<Irepository<KnowledgeCategory>, KnowledgeCategoryRepository>();
builder.Services.AddScoped<IService<KnowledgeCategoryDto>, KnowledgeCategoryService>();
builder.Services.AddScoped<My_areas_of_knowledge_Service>();
builder.Services.AddScoped<ManagerAlgorithm>();

// 6. Database connection selection
// -------------------------------------------
// Production (Render)  -> connect to PostgreSQL
// Development (local)  -> connect to SQL Server
// -------------------------------------------
var isProduction = builder.Environment.IsProduction();

// Note: Make sure you have these connection strings in your configuration files:
// "PostgresConnection"  -> for PostgreSQL (Production)
// "SqlServerConnection" -> for SQL Server (Development)
builder.Services.AddDbContext<Icontext, DataBase>(options =>
{
    if (isProduction)
    {
        var pgConnStr = builder.Configuration.GetConnectionString("PostgresConnection");
        if (string.IsNullOrWhiteSpace(pgConnStr))
        {
            throw new InvalidOperationException("PostgresConnection is missing for Production environment.");
        }

        options.UseNpgsql(pgConnStr);
    }
    else
    {
        var sqlConnStr = builder.Configuration.GetConnectionString("SqlServerConnection");
        if (string.IsNullOrWhiteSpace(sqlConnStr))
        {
            throw new InvalidOperationException("SqlServerConnection is missing for Development environment.");
        }

        options.UseSqlServer(sqlConnStr);
    }
});

// 7. Hosted services, AutoMapper, and custom services
builder.Services.AddHostedService<ScheduledCleanupService>();
builder.Services.AddService();
builder.Services.AddAutoMapper(typeof(MyMapper));

var app = builder.Build();

// 8. Middleware pipeline
// Swagger UI (always enabled; you can hide it in production if needed)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
