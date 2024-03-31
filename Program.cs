using Tinder_Admin.Helpers.Constants;
using Tinder_Admin.Services;
using Tinder_Admin.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls(AppConstants.URL);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Dependency Injection
builder.Services.AddScoped<IJWTService, JWTService>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
