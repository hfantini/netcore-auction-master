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
using AuctionMaster.App.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;

// == NAMESPACE
// ==============================================================================
namespace AuctionMaster.App.Service.Task
{
    /// <summary>
    /// 
    /// Defines an scheduled task process unit. Through this class you can create any type of 
    /// asynchronous process in accord of the standards of this application.
    /// 
    /// </summary>
    public abstract class GenericScheduledTask
    {
        // == CLASS
        // ==========================================================================

        // == DECLARATIONS
        // ======================================================================

        // == CONST

        // == VAR

        protected IServiceScopeFactory _scopeFactory;
        protected IServiceScope _scope;
        protected DatabaseContext _databaseContext;
        protected ScheduledTask _scheduledTask;
        protected IDbContextTransaction _databaseTransaction;
        protected ScheduledTaskLog _taskLog;
        protected JObject _message;
        private ScheduledTaskState _state;
        private Task<Object> _task = null;
        private CancellationTokenSource _cancellationToken;
        protected bool _newTentative = false;
        protected ScheduledTaskLogService _logService;

        // == CONSTRUCTOR(S)
        // ======================================================================

        public GenericScheduledTask(IServiceScopeFactory scopeFactory, ScheduledTask scheduledTask)
        {
            this._scopeFactory = scopeFactory;
            this._scheduledTask = scheduledTask;
            this._state = ScheduledTaskState.IDLE;
        }

        // == METHOD(S)
        // ======================================================================

        /// <summary>
        /// Method invoked when task starts the work.
        /// </summary>
        /// <returns>Object : Usually the return is NULL.</returns>
        private Object taskExecution()
        {
            Object retValue = null;

            try
            {
                if(this._state == ScheduledTaskState.ERROR)
                {
                    this._newTentative = true;
                }
                else
                {
                    this._newTentative = false;
                }

                this._state = ScheduledTaskState.RUNNING;

                // == TASK LIFE-CYCLE EXECUTION

                this._cancellationToken = new CancellationTokenSource();
                onStart();

                // PARAMETER TREATMENT

                JObject param = null;

                if (this.task.Param != null)
                {
                    try
                    {
                        param = JObject.Parse(this.task.Param);
                    }
                    catch (System.Exception e)
                    {
                        throw new AuctionMasterTaskException(ExceptionType.FATAL, "Invalid param formatting - Expected: Valid JSON.");
                    }
                }

                onExecute(param, this._cancellationToken.Token);

                onFinish();
                this._state = ScheduledTaskState.IDLE;
            }
            catch( System.Exception e )
            {
                if( !(e is AuctionMasterTaskException) )
                {
                    e = new AuctionMasterTaskException(ExceptionType.FATAL, e.Message, e);
                }

                this._state = ScheduledTaskState.ERROR;

                this.onError( (AuctionMasterTaskException) e );
                retValue = (AuctionMasterTaskException) e;
            }

            return retValue;
        }

