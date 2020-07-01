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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================
namespace AuctionMaster.App.Util
{
    // == CLASS
    // ==========================================================================

    /// <summary>
    /// Provides methods related with DateTime operations.
    /// </summary>
    public class DateTimeUtil
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST

        // == VAR

        // == CONSTRUCTOR(S)
        // ======================================================================

        // == METHOD(S)
        // ======================================================================

        /// <summary>
        /// Creates a log message.
        /// </summary>
        /// <param name="type">type of the log (Should be lower than LogType.Error)</param>
        /// <param name="message">message</param>
        /// <returns>String</returns>
        public static String getDateYYYYMMDD(DateTime dateTime, String separator = " ", String dateSeparator = "/", String timeSeparator = ":")
        {
            String retValue = null;

            if(dateTime != null)
            {
                String year = dateTime.Year.ToString();
                String month = (dateTime.Month.ToString().Length == 1 ? "0" + dateTime.Month.ToString() : dateTime.Month.ToString() );
                String day = (dateTime.Day.ToString().Length == 1 ? "0" + dateTime.Day.ToString() : dateTime.Month.ToString());

                String hour = (dateTime.Hour.ToString().Length == 1 ? "0" + dateTime.Hour.ToString() : dateTime.Hour.ToString());
                String minute = (dateTime.Minute.ToString().Length == 1 ? "0" + dateTime.Minute.ToString() : dateTime.Minute.ToString());
                String second = (dateTime.Second.ToString().Length == 1 ? "0" + dateTime.Second.ToString() : dateTime.Second.ToString());

                retValue = $"{year}{dateSeparator}{month}{dateSeparator}{day}{separator}{hour}{timeSeparator}{minute}{timeSeparator}{second}";
            }

            return retValue;
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
