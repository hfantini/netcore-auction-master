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
using AuctionMaster.App.Model;
using AuctionMaster.App.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================

namespace AuctionMaster.App.Service.Task
{
    // == CLASS
    // ==========================================================================

    public class ScheduledTaskLogService
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST

        // == VAR
        private Type _origin;
        private String _path;
        private ScheduledTaskLog _log;
        private StreamWriter _streamWriter;

        // == CONSTRUCTOR(S)
        // ======================================================================

        public ScheduledTaskLogService(Type origin, ScheduledTaskLog log)
        {
            this._origin = origin;
            this._log = log;
        }

        // == METHOD(S)
        // ======================================================================

        public void start()
        {
            try
            {
                // == DEFINE THE FINAL PATH 
                this._path = AppDomain.CurrentDomain.BaseDirectory + $"\\log\\{this._log.GetType().Name}/";

                // == CHECKS THE EXISTENCE OF DIRECTORY

                if ( !Directory.Exists(this._path) )
                {
                    Directory.CreateDirectory(this._path);
                }

                // == CREATES THE FILE NAME

                String fileName = $"{this._log.GetType().Name}_{this._log.Id}_{DateTimeUtil.getDateYYYYMMDD(this._log.StartTime, "-", "", "")}.txt";

                // == CREATES THE STREAM

                this._streamWriter = new StreamWriter(this._path + fileName, true);
                
            }
            catch(System.Exception e)
            {
                throw e;
            }
        }

        public void writeLine(LogType type, String message, Boolean showOnConsole = false)
        {
            String logMessage = null;

            if(showOnConsole)
            {
                logMessage = LogUtil.writeLog(this._origin, type, message);
            }
            else
            {
                logMessage = LogUtil.createLog(this._origin, type, message);
            }

            this._streamWriter.WriteLine(logMessage);
        }

        public void save()
        {
            this._streamWriter.Flush();
        }

        public void finish()
        {
            this._streamWriter.Close();
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
