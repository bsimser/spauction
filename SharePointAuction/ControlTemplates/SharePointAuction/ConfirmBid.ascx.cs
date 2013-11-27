using System;
using System.Globalization;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SharePointAuction.Core;

namespace SharePointAuction.ControlTemplates.SharePointAuction
{
    public partial class ConfirmBid : UserControl
    {
        private double _bidAmount;
        private int _itemId;

        protected void Page_Load(object sender, EventArgs e)
        {
            var config = SPContext.Current.Web.Lists[Constants.ConfigListName].Items[0];
            AuctionTitle.Text = config["Title"].ToString();

            var endDate = DateTime.Parse(config["EndDate"].ToString());
            if (TimeHelper.AuctionHasEnded(endDate))
            {
                AuctionEndedPanel.Visible = true;
                ConfirmBidPanel.Visible = false;
                HomeLink.NavigateUrl = Page.Request.Path;
            }
            else
            {
                AuctionEndedPanel.Visible = false;
                ConfirmBidPanel.Visible = true;

                if (Page.Request.QueryString["ItemId"] == null) return;
                if (Page.Request.QueryString["bidAmount"] == null) return;

                _itemId = Convert.ToInt32(Page.Request.QueryString["ItemId"]);
                _bidAmount = Convert.ToDouble(Page.Request.QueryString["BidAmount"]);

                var web = SPContext.Current.Web;
                var list = web.Lists[Constants.ItemsListName];
                var item = list.GetItemById(_itemId);

                if (item["Bid"] == null)
                {
                    var bid = Convert.ToDouble(item["StartingBid"].ToString());
                    CurrentBid.Text = bid.ToString("C");
                }
                else
                {
                    var bid = Convert.ToDouble(item["Bid"].ToString());
                    CurrentBid.Text = bid.ToString("C");
                }

                Title.Text = item.Title;
                YourBid.Text = _bidAmount.ToString("C");
                CancelLink.NavigateUrl = string.Format("{0}?uicontrol=ItemDetails&ItemId={1}", Page.Request.Path, _itemId);
            }
        }

        protected void OnBidClick(object sender, EventArgs e)
        {
            try
            {
                var config = SPContext.Current.Web.Lists[Constants.ConfigListName].Items[0];
                var endDate = DateTime.Parse(config["EndDate"].ToString());
                if (TimeHelper.AuctionHasEnded(endDate))
                {
                    AuctionEndedPanel.Visible = true;
                    HomeLink.NavigateUrl = Page.Request.Path;
                }
                else
                {
                    AuctionEndedPanel.Visible = false;

                    var bidder = SPContext.Current.Web.CurrentUser;

                    RunAsAdmin.Run((site, web) =>
                    {
                        var itemList = web.Lists[Constants.ItemsListName];
                        var item = itemList.GetItemById(_itemId);
                        NotifyOldBidder(web, item);
                        UpdateCurrentBidPrice(item);
                        IncrementBidCount(item);
                        AddBidder(web, item, bidder);
                        NotifyBidder(web, item, bidder);
                        item.Update();
                    });

                    Page.Response.Redirect(string.Format("{0}?uicontrol=ItemDetails&ItemId={1}", Page.Request.Path, _itemId));
                }
            }
            catch
            {
                throw;
            }
        }

        private void NotifyOldBidder(SPWeb web, SPItem item)
        {
            if (item["Bidder"] == null) return;
            var bidderColumn = (SPFieldUser)item.Fields.GetField("Bidder");
            var bidder = (SPFieldUserValue)bidderColumn.GetFieldValue(item["Bidder"].ToString());
            var mailFormatMessage = web.Lists[Constants.ConfigListName].Items[0]["AuctionOutbidItem"].ToString();
            var itemUrl = string.Format("{0}?uicontrol=ItemDetails&ItemId={1}", UrlHelper.GetUrl(Page.Request.Url),
                                        item.ID);
            SPUtility.SendEmail(web, true, false,
                                bidder.User.Email, "You have been outbid!",
                                string.Format(mailFormatMessage, item["Title"], item["Bid"], itemUrl, web.Url));
        }

        private void NotifyBidder(SPWeb web, SPItem item, SPUser bidder)
        {
            var mailFormatMessage = web.Lists[Constants.ConfigListName].Items[0]["AuctionItemLeadingBidder"].ToString();
            SPUtility.SendEmail(web, true, false,
                                bidder.Email, "You are the leading bidder",
                                string.Format(mailFormatMessage, _bidAmount, item["Title"], string.Format("{0}?uicontrol=ItemDetails&ItemId={1}", UrlHelper.GetUrl(Page.Request.Url), item.ID)));
        }

        private void AddBidder(SPWeb web, SPItem item, SPUser name)
        {
            var bidder = web.Lists[Constants.BidderListName].Items.Add();
            bidder["Title"] = string.Format("{0} bid by {1} at {2}", item["Title"], name, DateTime.Now);
            bidder["Bidder"] = name;
            bidder["Item"] = new SPFieldLookupValue(item.ID, item["Title"].ToString());
            bidder["Amount"] = _bidAmount;
            bidder.Update();
            item["Bidder"] = name;
            item.Update();
        }

        private static void IncrementBidCount(SPItem item)
        {
            var bids = 1;
            if (item["NumberOfBids"] != null)
            {
                bids = Convert.ToInt32(item["NumberOfBids"].ToString());
                bids++;
            }
            item["NumberOfBids"] = bids.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateCurrentBidPrice(SPItem item)
        {
            item["Bid"] = _bidAmount;
        }
    }
}
