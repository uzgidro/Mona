using Mona.Context;
using Mona.Hub;
using Mona.Service;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });

builder.Services.AddSqlite<MessageContext>("Data Source=UGEChat.db");
builder.Services.AddScoped<IMessageService, MessageService>();

var services = builder.Services;
var configuration = builder.Configuration;
var env = builder.Environment;

// services.AddTransient<ValidateMimeMultipartContentFilter>();

var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");


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
app.UseCors("AllowAllOrigins");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapHub<SimpleHub>("/chat");
app.Run();