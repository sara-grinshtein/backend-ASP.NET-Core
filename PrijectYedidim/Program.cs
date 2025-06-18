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

var builder = WebApplication.CreateBuilder(args);

//  שירותים עבור Razor Pages
builder.Services.AddRazorPages();

//  הוספת Controllers לאפליקציית API
builder.Services.AddControllers();

//  חובה ל־Swagger – אחרת תופיע שגיאה של constructor
builder.Services.AddEndpointsApiExplorer();

// רישום Swagger
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

// שירותים פנימיים שלך
builder.Services.AddService();
builder.Services.AddDbContext<Icontext, DataBase>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(option =>
              option.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = builder.Configuration["Jwt:Issuer"],
                  ValidAudience = builder.Configuration["Jwt:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

              });

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//cores
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});
builder.Services.AddScoped<Irepository<Message>,MessageRepository>();
builder.Services.AddScoped<IService<VolunteerDto>, VolunteerService>();


builder.Services.AddAutoMapper(typeof(MyMapper));

var app = builder.Build();

//  הפעלת Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API V1");
    c.RoutePrefix = string.Empty; // 👈 חשוב! זה פותח את Swagger ישירות ב- /
});

// תשתית האפליקציה
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);

// מיפוי גם ל־Controllers וגם ל־Razor Pages
app.MapControllers();
app.MapRazorPages();

app.Run();
