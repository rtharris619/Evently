using Evently.Api.Extensions;
using Evently.Modules.Events.Infrastructure;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Serilog;
using Evently.Api.Middleware;
using HealthChecks.UI.Client;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Users.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using System.Reflection;
using Evently.Modules.Attendance.Infrastructure;
using Evently.Common.Infrastructure.Configuration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Assembly[] moduleApplicationAssemblies = [    
    Evently.Modules.Events.Application.AssemblyReference.Assembly,
    Evently.Modules.Users.Application.AssemblyReference.Assembly,
    Evently.Modules.Ticketing.Application.AssemblyReference.Assembly,
    Evently.Modules.Attendance.Application.AssemblyReference.Assembly
];

builder.Services.AddApplication(moduleApplicationAssemblies);

string databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");
string redisConnectionString = builder.Configuration.GetConnectionStringOrThrow("Cache");

builder.Services.AddInfrastructure(
    [
        EventsModule.ConfigureConsumers(redisConnectionString),
        TicketingModule.ConfigureConsumers,
        AttendanceModule.ConfigureConsumers
    ],
    databaseConnectionString,
    redisConnectionString);

Uri keyCloakHealthUrl = builder.Configuration.GetKeyCloakHealthUrl();

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString)
    .AddKeyCloak(keyCloakHealthUrl);

builder.Configuration.AddModuleConfiguration(["events", "users", "ticketing", "attendance"]);

builder.Services.AddEventsModule(builder.Configuration);

builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddTicketingModule(builder.Configuration);

builder.Services.AddAttendanceModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapHealthChecks("health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

// Order is important!
app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();
