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

        public IConfiguration Configuration { get; }

        // == VAR

        // == CONSTRUCTOR(S)
        // ======================================================================

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // == METHOD(S)
        // ======================================================================

        public void ConfigureServices(IServiceCollection services)
        {
            // ADD DATABASE CONTEXT
            services.AddDbContext<DatabaseContext>(options => options.UseMySql( Configuration.GetConnectionString("AuctionMasterDatabase") ) );

            // ADD CONTROLLERS
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
