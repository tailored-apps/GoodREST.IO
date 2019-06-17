using GoodREST.Extensions.ServiceDiscovery.DataModel.Messages;
using GoodREST.Middleware.Services;
using System;

namespace GoodREST.Extensions.ServiceDiscovery.Middleware.Services
{
    public class ServiceDiscoveryService : ServiceBase
    {
        private IServiceInfoPersister service;

        public ServiceDiscoveryService(IServiceInfoPersister service)
        {
            this.service = service;
        }

        public GetAllRegisteredServicesResponse Get(GetAllRegisteredServices request)
        {
            var response = new GetAllRegisteredServicesResponse();
            try
            {
                response.Services = service.GetServices();
                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }

            return response;
        }

        public PostRegisterInServiceDiscoveryResponse Post(PostRegisterInServiceDiscovery request)
        {
            var response = new PostRegisterInServiceDiscoveryResponse();
            try
            {
                var endpoint = new Model.EndpointHosts
                {
                    Port = request.Port,
                    ServiceHost = request.ServiceHost,
                    LastHealthCheckUtc = DateTime.Now,
                    ProcessId = request.ProcessId,
                };

                service.Register(request.AppDomainName, request.Version, request.ApiPrefix, request.Operations, endpoint);
                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }

            return response;
        }

        public GetAllOperationsResponse Get(GetAllOperations request)
        {
            var response = new GetAllOperationsResponse();
            try
            {
                response.Operations = service.GetOperations();
                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }
            return response;
        }
    }
}