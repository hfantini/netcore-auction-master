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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================
namespace AuctionMaster.App.Service.Blizzard
{

    // == CLASS
    // ==========================================================================

    /// <summary>
    /// Defines a service which implements blizzard API calls.
    /// </summary>
    public abstract class BlizzardService
    {

        // == DECLARATIONS
        // ======================================================================

        // == CONST

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        // == VAR

        private DateTime _accessTokenUpdateDateTime;
        private JObject _accessToken;

        // == CONSTRUCTOR(S)
        // ======================================================================

        /// <summary>
        /// Class constructor
        /// </summary>
        public BlizzardService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this._configuration = configuration;
            this._httpClientFactory = httpClientFactory;
        }

        // == METHOD(S)
        // ======================================================================

        /// <summary>
        /// Request an access_token from Blizzard OAuth using the ClientID and Secret provided in configuration file.
        /// </summary>
        /// <returns></returns>
        private async Task<JObject> requestNewClientAccessToken()
        {
            JObject retValue = null;

            String cID = clientID;
            String cSecret = secret;

            if (cID == null)
            {
                throw new AuctionMasterBlizzardException(ExceptionType.FATAL, "Client ID not defined.");
            }

            if (cSecret == null)
            {
                throw new AuctionMasterBlizzardException(ExceptionType.FATAL, "Client secret not defined.");
            }

            // CREATE REQUEST CONTENT
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

            // ENCODE CREDENTIALS
            String authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cID}:{cSecret}"));

            // MAKE AN HTTP REQUEST
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://us.battle.net/oauth/token")
            {
                Content = new FormUrlEncodedContent(nvc)
            };

            request.Headers.Add("Authorization", $"Basic {authorization}");

            HttpClient client = this._httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                retValue = this._accessToken = JObject.Parse(await response.Content.ReadAsStringAsync());
                this._accessTokenUpdateDateTime = DateTime.Now;
            }
            else
            {
                throw new AuctionMasterBlizzardException(ExceptionType.FATAL, "Can't obtain an access_token from Blizzard." + response.Content.ToString() );
            }

            return retValue;
        }

        /// <summary>
        /// Obtain an access_token that allows to consume Blizzard API's.
        /// If a valid token has been generated before this one will be returned, or else a new one will be request using ClientID and Secret.
        /// </summary>
        /// <returns>String</returns>
        protected async Task<String> getClientAccessToken()
        {
            String retValue = null;

            if(this._accessToken == null)
            {
                // OBTAIN NEW TOKEN

                await this.requestNewClientAccessToken();
            }
            else
            {
                if (this._accessTokenUpdateDateTime != null)
                {
                    int expireTime = Convert.ToInt32(this._accessToken["expires_in"].Value<String>());

                    if ( (new DateTime() - this._accessTokenUpdateDateTime).TotalSeconds > expireTime)
                    {
                        await this.requestNewClientAccessToken();
                    }
                }
                else
                {
                    await this.requestNewClientAccessToken();
                }
            }

            // RETURN CURRENT TOKEN

            retValue = this._accessToken["access_token"].Value<String>();

            return retValue;
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================

        public String clientID
        {
            get
            {
                return this._configuration.GetSection("blizzard").GetValue<String>("clientID");
            }
        }

        public String secret
        {
            get
            {
                return this._configuration.GetSection("blizzard").GetValue<String>("secret");
            }
        }
    }
}
