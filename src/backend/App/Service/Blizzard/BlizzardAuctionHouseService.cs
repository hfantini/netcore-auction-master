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

using AuctionMaster.App.Enumeration;
using AuctionMaster.App.Exception;
using AuctionMaster.App.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================

namespace AuctionMaster.App.Service.Blizzard
{
    // == CLASS
    // ==========================================================================
    
    /// <summary>
    /// A service who handles requests related with World of Warcraft information
    /// about Auction House.
    /// </summary>
    public class BlizzardAuctionHouseService : BlizzardService, IBlizzardAuctionHouseService
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST

        private readonly IHttpClientFactory _httpClientFactory;

        // == VAR

        // == CONSTRUCTOR(S)
        // ======================================================================

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="configuration">Configuration object</param>
        public BlizzardAuctionHouseService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        // == METHOD(S)
        // ======================================================================

        public async Task<JObject> getAuctionList(ConnectedRealm cr)
        {
            JObject retValue = null;
            String qpRegion = "us";
            String qpNamespace = "dynamic-us";
            String qpLocale = "en_us";
            String accessToken = await getClientAccessToken();
            String endpoint = $"https://{qpRegion}.api.blizzard.com/data/wow/connected-realm/{cr.Id}/auctions?namespace={qpNamespace}&locale={qpLocale}&access_token={accessToken}";

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, endpoint);
            HttpClient httpClient = this._httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                retValue = JObject.Parse(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new AuctionMasterBlizzardException(ExceptionType.FATAL, await response.Content.ReadAsStringAsync());
            }

            return retValue;
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
