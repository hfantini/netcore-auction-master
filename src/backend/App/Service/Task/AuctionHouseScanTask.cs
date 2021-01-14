
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
        private readonly IBlizzardItemService _blizzardItemService;

        // == VAR
        private int _auctionScanCount;

        // == CONSTRUCTOR(S)
        // ======================================================================

        public AuctionHouseScanTask(IServiceScopeFactory scopeFactory, ScheduledTask scheduledTask, IBlizzardAuctionHouseService blizzardAuctionHouseService, IBlizzardItemService blizzardItemService) : base(scopeFactory, scheduledTask)
        {
            this._blizzardAuctionHouseService = blizzardAuctionHouseService;
            this._blizzardItemService = blizzardItemService;
        }

        // == METHOD(S)
        // ======================================================================

        protected override void onStart()
        {
            base.onStart();

            this._auctionScanCount = 0;
        }

        private void fullAuctionHouseScan(int connectedRealmID)
        {
            ConnectedRealm connectedRealm = this._databaseContext.ConnectedRealm.Find(connectedRealmID);

            if(connectedRealm != null)
            {
                Task<JObject> auctionTask = this._blizzardAuctionHouseService.getAuctionList(connectedRealm);
                auctionTask.Wait();

                JObject auctionList = auctionTask.Result;

                // == PROCESSING AUCTION HOUSE INFORMATION

                foreach( JObject requestAuction in auctionList.Value<JArray>("auctions") )
                {
                    // == ITEM TREATMENT

                    JObject auctionItem = requestAuction.Value<JObject>("item");
                    Item item = this._databaseContext.Item.Find(auctionItem.Value<int>("id"));

                    if(item == null)
                    {
                        this._logService.writeLine(LogType.INFO, $"Requesting information about item {requestAuction.Value<JObject>("item") }");

                        // GET ITEM INFORMATION FROM BLIZZARD

                        Task<JObject> itemTask = this._blizzardItemService.getItem( auctionItem.Value<int>("id") );
                        itemTask.Wait();

                        auctionItem = itemTask.Result;

                        // CREATES THE ITEM IN THE DATABASE

                        item = new Item();
                        item.Id = auctionItem.Value<int>("id");
                        item.Name = auctionItem.Value<string>("name");
                        item.Quality = 1;
                        item.Stackable = ( auctionItem.Value<Boolean>("is_stackable") == true ? Convert.ToSByte(1) : Convert.ToSByte(0) );
                        //item.Levelreq;
                        //item.PurchasePrice;
                        //item.SellPrice;

                        this._logService.writeLine(LogType.INFO, $"Item ( {item.Id} ) information obtained: {item.Name}");
                    }
                    else
                    {
                        this._databaseContext.Entry(item).State = EntityState.Modified;
                    }

                    // == CREATE AUCTION ENTRY

                    Auction auction = new Auction();
                    auction.ScheduledTaskLog = this._taskLog.Id;
                    auction.ItemNavigation = item;
                    auction.ConnectedRealm = connectedRealm.Id;

                    if (requestAuction.ContainsKey("buyout"))
                    {
                        auction.Buyout = requestAuction.Value<long>("buyout");
                    }

                    if (requestAuction.ContainsKey("unit_price"))
                    {
                        auction.UnitPrice = requestAuction.Value<long>("unit_price");
                    }

                    if(requestAuction.ContainsKey("bid"))
                    {
                        auction.Bid = requestAuction.Value<long>("bid");
                    }
                    
                    auction.Quantity = requestAuction.Value<int>("quantity");

                    this._databaseContext.Auction.Add(auction);
                    this._databaseContext.SaveChanges();

                    this._logService.writeLine(LogType.INFO, $"Auction ( {auction.Id} - {auction.ItemNavigation.Name} ) has been inserted.");
                }
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
            this._message.Add("auctionCount", this._auctionScanCount);

            base.onFinish();
        }

        protected override void onError(AuctionMasterTaskException e)
        {
            this._message.Add("error", e.Message);

            base.onError(e);
        }

        // == EVENT(S)
        // ======================================================================

        // == GETTER(S) AND SETTER(S)
        // ======================================================================
    }
}
