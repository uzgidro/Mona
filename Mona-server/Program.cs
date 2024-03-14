using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mona.Context;
using Mona.Hub;
using Mona.Model;
using Mona.Service;
using Mona.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<UserContext>();
builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });
builder.Services.AddControllers();
builder.Services.AddSqlite<MessageContext>("Data Source=UGEChat.db");
builder.Services.AddSqlite<UserContext>("Data Source=UGEChat.db");
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new JwtService(builder.Configuration).GetValidationParameters();
});
builder.Services.AddAuthorization();

var services = builder.Services;
var configuration = builder.Configuration;
var env = builder.Environment;

// services.AddTransient<ValidateMimeMultipartContentFilter>();

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

services.AddSignalR()
    .AddMessagePackProtocol();
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
app.MapIdentityApi<ApplicationUser>();
app.MapHub<SimpleHub>("/chat");
app.MapControllers();
app.Run();