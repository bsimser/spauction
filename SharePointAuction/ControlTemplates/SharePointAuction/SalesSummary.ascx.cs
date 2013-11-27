using System;
using System.Linq;
using System.Web.UI;
using Microsoft.SharePoint;
using SharePointAuction.Core;

namespace SharePointAuction.ControlTemplates.SharePointAuction
{
    public partial class SalesSummary : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPListItem config = SPContext.Current.Web.Lists[Constants.ConfigListName].Items[0];
            AuctionTitle.Text = config["Title"].ToString();

            var list = SPContext.Current.Web.Lists[Constants.ItemsListName];

            var jscriptFormatString = "javascript:auctionOpenModalURL(L_Menu_BaseUrl + '/Lists/" + Constants.ItemsListName + "/EditForm.aspx?ID={0}', 'Update Item', 645, 800, 'refreshCallback', false)";

            var items = (from SPListItem listItem in list.Items
                         orderby listItem.Title
                         select new AuctionItem
                         {
                             Id = listItem.ID,
                             Page = Page.Request.Path,
                             Title = FormatTitle(listItem),
                             Bids = FormatNumberOfBids(listItem),
                             Bid = FormatBid(listItem),
                             Value = listItem["Value"] == null ? "" : string.Format("<div class=\"auction-item-grid-value\">{0:c}</div>", listItem["Value"]),
                             Bidder = listItem["Bidder"] == null ? "" : FormatBidderName(listItem),
                             IsPaid = listItem["IsPaid"] == null ? "" : listItem["IsPaid"].ToString(),
                             IsSold = listItem["IsSold"] == null ? "" : listItem["IsSold"].ToString(),
                             Action = "<a href=\"#\" onclick=\"" + string.Format(jscriptFormatString, listItem.ID) + "\">Edit</a>",
                         }).ToList();

            ItemsGrid.DataSource = items;
            ItemsGrid.DataBind();

            HomeLink.NavigateUrl = Page.Request.Path;
        }

        private static string FormatBidderName(SPItem item)
        {
            var bidderColumn = (SPFieldUser)item.Fields.GetField("Bidder");
            var bidder = (SPFieldUserValue)bidderColumn.GetFieldValue(item["Bidder"].ToString());
            return bidder.User.Name;
        }

        private static string FormatBid(SPItem listItem)
        {
            return string.Format("<div class=\"auction-item-grid-bid\">{0:c}</div>", listItem["Bid"] ?? listItem["StartingBid"]);
        }

        private static string FormatNumberOfBids(SPItem listItem)
        {
            var rc = "";

            if (listItem["NumberOfBids"] != null)
            {
                var items = Convert.ToInt32(listItem["NumberOfBids"]);
                rc = string.Format("{0} Bid{1}", items, items > 1 ? "s" : "");
            }
            else
            {
                rc = "0 Bids";
            }

            return rc;
        }

        private static string FormatTitle(SPItem listItem)
        {
            return listItem["Subtitle"] != null ?
                string.Format("<div class=\"auction-item-grid-title\">{0}</div><div class=\"auction-item-grid-subtitle\">{1}</div>", listItem["Title"], listItem["Subtitle"]) :
                string.Format("<div class=\"auction-item-grid-title\">{0}</div>", listItem["Title"]);
        }

    }
}
