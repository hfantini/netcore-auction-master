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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================
namespace AuctionMaster.App.Exception
{

    // == CLASS
    // ==========================================================================

    /// <summary>
    /// Defines an structure of expcetion related with task execution.
    /// </summary>
    public class AuctionMasterTaskException : AuctionMasterException
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST

        // == VAR

        // == CONSTRUCTOR(S)
        // ======================================================================

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public AuctionMasterTaskException(ExceptionType type, String message) : base(type, message)
        {

        }


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public AuctionMasterTaskException(ExceptionType type, String message, System.Exception innerException) : base(type, message, innerException)
        {

        }

        // == METHOD(S)
        // ======================================================================

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
