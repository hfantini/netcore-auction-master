/*
 *  + - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
 *  
 *  == AUCTION MASTER ==
 *  
 *  Author: Henrique Fantini
 *  Contact: contact@henriquefantini.com
 * 
 *  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
 *  
 *  Auction Master is a software with the objective to collect and process data 
 *  from World of Warcraft's auction house. The idea behind this is display data 
 *  and graphics about the market of each realm and analyse your movimentation.
 *  
 *  + - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
 */

// == IMPORTS
// ==============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AuctionMaster.App.Model;
using AuctionMaster.App.Service.Blizzard;
using AuctionMaster.App.Service.Task;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// == NAMESPACE
// ==============================================================================

namespace AuctionMaster.Backend
{
    // == CLASS
    // ==========================================================================

    /// <summary>
    /// Class responsible for initilization and configuration of this web service.
    /// </summary>
    public class Startup
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST

        // == VAR
        public IConfiguration Configuration { get; }
        private IServiceCollection serviceColletion = null;

        // == CONSTRUCTOR(S)
        // ======================================================================

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // == METHOD(S)
        // ======================================================================

        public void ConfigureServices(IServiceCollection serviceColletion)
        {
            this.serviceColletion = serviceColletion;

            // == ADD DATABASE CONTEXT
            this.serviceColletion.AddDbContext<DatabaseContext>(options => options.UseMySql( Configuration.GetConnectionString("AuctionMasterDatabase") ) );

            // == ADD HTTP CLIENT
            this.serviceColletion.AddHttpClient();

            // == ADD CONTROLLERS
            this.serviceColletion.AddControllers();

            // == ADD SERVICES
            this.serviceColletion.AddSingleton<IBlizzardRealmService, BlizzardRealmService>();
            this.serviceColletion.AddSingleton<IBlizzardAuctionHouseService, BlizzardAuctionHouseService>();
            this.serviceColletion.AddSingleton<IBlizzardItemService, BlizzardItemService>();
            this.serviceColletion.AddSingleton<IScheduledTaskService, ScheduledTaskService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // SERVICES
            IScheduledTaskService scheduledTaskService = app.ApplicationServices.GetService<IScheduledTaskService>();
            scheduledTaskService.initTaskService();

            // HTTP

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    string version = typeof(Startup).Assembly.GetName().Version.ToString();
                    await context.Response.WriteAsync($"AuctionMaster BACKEND - {version}");
                });
            });
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
