using Convey;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Convey.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Infrastructure.Exceptions;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.Infrastructure.Mongo.Repositories;
using System;

namespace Pacco.Services.Availability.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInstrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IResourcesRepository, ResourcesMongoRepository>();
            return builder
                       .AddQueryHandlers()
                       .AddInMemoryQueryDispatcher()
                       .AddErrorHandler<ExceptionToResponseMapper>()
                       .AddMongo()
                       .AddMongoRepository<ResourceDocument, Guid>("resources");
        }

        public static IApplicationBuilder UseInstrastructure(this IApplicationBuilder app)
        {
            app
                .UseErrorHandler()
                .UseConvey();

            return app;
        }
    }
}