        /// <summary>
        /// Checks if the the task has been cancelled and finishes the process.
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        protected void checkCanceledTask(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                this._state = ScheduledTaskState.CANCELLED;
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Starts the task.
        /// </summary>
        /// <exception cref="AuctionMasterTaskException">If the task is already running.</exception>
        public async void start()
        {
            if (this._state != ScheduledTaskState.RUNNING)
            {
                this._task = new Task<Object>(taskExecution);
                this._task.Start();
            }
            else
            {
                throw new AuctionMasterTaskException(ExceptionType.ERROR, "This task is already running.");
            }
        }

        /// <summary>
        /// Stops the task.
        /// </summary>
        /// <exception cref="AuctionMasterTaskException">If the task is not running.</exception>
        public async void stop()
        {
            if (this._state == ScheduledTaskState.RUNNING)
            {
                this._cancellationToken.Cancel();
            }
            else
            {
                throw new AuctionMasterTaskException(ExceptionType.ERROR, "This task is not running.");
            }
        }

        // == EVENT(S)
        // ======================================================================

        /// <summary>
        /// Event called before the start of the task execution.
        /// </summary>
        protected virtual void onStart()
        {
            this._message = new JObject();

            // == CREATES THE SCOPE & DATABASE CONTEXT

            this._scope = this._scopeFactory.CreateScope();
            this._databaseContext = this._scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            // == CREATES THE ENTRY OF THIS SCAN ON DATABASE

            var currentTime = DateTime.Now;

            this._databaseTransaction = this._databaseContext.Database.BeginTransaction();

            // SCHEDULED_TASK_LOG

            if (!this._newTentative)
            {
                this._taskLog = new ScheduledTaskLog();
                this._taskLog.StartTime = currentTime;
                this._taskLog.ScheduledTaskNavigation = this._scheduledTask;
                this._databaseContext.Entry(this._taskLog.ScheduledTaskNavigation).State = EntityState.Unchanged;
                this._taskLog.ScheduledTask = this._scheduledTask.Id;
                this._databaseContext.ScheduledTaskLog.Add(this._taskLog);
            }
            else
            {
                this._taskLog = this._databaseContext.ScheduledTaskLog.Where(log => log.ScheduledTask == this._scheduledTask.Id).OrderByDescending(log => log.StartTime).FirstOrDefault();

                if(this._taskLog == null)
                {
                    throw new AuctionMasterTaskException(ExceptionType.WARNING, "Last log not found");
                }
            }

            this._databaseContext.SaveChanges();
            this._databaseTransaction.Commit();

            this._logService = new ScheduledTaskLogService(this.GetType(), this._taskLog);

            this._logService.start();
            this._logService.writeLine(LogType.INFO, $"Scheduled task [{this._scheduledTask.Name}] has started.", true);
        }

        /// <summary>
        /// The task execution method.
        /// </summary>
        protected virtual void onExecute(JObject param, CancellationToken cancellationToken)
        {

        }

        /// <summary>
        /// Event called after an successful execution of task.
        /// </summary>
        protected virtual void onFinish()
        {
            // == WRITES LOG ABOUT THE EXECUTION AND UPDATE TASK DATA

            var taskLog = this._databaseContext.ScheduledTaskLog.Attach(this._taskLog);
            this._taskLog.Status = 1;
            this._taskLog.EndTime = DateTime.Now;

            this._taskLog.Message = this._message.ToString();
            this._databaseContext.Entry(this._taskLog).State = EntityState.Modified;
            this._databaseContext.SaveChanges();

            this._scope.Dispose();

            this._logService.writeLine(LogType.SUCCESS, $"Scheduled task [{this._scheduledTask.Name}] has finished without errors. Details: {this._message.ToString()}", true);
            this._logService.finish();
        }

        /// <summary>
        /// Event called when an exception is thrown inside the execution.
        /// </summary>
        /// <param name="e">Exception structure</param>
        protected virtual void onError(AuctionMasterTaskException e)
        {
            // == WRITES LOG ABOUT THE ERROR AND UPDATE TASK DATA

            this._taskLog.Status = 0;
            this._taskLog.EndTime = DateTime.Now;
            this._taskLog.Tentatives++;

            this._taskLog.Message = this._message.ToString();
            this._databaseContext.Entry(this._taskLog).State = EntityState.Modified;
            this._databaseContext.SaveChanges();

            this._scope.Dispose();

            this._logService.writeLine(LogType.ERROR, $"Scheduled task [{this._scheduledTask.Name}] has finished with errors (Tentative {this._taskLog.Tentatives}). Details: {this._message.ToString()}", true);
            this._logService.finish();
        }

        // == GETTER(S) AND SETTER(S)
        // ======================================================================

        /// <summary>
        /// Obtains the current state of this task.
        /// </summary>
        public ScheduledTaskState state
        {
            get { return this._state; }
            set { this._state = value; }
        }

        /// <summary>
        /// Returns scheduled task information.
        /// </summary>
        public ScheduledTask task
        {
            get { return this._scheduledTask; }
        }
    }
}
