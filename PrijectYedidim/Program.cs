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

// Program.cs

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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
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

// 6. Database connection
// Production  (Render / ASPNETCORE_ENVIRONMENT=Production) -> PostgreSQL (DefaultConnection)
// Development (local)                                      -> SQL Server   (SqlServerConnection or fallback local)
builder.Services.AddDbContext<Icontext, DataBase>(options =>
{
    if (builder.Environment.IsProduction())
    {
        // Production / Render => PostgreSQL
        var pgConnStr = builder.Configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(pgConnStr))
        {
            throw new InvalidOperationException("DefaultConnection (PostgreSQL) is missing in Production.");
        }

        options.UseNpgsql(pgConnStr);
    }
    else
    {
        // Local dev => SQL Server
        var sqlConnStr = builder.Configuration.GetConnectionString("SqlServerConnection");

        // Fallback hard-coded local connection if not provided in config
        if (string.IsNullOrWhiteSpace(sqlConnStr))
        {
            sqlConnStr =
                "Server=localhost;Database=project_yedidim1;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";
        }

        options.UseSqlServer(sqlConnStr);
    }
});

// 7. Hosted services, AutoMapper, and custom services
builder.Services.AddHostedService<ScheduledCleanupService>();
builder.Services.AddService();
builder.Services.AddAutoMapper(typeof(MyMapper));

var app = builder.Build();

// 7.5 Ensure database exists & migrations are applied (important in Production)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Icontext>() as DataBase;
    if (db != null)
    {
        // This will create the DB / tables in Postgres on first deploy
        db.Database.Migrate();
    }
}

// 8. Middleware pipeline
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
