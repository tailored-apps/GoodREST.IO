using GoodREST.Client;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using WebApplication;
using Xunit;

namespace GoodREST.Core.Tests.Commons
{
    public static class TestHost
    {
        private static readonly string HOST_LOCAL_TEST = @"http://localhost:4323/";
        private static IWebHost host;
        private static object lockObject = new object();

        public static IWebHost Host
        {
            get
            {
                lock (lockObject)
                {
                    if (host == null)
                    {
                        host = new WebHostBuilder()
                            .UseKestrel(x => { x.AllowSynchronousIO = true; })
                           .UseContentRoot(Directory.GetCurrentDirectory())
                           .UseEnvironment("dev")
                           .UseStartup<Startup>()
                           .UseUrls(HOST_LOCAL_TEST)
                           .Build();
                        host.Start();
                    }
                }
                return host;
                {
                }
            }
        }
    }

    public class CoreServiceTestsFixture : IDisposable
    {
        private readonly string HOST_LOCAL_TEST = @"http://localhost:4323/";
        private readonly IWebHost host;

        public CoreServiceTestsFixture()
        {
            host = TestHost.Host;

            Client = new TestClient(HOST_LOCAL_TEST);
        }

        public TestClient Client { private set; get; }

        public void Dispose()
        {
        }

        public class TestClient : JsonClient
        {
            public TestClient(string endpointAddress) : base(endpointAddress)
            {
            }

            public override R Put<R, K>(K request)
            {
                var routeAttribute = request.GetType().GetTypeInfo().GetCustomAttribute<GoodREST.Annotations.RouteAttribute>();
                Assert.NotNull(routeAttribute);
                if (routeAttribute.Verb.CompareTo(GoodREST.Enums.HttpVerb.PUT) != 0)
                {
                    throw new ArgumentException($"Expected PUT VERB, was {routeAttribute.Verb}");
                }

                return base.Put<R, K>(request);
            }

            public override R Delete<R, K>(K request)
            {
                var routeAttribute = request.GetType().GetTypeInfo().GetCustomAttribute<GoodREST.Annotations.RouteAttribute>();
                Assert.NotNull(routeAttribute);
                if (routeAttribute.Verb.CompareTo(GoodREST.Enums.HttpVerb.DELETE) != 0)
                {
                    throw new ArgumentException($"Expected DELETE VERB, was {routeAttribute.Verb}");
                }
                return base.Delete<R, K>(request);
            }

            public override R Get<R, K>(K request)
            {
                var routeAttribute = request.GetType().GetTypeInfo().GetCustomAttributes<GoodREST.Annotations.RouteAttribute>();
                Assert.NotNull(routeAttribute);
                if (routeAttribute.Any(x => x.Verb.CompareTo(GoodREST.Enums.HttpVerb.GET) != 0))
                {
                    throw new ArgumentException($"Expected GET VERB, was: {string.Join(", ", routeAttribute.Select(x => x.Verb))}");
                }
                return base.Get<R, K>(request);
            }

            public override R Post<R, K>(K request)
            {
                var routeAttribute = request.GetType().GetTypeInfo().GetCustomAttribute<GoodREST.Annotations.RouteAttribute>();
                Assert.NotNull(routeAttribute);
                if (routeAttribute.Verb.CompareTo(GoodREST.Enums.HttpVerb.POST) != 0)
                {
                    throw new ArgumentException($"Expected POST VERB, was {routeAttribute.Verb}");
                }
                return base.Post<R, K>(request);
            }

            public override R Patch<R, K>(K request)
            {
                var routeAttribute = request.GetType().GetTypeInfo().GetCustomAttribute<GoodREST.Annotations.RouteAttribute>();
                Assert.NotNull(routeAttribute);
                if (routeAttribute.Verb.CompareTo(GoodREST.Enums.HttpVerb.PATCH) != 0)
                {
                    throw new ArgumentException($"Expected Patch VERB, was {routeAttribute.Verb}");
                }
                return base.Patch<R, K>(request);
            }
        }
    }
}