using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using SharePointAuction.Core;

namespace SharePointAuction.ControlTemplates.SharePointAuction
{
    public partial class ItemDetails : UserControl
    {
        private int _itemId = -1;
        private double _currentBid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["ItemId"] == null) return;

            var web = SPContext.Current.Web;
            var config = web.Lists[Constants.ConfigListName].Items[0];
            var endDate = DateTime.Parse(config["EndDate"].ToString());
            var startDate = DateTime.Parse(config["StartDate"].ToString());

            _itemId = Convert.ToInt32(Page.Request.QueryString["ItemId"]);
            var item = SPContext.Current.Web.Lists[Constants.ItemsListName].Items.GetItemById(_itemId);

            RenderTitles(config, item);
            RenderBidValues(item, startDate, endDate);
            RenderStartPanel(startDate);
            RenderMarketValue(item);
            RenderNumberOfBids(item);
            RenderPictures(item);
            RenderDescription(item);
            RenderPickupInstructions(config);
            RenderCategoryList(item);
            RenderDonorPanel(item);
            RenderAdminArea(item);

            UpdateEditLink();

            HomeLinkTop.NavigateUrl = Page.Request.Path;
            HomeLinkBottom.NavigateUrl = Page.Request.Path;
        }

        private void RenderNumberOfBids(SPListItem item)
        {
            if (item["NumberOfBids"] != null)
            {
                var bids = item["NumberOfBids"].ToString();
                NumberOfBidsLabel.Text = string.Format("{0} - <a href=\"#\" onclick=\"javascript:$('#auction-bid-history').slideToggle()\">Bid History</a>", bids);
                // build list of bids for this item
                var query = new SPQuery
                {
                    Query =
                        "<Where><Eq><FieldRef Name=\"Item\" LookupId=\"TRUE\"/><Value Type=\"Lookup\">" + item.ID +
                        "</Value></Eq></Where><OrderBy><FieldRef Name=\"Modified\" Ascending=\"FALSE\"/></OrderBy></Query>"
                };
                var bidHistory = SPContext.Current.Web.Lists[Constants.BidderListName].GetItems(query);

                BidHistory.Text = "<table cellpadding=\"4\" cellspacing=\"4\"><tr>";
                BidHistory.Text += "<th>Bidder</th><th>Bid Amount</th><th>Bid Time</th>";
                foreach (SPListItem bidItem in bidHistory)
                {
                    var bidderColumn = (SPFieldUser)bidItem.Fields.GetField("Bidder");
                    var bidder = (SPFieldUserValue)bidderColumn.GetFieldValue(bidItem["Bidder"].ToString());
                    BidHistory.Text += string.Format("<tr><td>{0}</td><td>{1:c}</td><td>{2}</td></tr>", bidder.User.Name, bidItem["Amount"], bidItem["Modified"]);
                }
                BidHistory.Text += "</tr></table>";
            }
            else
            {
                NumberOfBidsLabel.Text = "0";
            }
        }

        private void UpdateEditLink()
        {
            EditLink.NavigateUrl = string.Format("javascript:editAuctionItem({0})", _itemId);
        }

        private void RenderAdminArea(SPListItem item)
        {
            StartingBid.Text = string.Format("{0:c}", item["StartingBid"]);
            MinimumBidIncrement.Text = string.Format("{0:c}", item["MinimumBidIncrement"]);
        }

        private void RenderDonorPanel(SPListItem item)
        {
            if (item["Donor"] != null)
            {
                var donor = new SPFieldLookupValue(item["Donor"].ToString());
                try
                {
                    var linkField = SPContext.Current.Web.Lists["Donors"].GetItemById(donor.LookupId);

                    DonorLink.Text = donor.LookupValue;

                    if (linkField["Website"] != null)
                    {
                        var link = new SPFieldUrlValue(linkField["Website"].ToString());
                        DonorLink.NavigateUrl = link.Url;
                        DonorLink.Target = "_blank";
                    }

                    DonorPanel.Visible = true;
                }
                catch (ArgumentException)
                {
                    //throw new Exception(string.Format("The requested Donor \"{0}\" was not found for the item titled \"{1}\".", donor, item.Title));
                    DonorPanel.Visible = false;
                }
            }
            else
            {
                DonorPanel.Visible = false;
            }
        }

        private void RenderCategoryList(SPListItem item)
        {
            CategoryList.Items.Clear();
            var categories = (SPFieldLookupValueCollection)item["Categories"];
            foreach (SPFieldLookupValue category in categories)
            {
                CategoryList.Items.Add(category.LookupValue);
            }
        }

        private void RenderPickupInstructions(SPListItem config)
        {
            if (config["PickupInstructions"] != null)
            {
                PickupInstructions.Text = config["PickupInstructions"].ToString();
            }
        }

        private void RenderPictures(SPListItem item)
        {
            if (item["Pictures"] != null)
            {
                PictureList.Text = "<ul id=\"myGallery\">";

                var pictures = (SPFieldLookupValueCollection)item["Pictures"];

                if (pictures.Count == 0)
                {
                    PictureList.Text += string.Format("<li>{0}</li>", Constants.NoImageSrc);
                }
                else
                {
                    foreach (SPFieldLookupValue picture in pictures)
                    {
                        int pictureId = picture.LookupId;
                        try
                        {
                            var pictureItem = SPContext.Current.Web.Lists[Constants.PicturesListName].GetItemById(pictureId);
                            var pictureUrl = string.Format(
                                "<img alt=\"{2}\" title=\"{2}\" src=\"{0}/Pictures/_w/{1}.jpg\"/>",
                                pictureItem.ParentList.ParentWebUrl, pictureItem.Name.Replace('.', '_'), picture.LookupValue);
                            PictureList.Text += string.Format("<li>{0}</li>", pictureUrl);
                        }
                        catch (ArgumentException)
                        {
                        }
                    }
                }
                PictureList.Text += "</ul>";
            }
            else
            {
                PictureList.Text = "<ul id=\"myGallery\">";
                PictureList.Text += string.Format("<li>{0}</li>", Constants.NoImageSrc);
                PictureList.Text += "</ul>";
            }
        }

        private void RenderTitles(SPListItem config, SPListItem item)
        {
            AuctionTitle.Text = config["Title"].ToString();
            ItemTitle.Text = item["Title"].ToString();
            ItemSubtitle.Text = item["Subtitle"] != null ? item["Subtitle"].ToString() : "";
        }

        private void RenderBidValues(SPListItem item, DateTime startDate, DateTime endDate)
        {
            YouWonPanel.Visible = false;
            HighestBidderPanel.Visible = false;

            if (TimeHelper.AuctionHasEnded(endDate))
            {
                StartPanel.Visible = false;
                StartingBidPanel.Visible = false;
                EndLabelText.Text = "Ended:";
                EndLabelValue.Text = endDate.ToString();
                CurrentBidPanel.Visible = false;
                PlaceBidPanel.Visible = false;
                SPSecurityTrimmedControlName.Visible = false;

                if (item["Bidder"] != null)
                {
                    var bidderColumn = (SPFieldUser)item.Fields.GetField("Bidder");
                    var bidderFieldValue = (SPFieldUserValue)bidderColumn.GetFieldValue(item["Bidder"].ToString());

                    if (bidderFieldValue.User.LoginName.Equals(SPContext.Current.Web.CurrentUser.LoginName))
                    {
                        YouWonPanel.Visible = true;
                    }

                    StartingBidPanel.Visible = true;
                    BidSellLabel.Text = "Selling Price:";
                    BidSellValue.Text = string.Format("{0:c}", item["Bid"]);
                }
            }
            else
            {
                if (TimeHelper.AuctionHasStarted(startDate))
                {
                    PlaceBidPanel.Visible = true;
                }
                else
                {
                    PlaceBidPanel.Visible = false;
                }

                StartPanel.Visible = true;

                StartingBidPanel.Visible = true;
                BidSellLabel.Text = "Starting Bid:";
                BidSellValue.Text = string.Format("{0:c}", item["StartingBid"]);

                var bidCountField = item["NumberOfBids"];
                if (bidCountField != null)
                {
                    var bids = Convert.ToInt32(bidCountField.ToString());
                    if (bids > 0)
                    {
                        StartingBidPanel.Visible = false;
                    }
                }

                EndLabelText.Text = "Ends:";
                EndLabelValue.Text = endDate.ToString();

                if (item["Bidder"] != null)
                {
                    var bidderColumn = (SPFieldUser)item.Fields.GetField("Bidder");
                    var bidderFieldValue = (SPFieldUserValue)bidderColumn.GetFieldValue(item["Bidder"].ToString());

                    // Is the current user the current bidder?
                    if (bidderFieldValue.User.LoginName.Equals(SPContext.Current.Web.CurrentUser.LoginName))
                    {
                        HighestBidderPanel.Visible = true;
                        PlaceBidPanel.Visible = false;
                    }
                    else
                    {
                        HighestBidderPanel.Visible = false;
                        PlaceBidPanel.Visible = true;
                    }
                }

                if (item["Bid"] != null)
                {
                    _currentBid = Convert.ToDouble(item["Bid"].ToString());
                    var bidIncrement = Convert.ToDouble(item["MinimumBidIncrement"].ToString());
                    MinimumBidLabel.Text = string.Format("(Enter {0:c} or More to Bid)", _currentBid + bidIncrement);
                    CurrentBidPanel.Visible = true;
                    CurrentBidValue.Text = string.Format("{0:c}", _currentBid);
                }
                else
                {
                    CurrentBidPanel.Visible = false;
                    var minBid = Convert.ToDouble(item["StartingBid"].ToString());
                    var bidIncrement = Convert.ToDouble(item["MinimumBidIncrement"].ToString());
                    MinimumBidLabel.Text = string.Format("(Enter {0:c} or More to Bid)", minBid + bidIncrement);
                }
            }
        }

        private void RenderMarketValue(SPListItem item)
        {
            if (item["Value"] != null)
            {
                MarketValuePanel.Visible = true;
                MarketValue.Text = string.Format("{0:c}", Convert.ToDouble(item["Value"]));
            }
            else
            {
                MarketValuePanel.Visible = false;
            }
        }

        private void RenderStartPanel(DateTime startDate)
        {
            if (TimeHelper.AuctionHasStarted(startDate))
            {
                StartPanel.Visible = false;
            }
            else
            {
                StartPanel.Visible = true;
                StartLabelValue.Text = startDate.ToString();
            }
        }

        private void RenderDescription(SPListItem item)
        {
            if (item["Description"] != null)
            {
                Description.Text = item["Description"].ToString().Replace("\r\n", "<br/>");
            }
        }

        protected void PlaceBid(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            double bid;
            if (double.TryParse(YourBid.Text, out bid))
            {
                Page.Response.Redirect(string.Format("{0}?uicontrol=ConfirmBid&ItemId={1}&BidAmount={2}", Page.Request.Path, _itemId, bid));
            }
        }

        protected void ValidateBid(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            if (_itemId == -1) return;

            double bid;

            if (double.TryParse(YourBid.Text, out bid))
            {
                var web = SPContext.Current.Web;
                var list = web.Lists[Constants.ItemsListName];
                var item = list.GetItemById(_itemId);

                if (item["Bid"] != null)
                {
                    var increment = Convert.ToDouble(item["MinimumBidIncrement"].ToString());
                    _currentBid = Convert.ToDouble(item["Bid"].ToString());
                    args.IsValid = bid >= _currentBid + increment;
                }
                else
                {
                    var startingBid = Convert.ToDouble(item["StartingBid"].ToString());
                    args.IsValid = bid >= startingBid;
                }
            }
            else
            {
                args.IsValid = false;
            }
        }
    }
}
