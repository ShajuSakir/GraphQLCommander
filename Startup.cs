  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommanderGQL.Data;
using CommanderGQL.GraphQL.Platforms;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommanderGQL
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        //DI
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //registering dbcontext with a service container
            //use AddPooledDbContextFactory instead of AddDbContext to enable multithreading
            services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseSqlServer
                (Configuration.GetConnectionString("CommandConString")));

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddType<PlatformType>()
                .AddType<CommandType>()
                //.AddProjections();
                .AddFiltering()
                .AddSorting()
                .AddInMemorySubscriptions();//allows us to manage and track subscription in memory

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                //Add graphql in the request pipeline
                endpoints.MapGraphQL();
            });


            //to use grapgql voyager
            // app.UseGraphQLVoyager(new GraphQLVoyagerOptions()
            // {
            //     GraphQLEndPoint = "/graphql",
            //     Path = "/graphql-voyager"
            // });

            app.UseGraphQLVoyager(new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql"
            }, "/graphql-voyager");
            
        }
    }
}
