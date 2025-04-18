﻿using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Infrastructure.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using MassTransit;
using Evently.Modules.Ticketing.Presentation.Customers;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.Infrastructure.Events;
using Evently.Modules.Ticketing.Infrastructure.Orders;
using Evently.Modules.Ticketing.Infrastructure.Payments;
using Evently.Modules.Ticketing.Infrastructure.Tickets;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Common.Infrastructure.Outbox;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicketingDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Ticketing))
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>())
                .UseSnakeCaseNamingConvention());

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketingDbContext>());

        services.AddSingleton<CartService>();
        services.AddSingleton<IPaymentService, PaymentService>();
    }
}
