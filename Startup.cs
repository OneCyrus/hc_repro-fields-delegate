using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;

namespace repro_fields_delegate
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            HostingEnvironment = env;
        }

        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddCors(options => options.AddPolicy("GraphPolicy", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            }));

            var networkSdl = Path.Combine(HostingEnvironment.ContentRootPath, "GIA.WebServices.Network.graphql");
            services.AddHttpClient("network", (serviceProvider, client) =>
            {
                client.BaseAddress = (!HostingEnvironment.IsDevelopment()) ? new Uri("https://network.webservices.gclsintra.net/graphql") : new Uri("http://localhost:5000/graphql");
                client.DefaultRequestHeaders.Add("Authorization", GetBearerToken(serviceProvider));
            });

            // If you need dependency injection with your query object add your query type as a services.
            // services.AddSingleton<Query>();
            services
                .AddRouting()
                .AddGraphQLServer()                
                .AddQueryType<Query>()
                .AddRemoteSchemaFromFile("network", networkSdl, true)
                .AddTypeExtensionsFromFile(Path.Combine(HostingEnvironment.ContentRootPath, "NetworkSchemaExtension.graphql"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("GraphPolicy");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // By default the GraphQL server is mapped to /graphql
                // This route also provides you with our GraphQL IDE. In order to configure the
                // the GraphQL IDE use endpoints.MapGraphQL().WithToolOptions(...).
                endpoints.MapGraphQL();
            });
        }

        private string GetBearerToken(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var bearerToken = context
                .Request
                .Headers["Authorization"]
                .FirstOrDefault(h => h.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase));

            return bearerToken;
        }
    }
}
