using System;
using System.Linq;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SharePointAuction.Core;

namespace SharePointAuction.ControlTemplates.SharePointAuction
{
    public partial class AuctionInformation : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var configList = SPContext.Current.Web.Lists[Constants.ConfigListName];
            if(configList == null) return;
            if (configList.ItemCount == 0) return;
            var config = configList.Items[0];

            AuctionTitle.Text = config["Title"].ToString();
            StartDateTime.Text = config["StartDate"].ToString();
            EndDateTime.Text = config["EndDate"].ToString();
            Description.Text = config["Description"].ToString();
            PickupInstructions.Text = config["PickupInstructions"] == null ? "None specified" : config["PickupInstructions"].ToString();
            HomeLink.NavigateUrl = Page.Request.Path;
            SalesSummaryLink.NavigateUrl = string.Format("{0}?uicontrol=SalesSummary", Page.Request.Path);
            AmountRaised.Text = GetAmountRaised();

            var endDate = DateTime.Parse(config["EndDate"].ToString());
            if (TimeHelper.AuctionHasEnded(endDate))
            {
                CloseLink.Visible = true;
                CloseLink.Click += CloseLink_Click;
            }
            else
            {
                CloseLink.Visible = false;
            }

            ResetItems.Click += ResetItems_Click;
            ResetAuction.Click += ResetAuction_Click;
        }

        private static string GetAmountRaised()
        {
            var amountRaised = 0.0;

            var list = SPContext.Current.Web.Lists.TryGetList(Constants.ItemsListName);
            if (list != null)
            {
                var items = list.Items;
                amountRaised += (from SPItem item in items where item["Bid"] != null select Convert.ToDouble(item["Bid"].ToString())).Sum();
            }

            return amountRaised.ToString("C");
        }

        static void ResetAuction_Click(object sender, EventArgs e)
        {
            DeleteItems();
        }

        static void ResetItems_Click(object sender, EventArgs e)
        {
            DeleteItems();
        }

        private static void DeleteItems()
        {
            DeleteListItems(Constants.ItemsListName);
            DeleteListItems(Constants.BidderListName);
        }

        private static void DeleteListItems(string listTitle)
        {
            var list = SPContext.Current.Web.Lists[listTitle];
            var items = list.Items;
            var itemIds = (from SPListItem item in items select item.ID).ToList();

            foreach (var itemId in itemIds)
            {
                items.DeleteItemById(itemId);
            }
        }

        void CloseLink_Click(object sender, EventArgs e)
        {
            var configList = SPContext.Current.Web.Lists[Constants.ConfigListName];
            if (configList == null) return;
            if (configList.ItemCount == 0) return;
            var config = configList.Items[0];
            var endDate = DateTime.Parse(config["EndDate"].ToString());
            if (!TimeHelper.AuctionHasEnded(endDate)) return;

            var auctionEndedTopBidderMail = config["AuctionEndedTopBidder"].ToString();
            var itemList = SPContext.Current.Web.Lists[Constants.ItemsListName];
            var items = itemList.Items;

            foreach (var item in items.Cast<SPListItem>().Where(item => item["Bid"] != null))
            {
                item["IsSold"] = true;
                item.Update();

                var bidderColumn = (SPFieldUser)item.Fields.GetField("Bidder");
                var bidder = (SPFieldUserValue)bidderColumn.GetFieldValue(item["Bidder"].ToString());
                var htmlBody = string.Format(auctionEndedTopBidderMail,
                                             bidder.User.Name,
                                             item["Title"],
                                             config["Title"],
                                             string.Format("{0}?uicontrol=ItemDetails&ItemId={1}", UrlHelper.GetUrl(Page.Request.Url), item.ID),
                                             SPContext.Current.Web.Url,
                                             config["PickupInstructions"]);
                SPUtility.SendEmail(SPContext.Current.Web, true, false, bidder.User.Email, "Auction has ended and you're the top bidder", htmlBody);
            }

            var genericUserAuctionEndedEmail = config["GenericUserAuctionEndedEmail"].ToString();
            var adminBody = string.Format(genericUserAuctionEndedEmail,
                                          config["Title"],
                                          string.Format("{0}?uicontrol=SalesSummary", UrlHelper.GetUrl(Page.Request.Url)),
                                          SPContext.Current.Web.Url);
            SPUtility.SendEmail(SPContext.Current.Web, true, false, SPContext.Current.Web.CurrentUser.Email, "Your auction has ended", adminBody);
        }
    }
}
