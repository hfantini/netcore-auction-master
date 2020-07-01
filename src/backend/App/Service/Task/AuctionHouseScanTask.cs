
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
using AuctionMaster.App.Service.Blizzard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================
namespace AuctionMaster.App.Service.Task
{
    // == CLASS
    // ==========================================================================

    public class AuctionHouseScanTask : GenericScheduledTask
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST
        private readonly IBlizzardAuctionHouseService _blizzardAuctionHouseService;

        // == VAR
        private IServiceScope _scope;
        private DatabaseContext _databaseContext;
        private IDbContextTransaction _dbTransaction;
        private ScheduledTaskLog _taskLog;
        private int _auctionScanCount;

        // == CONSTRUCTOR(S)
        // ======================================================================

        public AuctionHouseScanTask(IServiceScopeFactory scopeFactory, ScheduledTask scheduledTask, IBlizzardAuctionHouseService blizzardAuctionHouseService) : base(scopeFactory, scheduledTask)
        {
            this._blizzardAuctionHouseService = blizzardAuctionHouseService;
        }

        // == METHOD(S)
        // ======================================================================

        protected override void onStart()
        {
            base.onStart();

            this._auctionScanCount = 0;

            // == CREATES THE SCOPE & DATABASE CONTEXT

            this._scope = this._scopeFactory.CreateScope();
            this._databaseContext = this._scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            // == CREATES THE ENTRY OF THIS SCAN ON DATABASE

            var currentTime = DateTime.Now;

            this._dbTransaction = this._databaseContext.Database.BeginTransaction();

            // SCHEDULED_TASK_LOG

            this._taskLog = new ScheduledTaskLog();
            this._taskLog.StartTime = currentTime;
            this._taskLog.ScheduledTask = this._scheduledTask.Id;
            this._databaseContext.ScheduledTaskLog.Add(this._taskLog);

            this._databaseContext.SaveChanges();

            this._dbTransaction.Commit();
            this._dbTransaction.Dispose();
            this._dbTransaction = null;
        }

        private void fullAuctionHouseScan(int connectedRealmID)
        {
            ConnectedRealm connectedRealm = this._databaseContext.ConnectedRealm.Find(connectedRealmID);

            if(connectedRealm == null)
            {
                Task<JObject> auctionTask = this._blizzardAuctionHouseService.getAuctionList(connectedRealm);
                auctionTask.Wait();

                JObject auctionList = auctionTask.Result;
                int value = auctionList.Count;

                // == PROCESSING AUCTION HOUSE INFORMATION

                ToString();
            }
            else
            {
                throw new AuctionMasterTaskException(ExceptionType.FATAL, "Invalid Connected Realm ID. Did you scanned realms before run this task?");
            }
        }

        protected override void onExecute(JObject param, CancellationToken cancellationToken)
        {
            base.onExecute(param, cancellationToken);

            // == PARAM VERIFICATION

            if(!param.ContainsKey("cr"))
            {
                throw new AuctionMasterTaskException(ExceptionType.FATAL, "The parameter 'cr' is missing and can't execute the task.");
            }

            var cr = param.GetValue("cr").Value<int>();
            fullAuctionHouseScan(cr);
        }

        protected override void onFinish()
        {
            base.onFinish();

            // == WRITES LOG ABOUT THE EXECUTION AND UPDATE TASK DATA

            var taskLog = this._databaseContext.ScheduledTaskLog.Attach(this._taskLog);
            this._taskLog.Status = 1;
            this._taskLog.EndTime = DateTime.Now;

            JObject message = new JObject();
            message.Add("auctionCount", this._auctionScanCount);

            this._taskLog.Message = message.ToString();
            this._databaseContext.Entry(this._taskLog).State = EntityState.Modified;
            this._databaseContext.SaveChanges();

            this._scope.Dispose();
        }

        protected override void onError(AuctionMasterTaskException e)
        {
            base.onError(e);

            // == WRITES LOG ABOUT THE ERROR AND UPDATE TASK DATA
            this._taskLog.Status = 1;
            this._taskLog.EndTime = DateTime.Now;

            JObject message = new JObject();
            message.Add("error", e.Message);

            this._taskLog.Message = message.ToString();
            this._databaseContext.Entry(this._taskLog).State = EntityState.Modified;
            this._databaseContext.SaveChanges();

            this._scope.Dispose();
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
