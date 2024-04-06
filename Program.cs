using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Tinder_Admin.Data;
using Tinder_Admin.Entities;
using Tinder_Admin.Helpers.Constants;
using Tinder_Admin.Services;
using Tinder_Admin.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls(AppConstants.URL);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect Database
builder.Services.AddDbContext<TinderCoreDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TinderCoreDBContext")
        ?? throw new InvalidOperationException("Connection string 'TinderCoreDBContext' not found.")));


// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<TinderCoreDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityCore<AppUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<TinderCoreDBContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

// Dependency Injection
builder.Services.AddScoped<IJWTService, JWTService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tinder API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
