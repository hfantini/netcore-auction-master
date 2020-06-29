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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================

namespace AuctionMaster.App.DTO
{

    // == CLASS
    // ==========================================================================

    public class ApiResponse
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST

        // == VAR
        private int _responseCode = 0;
        private ApiResponseType _responseType = 0;
        private String _responsePayload = null;

        // == CONSTRUCTOR(S)
        // ======================================================================

        /// <summary>
        /// Creates an response based in HTTP response code and a message.
        /// </summary>
        /// <param name="responseCode"></param>
        public ApiResponse(int responseCode, string message = null)
        {
            this._responseCode = responseCode;
            
            if(responseCode == 200)
            {
                this._responseType = ApiResponseType.OK;
            }
            else
            {
                this._responseType = ApiResponseType.FAIL;
            }

            this._responsePayload = message;
        }

        /// <summary>
        /// Creates an error response based in an exception.
        /// </summary>
        /// <param name="e">Exception</param>
        public ApiResponse(AuctionMasterException e)
        {
            this._responseCode = 500;
            this._responseType = ApiResponseType.FAIL;

            if (e != null)
            {
                this._responsePayload = e.ToString();
            }
        }

        /// <summary>
        /// Creates a response based in a payload object.
        /// </summary>
        /// <param name="responsePayload">Payload object</param>
        public ApiResponse(String responsePayload)
        {
            this._responseCode = 200;
            this._responseType = ApiResponseType.OK;
            this._responsePayload = responsePayload;
        }

        // == METHOD(S)
        // ======================================================================

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================

        public int responseCode
        {
            get { return this._responseCode; }
        }

        public ApiResponseType responseType
        {
            get { return this._responseType; }
        }

        public object responsePayload
        {
            get { return this._responsePayload; }
        }
    }
}
