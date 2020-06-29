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

using AuctionMaster.App.DTO;
using AuctionMaster.App.Service.Blizzard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================
namespace AuctionMaster.App.Controller
{
    /// <summary>
    /// Controller class used to perform tests in the application.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class MainController
    {
        // == CLASS
        // ==========================================================================

        // == DECLARATIONS
        // ======================================================================

        // == CONST

        // == VAR
        private IBlizzardRealmService _realmService;

        // == CONSTRUCTOR(S)
        // ======================================================================

        public MainController(IBlizzardRealmService realmService)
        {
            this._realmService = realmService;
        }

        // == METHOD(S)
        // ======================================================================
    
        [HttpGet]
        public IEnumerable<ApiResponse> get()
        {
            yield return new ApiResponse(200);
        }

        [HttpGet]
        [Route("realm")]
        public async Task<ApiResponse> getRealm()
        {
            var retValue = await this._realmService.getConnectedRealmList("us");
             return new ApiResponse( retValue.ToString(Newtonsoft.Json.Formatting.None) );
        }

        [HttpGet]
        [Route("realm/connected")]
        public async Task<ApiResponse> getConnectedRealm()
        {
            var retValue = await this._realmService.getConnectedRealmInformation(11, "us");
            return new ApiResponse(retValue.ToString(Newtonsoft.Json.Formatting.None));
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
