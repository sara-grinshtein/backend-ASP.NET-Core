using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using AutoMapper;
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
using NuGet.Protocol.Core.Types;
using Common.Dto.Common.Dto;


var builder = WebApplication.CreateBuilder(args);

// 1. שירותי Razor + Controllers
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

// 3. Authentication
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// 4. CORS – מאפשר גישה רק ל־localhost:3000
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 5. Dependency Injection
builder.Services.AddScoped<Irepository<Message>, MessageRepository>();
builder.Services.AddScoped<IService<VolunteerDto>, VolunteerService>();
builder.Services.AddScoped<IService<MessageDto>, MessageService>(); 
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<Irepository<KnowledgeCategory>, KnowledgeCategoryRepository>();
builder.Services.AddScoped<IService<KnowledgeCategoryDto>, KnowledgeCategoryService>();
builder.Services.AddScoped<My_areas_of_knowledge_Service>();
builder.Services.AddScoped<ManagerAlgorithm>();

// connect to DB 
builder.Services.AddDbContext<Icontext, DataBase>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHostedService<ScheduledCleanupService>();

builder.Services.AddService();
builder.Services.AddAutoMapper(typeof(MyMapper));

var app = builder.Build();

// 6. Middleware pipeline
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
