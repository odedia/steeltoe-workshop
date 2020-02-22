﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Steeltoe.Management.CloudFoundry;

namespace MySqlEFCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Context and use MySql as provider ... provider will be configured from VCAP_ info
            if (Configuration.GetValue<bool>("multipleMySqlDatabases"))
            {
                // For multiple databases, specify the service binding name
                // review appsettings.development.json to see how local connection info is provided
                services.AddDbContext<TestContext>(options => options.UseMySql(Configuration, "myMySqlService"));
                services.AddMySqlHealthContributor(Configuration, "myMySqlService");
                services.AddDbContext<SecondTestContext>(options => options.UseMySql(Configuration, "myOtherMySqlService"));
                services.AddMySqlHealthContributor(Configuration, "myOtherMySqlService");
            }
            else
            {
                services.AddDbContext<TestContext>(options => options.UseMySql(Configuration));
                services.AddMySqlHealthContributor(Configuration);
            }

            // Add framework services.
#if NETCOREAPP3_0
            services.AddControllersWithViews();
#else
            services.AddMvc();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

#if NETCOREAPP3_0
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
#else
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
#endif
        }
    }
}
