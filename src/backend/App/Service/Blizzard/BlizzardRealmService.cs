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
    /// </summary>
    public class BlizzardRealmService : BlizzardService, IBlizzardRealmService
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
        public BlizzardRealmService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        // == METHOD(S)
        // ======================================================================

        /// <summary>
        /// Obtain a list from Blizzard API of the connected realm list.
        /// </summary>
        /// <param name="region">The region of the data to retrieve.</param>
        /// <returns>Task<JObject></returns>
        public async Task<JObject> getConnectedRealmList(String region)
        {
            JObject retValue = null;
            String qpRegion = region;
            String qpNamespace = "dynamic-us";
            String qpLocale = "en_us";
            String accessToken = await getClientAccessToken();
            String endpoint = $"https://{qpRegion}.api.blizzard.com/data/wow/connected-realm/index?namespace={qpNamespace}&locale={qpLocale}&access_token={accessToken}";

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, endpoint);
            HttpClient httpClient = this._httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                retValue = JObject.Parse( await response.Content.ReadAsStringAsync() );
            }
            else
            {
                throw new AuctionMasterBlizzardException(ExceptionType.FATAL, await response.Content.ReadAsStringAsync() );
            }

            return retValue;
        }

        /// <summary>
        /// Obtain detailed information about a connected realm.
        /// </summary>
        /// <param name="id">Connected realm ID</param>
        /// <param name="region">The region of the data to retrieve.</param>
        /// <returns>Task<JObject></returns>
        public async Task<JObject> getConnectedRealmInformation(int id, String region)
        {
            JObject retValue = null;
            int qpId = id;
            String qpRegion = region;
            String qpNamespace = "dynamic-us";
            String qpLocale = "en_us";
            String accessToken = await getClientAccessToken();
            String endpoint = $"https://{qpRegion}.api.blizzard.com/data/wow/connected-realm/{qpId}?namespace={qpNamespace}&locale={qpLocale}&access_token={accessToken}";

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
