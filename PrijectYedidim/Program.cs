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

var builder = WebApplication.CreateBuilder(args);

//  ������� ���� Razor Pages
builder.Services.AddRazorPages();

//  ����� Controllers ���������� API
builder.Services.AddControllers();

//  ���� ��Swagger � ���� ����� ����� �� constructor
builder.Services.AddEndpointsApiExplorer();

// ����� Swagger
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
});

// ������� ������� ���
builder.Services.AddService();
builder.Services.AddDbContext<Icontext, DataBase>();
builder.Services.AddAutoMapper(typeof(MyMapper));

var app = builder.Build();

//  ����� Swagger
app.UseSwagger();
app.UseSwaggerUI();

// ����� ���������
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ����� �� ��Controllers ��� ��Razor Pages
app.MapControllers();
app.MapRazorPages();

app.Run();
