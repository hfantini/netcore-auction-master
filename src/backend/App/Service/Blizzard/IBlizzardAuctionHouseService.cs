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

using AuctionMaster.App.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================

namespace AuctionMaster.App.Service.Blizzard
{
    // == INTERFACE
    // ==========================================================================
    
    /// <summary>
    /// A service who handles requests related with World of Warcraft information
    /// </summary>
    public interface IBlizzardAuctionHouseService
    {
        // == METHOD(S)
        // ======================================================================

        public Task<JObject> getAuctionList(ConnectedRealm cr);

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
