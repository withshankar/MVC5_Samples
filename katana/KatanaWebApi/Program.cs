﻿using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KatanaWebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:8080/";

            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Started");
                Console.ReadKey();
                Console.WriteLine("Stopping");
            }
        }      
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            InstallMiddleware(app);

            var config = new HttpConfiguration();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
           
            app.UseWebApi(config);
        }

        private static void InstallMiddleware(IAppBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                // pre handler
                Console.WriteLine("Getting request " + ctx.Request.Path);

                await next();

                // post handler

                Console.WriteLine("\tSending response: " + ctx.Response.StatusCode);

            });
        }
    }
}
