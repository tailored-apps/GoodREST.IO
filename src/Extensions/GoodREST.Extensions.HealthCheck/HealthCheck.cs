using System;
using System.Collections.Generic;
using System.Text;
using GoodREST.Extensions.HealthCheck.Messages;
using GoodREST.Middleware;
using GoodREST.Middleware.Interface;
using GoodREST.Middleware.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace GoodREST.Extensions.HealthCheck
{
    public class HealthCheck : IExtension
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IRestModel restModel;
        public HealthCheck(IServiceCollection serviceCollection, IRestModel restModel)
        {
            this.serviceCollection = serviceCollection;
            this.restModel = restModel;
        }
        public void Install(RouteBuilder routeBuilder)
        {
            serviceCollection.AddScoped<ServiceBase, HealthCheckService>();
            restModel.RegisterMessageModel<Check>();
        }
    }
}
