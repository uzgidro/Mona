using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Mona.Config;
using Mona.Context;
using Mona.Hub;
using Mona.Model;
using Mona.Service;
using Mona.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityApiEndpoints<UserModel>()
    .AddEntityFrameworkStores<ApplicationContext>();
builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });
builder.Services.AddControllers();
builder.Services.AddSqlite<ApplicationContext>("Data Source=UGEChat.db");
builder.Services
    .AddScoped<IMessageService, MessageService>()
    .AddScoped<ICryptoService, CryptoService>()
    .AddScoped<IJwtService, JwtService>()
    .AddScoped<IFileService, FileService>()
    .AddScoped<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new JwtService(builder.Configuration).GetValidationParameters();
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

var services = builder.Services;
var configuration = builder.Configuration;
var env = builder.Environment;

configuration.GetConnectionString("DefaultConnection");


services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policyBuilder =>
        {
            policyBuilder
                .AllowCredentials()
                .WithOrigins(
                    "http://localhost:4200")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

services.AddSignalR();
services.AddSingleton<IUserIdProvider, UserIdProvider>();
services.AddControllersWithViews();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAllOrigins");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<UserModel>();
app.MapHub<SimpleHub>("/hub");
app.MapControllers();
app.Run();