using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Docs.Swagger;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Availability.Application;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Events.External;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Infrastructure.Decorators;
using Pacco.Services.Availability.Infrastructure.Exceptions;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.Infrastructure.Mongo.Repositories;
using Pacco.Services.Availability.Infrastructure.Services;
using System;

namespace Pacco.Services.Availability.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInstrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IResourcesRepository, ResourcesMongoRepository>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IEventProcessor, EventProcessor>();
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

            builder.Services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            return builder
                       .AddQueryHandlers()
                       .AddInMemoryQueryDispatcher()
                       .AddErrorHandler<ExceptionToResponseMapper>()
                       .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                       .AddMongo()
                       .AddMongoRepository<ResourceDocument, Guid>("resources")
                       .AddRabbitMq()
                       .AddSwaggerDocs()
                       .AddWebApiSwaggerDocs()
                       .AddMessageOutbox(o => o.AddMongo());
        }

        public static IApplicationBuilder UseInstrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey()
                .UsePublicContracts<ContractAttribute>()
                .UseSwaggerDocs()
                .UseRabbitMq()
                .SubscribeEvent<SignedUp>();

            return app;
        }
    }
}
