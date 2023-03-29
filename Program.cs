using HD_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using HD_Backend.Filters.ActionFilters;
using HD_Backend.Data.Entities;
using HD_Backend.Data.Services;
using HD_Backend.Data.Interfaces;
using HD_Backend.Extensions;

var builder = WebApplication.CreateBuilder(args);

LoggerManager logger = new LoggerManager();

builder.Services.ConfigureResponseCaching();

builder.Services.RegisterDependencies();
builder.Services.ConfigureMapping();
builder.Services.ConfigureLoggerService();

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();


builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.ConfigureControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
