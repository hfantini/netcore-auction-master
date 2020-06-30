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
        protected ScheduledTask _scheduledTask;
        private ScheduledTaskState _state;
        private Task<Object> _task = null;
        private CancellationTokenSource cancellationToken;

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
                this._state = ScheduledTaskState.RUNNING;

                // == TASK LIFE-CYCLE EXECUTION

                this.cancellationToken = new CancellationTokenSource();
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

                onExecute(param, this.cancellationToken.Token);

                onFinish();
                this._state = ScheduledTaskState.IDLE;
            }
            catch( System.Exception e )
            {
                if( !(e is AuctionMasterTaskException) )
                {
                    e = new AuctionMasterTaskException(ExceptionType.FATAL, "An error ocurred during the task execution. Original Message:" + e.Message, e);
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
                this.cancellationToken.Cancel();
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

        }

        /// <summary>
        /// Event called when an exception is thrown inside the execution.
        /// </summary>
        /// <param name="e">Exception structure</param>
        protected virtual void onError(AuctionMasterTaskException e)
        {
            Console.WriteLine("[ERROR]: " + e.Message);
        }

        // == GETTER(S) AND SETTER(S)
        // ======================================================================

        /// <summary>
        /// Obtains the current state of this task.
        /// </summary>
        public ScheduledTaskState state
        {
            get { return this._state; }
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
