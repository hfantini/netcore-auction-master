
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

    public class RealmScanTask : GenericScheduledTask
    {
        // == DECLARATIONS
        // ======================================================================

        // == CONST
        private readonly IBlizzardRealmService _blizzardRealmService;

        // == VAR
        private IServiceScope _scope;
        private DatabaseContext _databaseContext;
        private IDbContextTransaction _dbTransaction;
        private ScheduledTaskLog _taskLog;
        private int _connectedRealmCount;
        private int _RealmCount;

        // == CONSTRUCTOR(S)
        // ======================================================================

        public RealmScanTask(IServiceScopeFactory scopeFactory, ScheduledTask scheduledTask, IBlizzardRealmService blizzardRealmService) : base(scopeFactory, scheduledTask)
        {
            this._blizzardRealmService = blizzardRealmService;
        }

        // == METHOD(S)
        // ======================================================================

        protected override void onStart()
        {
            base.onStart();

            this._connectedRealmCount = 0;
            this._RealmCount = 0;

            // == CREATES THE SCOPE & DATABASE CONTEXT

            this._scope = this._scopeFactory.CreateScope();
            this._databaseContext = this._scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            // == CREATES THE ENTRY OF THIS SCAN ON DATABASE

            var currentTime = DateTime.Now;

            this._dbTransaction = this._databaseContext.Database.BeginTransaction();

            // SCHEDULED_TASK LAST EXECUTION

            //this._scheduledTask.LastExecution = currentTime;
            //this._databaseContext.ScheduledTask.Update(this._scheduledTask);

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

        protected override void onExecute(JObject param, CancellationToken cancellationToken)
        {
            base.onExecute(param, cancellationToken);

            // OBTAIN A LIST OF CONNECTED REALM REGION

            List<ConnectedRealmRegion> realmRegions = this._databaseContext.ConnectedRealmRegion.OrderBy(crr => crr.Id).ToList<ConnectedRealmRegion>();

            this._dbTransaction = this._databaseContext.Database.BeginTransaction();

            // LOOP OVER THE REGIONS

            foreach ( ConnectedRealmRegion connectedRealmRegion in realmRegions )
            {
                // OBTAINS THE LIST OF CONNECTED REALMS

                Task<JObject> taskConnectedRealmList = this._blizzardRealmService.getConnectedRealmList(connectedRealmRegion.Code);
                taskConnectedRealmList.Wait();

                JObject cRealmList = taskConnectedRealmList.Result;

                // LOOP OVER THE CONNECTED REALM LIST

                foreach( JObject cRealmListHref in cRealmList.Value<JArray>("connected_realms").Children() )
                {
                    this._connectedRealmCount++;

                    String href = cRealmListHref.Value<String>("href");
                    int id = Convert.ToInt32( Regex.Match(href, @"\d+").Value );

                    // GET CONNECTED REALM INFORMATION

                    Task<JObject> taskConnectedRealmInfo = this._blizzardRealmService.getConnectedRealmInformation(id, connectedRealmRegion.Code);
                    taskConnectedRealmInfo.Wait();

                    JObject connectedRealmInfo = taskConnectedRealmInfo.Result;

                    // WRITES THE DATA IN THE DATABASE

                    bool update = true;
                    ConnectedRealm cRealm = this._databaseContext.ConnectedRealm.Find(connectedRealmInfo.Value<int>("id"));

                    if(cRealm == null)
                    {
                        update = false;
                        cRealm = new ConnectedRealm();
                        cRealm.Id = connectedRealmInfo.Value<int>("id");
                        cRealm.RealmRegion = connectedRealmRegion.Id;
                        this._databaseContext.Entry(cRealm).State = EntityState.Added;
                    }
                    else
                    {
                        this._databaseContext.Entry(cRealm).State = EntityState.Modified;
                    }

                    foreach (JObject cRealmChildren in connectedRealmInfo.Value<JArray>("realms").Children())
                    {
                        Realm realm = this._databaseContext.Realm.Find(cRealmChildren.Value<int>("id") );

                        if (realm == null)
                        {
                            realm = new Realm();
                            realm.Id = cRealmChildren.Value<int>("id");
                            realm.Name = cRealmChildren.Value<String>("name");
                            realm.Locale = cRealmChildren.Value<String>("locale");
                            realm.Timezone = cRealmChildren.Value<String>("timezone");
                            realm.Category = cRealmChildren.Value<String>("category");
                            this._databaseContext.Entry(realm).State = EntityState.Added;
                        }
                        else
                        {
                            realm.Name = cRealmChildren.Value<String>("name");
                            realm.Locale = cRealmChildren.Value<String>("locale");
                            realm.Timezone = cRealmChildren.Value<String>("timezone");
                            realm.Category = cRealmChildren.Value<String>("category");
                            this._databaseContext.Entry(realm).State = EntityState.Modified;
                        }

                        cRealm.Realm.Add(realm);
                        this._RealmCount++;
                    }

                    if(!update)
                    {
                        this._databaseContext.ConnectedRealm.Add(cRealm);
                    }
                    else
                    {
                        this._databaseContext.ConnectedRealm.Update(cRealm);
                    }

                    this._databaseContext.SaveChanges();
                }
            }

            this._dbTransaction.Commit();
        }

        protected override void onFinish()
        {
            base.onFinish();

            // == WRITES LOG ABOUT THE EXECUTION AND UPDATE TASK DATA

            var taskLog = this._databaseContext.ScheduledTaskLog.Attach(this._taskLog);
            taskLog.Entity.Status = 1;
            taskLog.Entity.EndTime = DateTime.Now;

            JObject message = new JObject();
            message.Add("connectedRealmCount", this._connectedRealmCount);
            message.Add("realmCount", this._RealmCount);

            taskLog.Entity.Message = message.ToString();
            taskLog.State = EntityState.Modified;
            this._databaseContext.SaveChanges();

            this._scope.Dispose();
        }

        protected override void onError(AuctionMasterTaskException e)
        {
            base.onError(e);

            // == WRITES LOG ABOUT THE ERROR AND UPDATE TASK DATA
            var taskLog = this._databaseContext.ScheduledTaskLog.Attach(this._taskLog);
            taskLog.Entity.Status = 1;
            taskLog.Entity.EndTime = DateTime.Now;

            JObject message = new JObject();
            message.Add("error", e.Message);

            taskLog.Entity.Message = message.ToString();
            taskLog.State = EntityState.Modified;
            this._databaseContext.SaveChanges();

            this._scope.Dispose();
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
