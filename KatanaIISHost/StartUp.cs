using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Owin;

namespace KatanaIISHost
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class StartUp
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            //Middleware
            appBuilder.Use(async (enviroment, next) =>
            {
                Console.WriteLine($"Requesting : {enviroment.Request.Path}");

                await next();

                Console.WriteLine($"Response : {enviroment.Response.StatusCode}");
            });

            //Configure to use WebApi
            ConfigureWebApi(appBuilder);

            // Middleware - Low Level Component
            appBuilder.UseHelloWorld();
        }

        private void ConfigureWebApi(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi", 
                "api/{controller}/{id}", 
                new {id = RouteParameter.Optional});

            appBuilder.UseWebApi(config);
        }
    }

    public static class AppBuilderExtensions
    {
        public static void UseHelloWorld(this IAppBuilder appBuilder)
        {
            appBuilder.Use<HelloWorldComponent>();
        }
    }

    public class HelloWorldComponent
    {
        private readonly AppFunc _Next;

        public HelloWorldComponent(AppFunc next)
        {
            _Next = next;
        }

        public Task Invoke(IDictionary<string, object> enviroment)
        {
            var response = enviroment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response ?? throw new InvalidOperationException()))
                return writer.WriteAsync("Hello World");

        }
    }

}
