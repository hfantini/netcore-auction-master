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
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================
namespace AuctionMaster.App.Util
{
    // == CLASS
    // ==========================================================================

    /// <summary>
    /// Provides methods related with log (Console and Files).
    /// </summary>
    public class LogUtil
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
        /// <param name="type">type of the log</param>
        /// <param name="message">message</param>
        /// <returns>String</returns>
        public static String createLog(Type origin, LogType type, String message)
        {
            String retValue = null;

            if (origin == null)
            {
                origin = typeof(LogUtil);
            }

            DateTime dateTime = DateTime.Now;
            retValue = $"{DateTimeUtil.getDateYYYYMMDD(DateTime.Now)} -> [{type.ToString()}] [{origin.Name}]: {message}".ToUpper();

            return retValue;
        }

        /// <summary>
        /// Creates a log message from an exception.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <returns>String</returns>
        public static String createLog(AuctionMasterException e)
        {
            return null;
        }

        /// <summary>
        /// Writes a log in the console.
        /// </summary>
        /// <returns>String</returns>
        public static String writeLog(Type origin, LogType type, String message)
        {
            String retvalue = LogUtil.createLog(origin, type, message);

            switch (type)
            {
                case LogType.INFO:
                    Console.ForegroundColor = ConsoleColor.Blue;
                break;

                case LogType.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                break;

                case LogType.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                break;
            }

            Console.WriteLine( LogUtil.createLog(origin, type, message) );
            Console.ResetColor();

            return retvalue;
        }

        public static void writeSoftwareHeader()
        {
            Console.WriteLine(
$@"

█████╗ ██╗   ██╗ ██████╗████████╗██╗ ██████╗ ███╗   ██╗    ███╗   ███╗ █████╗ ███████╗████████╗███████╗██████╗ 
██╔══██╗██║   ██║██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║    ████╗ ████║██╔══██╗██╔════╝╚══██╔══╝██╔════╝██╔══██╗
███████║██║   ██║██║        ██║   ██║██║   ██║██╔██╗ ██║    ██╔████╔██║███████║███████╗   ██║   █████╗  ██████╔╝
██╔══██║██║   ██║██║        ██║   ██║██║   ██║██║╚██╗██║    ██║╚██╔╝██║██╔══██║╚════██║   ██║   ██╔══╝  ██╔══██╗
██║  ██║╚██████╔╝╚██████╗   ██║   ██║╚██████╔╝██║ ╚████║    ██║ ╚═╝ ██║██║  ██║███████║   ██║   ███████╗██║  ██║
╚═╝  ╚═╝ ╚═════╝  ╚═════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝    ╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝

Version: {typeof(LogUtil).Assembly.GetName().Version.ToString()}
Created by Henrique Fantini (contact@henriquefantini.com)

------------ X --------------

"
            );
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
