using CaptureMatchApi.Data;
using CaptureMatchApi.Entities;
using CaptureMatchApi.Extensions;
using CaptureMatchApi.Helpers;
using CaptureMatchApi.Interfaces;
using CaptureMatchApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//async Task SeedRoles(RoleManager<AppRole> roleManager)
//{
//    var roles = new List<AppRole>
//    {
//        new() {Name = "Client"},
//        new() {Name = "Photographer"},
//    };

//    foreach (var role in roles)
//    {

//        await roleManager.CreateAsync(role);
//    }
//}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

    var roles = new List<AppRole>
    {
        new() {Name = "Client"},
        new() {Name = "Photographer"},
    };

    foreach (var role in roles)
    {
            await roleManager.CreateAsync(role);

    }
    //await SeedRoles(roleManager);
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
