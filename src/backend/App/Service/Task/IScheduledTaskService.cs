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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================

namespace AuctionMaster.App.Service.Task
{
    // == CLASS
    // ==========================================================================

    /// <summary>
    /// Interface of ScheduledTaskService
    /// </summary>
    public interface IScheduledTaskService
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST

        // == VAR

        // == CONSTRUCTOR(S)
        // ======================================================================

        // == METHOD(S)
        // ======================================================================

        void initTaskService();

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
