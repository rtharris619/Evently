﻿using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Evently.Common.Application.EventBus;

namespace Evently.Common.Infrastructure.Inbox;

public static class IntegrationEventHandlersFactory
{
    private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

    public static IEnumerable<IIntegrationEventHandler> GetHandlers(
        Type type,
        IServiceProvider serviceProvider,
        Assembly assembly)
    {
        Type[] integrationEventHandlerTypes = HandlersDictionary.GetOrAdd(
            $"{assembly.GetName().Name}-{type.Name}",
            _ =>
            {
                Type[] integrationEventHandlers = assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler<>).MakeGenericType(type)))
                    .ToArray();

                return integrationEventHandlers;
            });

        List<IIntegrationEventHandler> handlers = [];
        foreach (Type integrationEventHandlerType in integrationEventHandlerTypes)
        {
            object integrationEventHandler = serviceProvider.GetRequiredService(integrationEventHandlerType);

            handlers.Add((integrationEventHandler as IIntegrationEventHandler)!);
        }

        return handlers;
    }
}
