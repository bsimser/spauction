using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Microsoft.SharePoint;
using SharePointAuction.Core;

namespace SharePointAuction.ControlTemplates.SharePointAuction
{
    public partial class ItemGrid : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateAddNewItemLinks();

            var list = SPContext.Current.Web.Lists[Constants.ItemsListName];
            var catfilter = Page.Request.QueryString["catid"];
            List<AuctionItem> items;

            if (catfilter != null)
            {
                var catid = Convert.ToInt32(catfilter);
                var cat = string.Format("{0};#", catid);

                items = (from SPListItem listItem in list.Items
                         orderby listItem.Title
                         where listItem["Categories"].ToString().Contains(cat)
                         select new AuctionItem
                         {
                             Image = GetThumbnailImage((SPFieldLookupValueCollection)listItem["Pictures"], listItem.ID),
                             Id = listItem.ID,
                             Page = Page.Request.Path,
                             Title = FormatTitle(listItem),
                             Bid = FormatBid(listItem),
                         }).ToList();
            }
            else
            {
                items = (from SPListItem listItem in list.Items
                         orderby listItem.Title
                         select new AuctionItem
                         {
                             Image = GetThumbnailImage((SPFieldLookupValueCollection)listItem["Pictures"], listItem.ID),
                             Id = listItem.ID,
                             Page = Page.Request.Path,
                             Title = FormatTitle(listItem),
                             Bid = FormatBid(listItem),
                         }).ToList();
            }

            ItemsGrid.DataSource = items;
            ItemsGrid.DataBind();

            UpdateAuctionEndsLabel();
            UpdateSearchCriteriaLabel(items.Count);
        }
        private static string FormatBid(SPListItem listItem)
        {
            var bidAmount = "";

            if (listItem["NumberOfBids"] != null)
            {
                var items = Convert.ToInt32(listItem["NumberOfBids"]);
                bidAmount = string.Format("{0} Bid{1}", items, items > 1 ? "s" : "");
            }
            else
            {
                bidAmount = "No Bids";
            }

            return string.Format("<div class=\"auction-item-grid-bid\">{0:c}<br/>{1}</div>", listItem["Bid"] ?? listItem["StartingBid"], bidAmount);
        }

        private void UpdateAddNewItemLinks()
        {
            var list = SPContext.Current.Web.Lists[Constants.ConfigListName].Items[0];
            var endDate = DateTime.Parse(list["EndDate"].ToString());

            if (TimeHelper.AuctionHasEnded(endDate))
            {
                AddNewItemPanelTop.Visible = false;
                AddNewItemPanelBottom.Visible = false;
            }
            else
            {
                AddNewItemPanelTop.Visible = true;
                AddNewItemPanelBottom.Visible = true;
            }
        }

        private void UpdateSearchCriteriaLabel(int count)
        {
            var text = "All Items";
            var countText = "";

            if (count > 0)
            {
                countText = string.Format(count > 1 ? "Displaying {0} items" : "{0} item", count);
            }

            if (Page.Request.QueryString["catid"] == null)
            {
                text = string.Format("All Items: {0}", countText);
            }
            else
            {
                var catid = Convert.ToInt32(Page.Request.QueryString["catid"]);
                var categoryList = SPContext.Current.Web.Lists[Constants.CategoryListName];
                var item = categoryList.GetItemById(catid);
                text = string.Format("{0}: {1}", item["Title"], countText);
            }

            SearchDisplay.Text = text;
        }

        private void UpdateAuctionEndsLabel()
        {
            var list = SPContext.Current.Web.Lists[Constants.ConfigListName].Items[0];
            var endDate = DateTime.Parse(list["EndDate"].ToString());
            var startDate = DateTime.Parse(list["StartDate"].ToString());
            var timeLeft = TimeHelper.GetTimeLeft(endDate);

            AuctionEnds.Text = "<div class=\"auction-time\">";

            if (!TimeHelper.AuctionHasStarted(startDate) && !TimeHelper.AuctionHasEnded(endDate))
            {
                AuctionEnds.Text += string.Format("Auction Starts: <strong>{0}</strong>", startDate);
            }
            else if (TimeHelper.AuctionHasEnded(endDate))
            {
                AuctionEnds.Text += string.Format("Auction Ended: <strong>{0}</strong>", endDate);
            }
            else
            {
                AuctionEnds.Text += string.Format("Auction Ends: <strong>{0}</strong>", endDate);
            }

            AuctionEnds.Text += "</div>";
        }

        private string GetThumbnailImage(IList<SPFieldLookupValue> pictures, int id)
        {
            if (pictures == null || pictures.Count == 0)
                return string.Format("<a href=\"{0}?uicontrol=ItemDetails&ItemId={1}\">{2}</a>",
                    Page.Request.Path, id, Constants.NoImageGridSrc);
            var pictureId = pictures[0].LookupId;
            var gallery = SPContext.Current.Web.Lists[Constants.PicturesListName];
            var item = gallery.Items.GetItemById(pictureId);
            return string.Format("<a href=\"{0}?uicontrol=ItemDetails&ItemId={1}\"><img class=\"auction-item-grid-picture\" src=\"{2}/{3}/_t/{4}.jpg\" /></a>", 
                Page.Request.Path, id,
                item.Web.Url, gallery.Title, item.Name.Replace(".", "_"));
        }

        private static string FormatTitle(SPItem listItem)
        {
            return listItem["Subtitle"] != null ? string.Format("<div class=\"auction-item-grid-title\">{0}</div><div class=\"auction-item-grid-subtitle\">{1}</div>", listItem["Title"], listItem["Subtitle"]) : string.Format("<div class=\"auction-item-grid-title\">{0}</div>", listItem["Title"]);
        }
    }
}
