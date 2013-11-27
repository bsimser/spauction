using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using SharePointAuction.Core;

namespace SharePointAuction.Features.SiteFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("3497bdca-8df5-498c-a1e7-d645065b14f0")]
    public class SiteFeatureEventReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// Create the lists needed for the feature. Order is important here because
        /// there are lookups into other lists that need to be there first.
        /// </summary>
        /// <param name="properties"></param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (var web = properties.Feature.Parent as SPWeb)
            {
                BuildConfigList(web);
                BuildCategoryList(web);
                BuildDonorList(web);
                BuildPictureList(web);
                BuildItemList(web);
                BuildBidderList(web);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            using (var web = properties.Feature.Parent as SPWeb)
            {
            }
        }

        private static void BuildConfigList(SPWeb web)
        {
            if (web.Lists.TryGetList(Constants.ConfigListName) != null) return;

            var id = web.Lists.Add(Constants.ConfigListName, "This custom list was created by the Auction feature to store configuration information about this site.", SPListTemplateType.GenericList);
            web.Update();

            var list = web.Lists[id];

            AddNoteField(list, "Description", true);
            AddNoteField(list, "PickupInstructions");
            list.Fields.Add("StartDate", SPFieldType.DateTime, true);
            list.Fields.Add("EndDate", SPFieldType.DateTime, true);
            AddNoteField(list, "GenericUserAuctionEndedEmail"); // sent to all bidders
            AddNoteField(list, "AuctionEndedTopBidder"); // sent to top bidder of each item
            AddNoteField(list, "AuctionItemLeadingBidder"); // sent to bidder when they place bid on and item
            AddNoteField(list, "AuctionOutbidItem"); // sent to bidder when they are outbid on an item

            list.Update();

            // seed the list with config information
            var item = list.Items.Add();

            item["Title"] = "New Auction";
            item["Description"] = "New Auction Description";
            item["PickupInstructions"] = "New Auction Pickup Instructions";
            item["StartDate"] = DateTime.Now.Date.AddHours(9).AddDays(1);
            item["EndDate"] = DateTime.Now.Date.AddHours(16).AddDays(8);

            var emailBody = "";

            // subject: "Your auction has ended"
            emailBody = "The auction {0} has ended.<br/><br/>";
            emailBody += "Please visit this page to view the sales summary. The sales summary shows all of the items in the auction and the sale information for each item that sold.<br/><br/>";
            emailBody += "<a href='{1}'>{2}</a><br/><br/>";
            emailBody += "Thanks";
            item["GenericUserAuctionEndedEmail"] = emailBody;

            emailBody = "{0},<br/><br/>";
            emailBody += "You have won the {1} item in the {2} auction.<br/><br/>";
            emailBody += "Please visit this page to view the items you won.<br/><br/>";
            emailBody += "<a href='{3}'>{4}</a><br/><br/>";
            emailBody += "Thank you for your support.<br/><br/>";
            emailBody += "Pickup instructions:<br/><br/>";
            emailBody += "{5}<br/><br/>";
            emailBody += "Thanks";
            item["AuctionEndedTopBidder"] = emailBody;

            emailBody = "Congratulations on currently having the <strong>leading bid</strong> on the <strong>{1}</strong>.<br/><br/>";
            emailBody += "This confirms your bid of <strong>{0:c}</strong> for this item. To review your bid, click the Bid History link found on the <a href=\"{2}\">auction item page</a><br/><br/>";
            emailBody += "If you are outbid, we will send an email with a link to the item so you can quickly take back the lead.<br/><br/>";
            emailBody += "Thanks";
            item["AuctionItemLeadingBidder"] = emailBody;

            emailBody = "You have been outbid.<br/></br>";
            emailBody += "Someone has submitted a bid for {0} that is higher than your bid of {1:c}.<br/></br>";
            emailBody += "To review the bids, click the Bid History link found on the auction item page at:<br/><br/>";
            emailBody += "<a href=\"{2}\">{3}</a><br/><br/>";
            emailBody += "Thanks";
            item["AuctionOutbidItem"] = emailBody;

            item.Update();
        }

        private static void BuildDonorList(SPWeb web)
        {
            if (web.Lists.TryGetList(Constants.DonorListName) != null) return;

            var id = web.Lists.Add(Constants.DonorListName, "This custom list was created by the Auction feature to store donors for items in this site.", SPListTemplateType.GenericList);
            web.Update();

            var list = web.Lists[id];

            list.Fields.Add("Website", SPFieldType.URL, false);

            list.Update();
        }

        private static void BuildBidderList(SPWeb web)
        {
            if (web.Lists.TryGetList(Constants.BidderListName) != null) return;

            var id = web.Lists.Add(Constants.BidderListName, "This custom list was created by the Auction feature to store bidders for items in this site.", SPListTemplateType.GenericList);
            web.Update();

            var list = web.Lists[id];
            list.Fields.Add("Bidder", SPFieldType.User, true);
            list.Fields.Add("Amount", SPFieldType.Currency, true);
            AddItemLookupToBidderList(web, list);

            list.Update();
        }

        private static void BuildItemList(SPWeb web)
        {
            if (web.Lists.TryGetList(Constants.ItemsListName) != null) return;

            var id = web.Lists.Add(Constants.ItemsListName, "This custom list was created by the Auction feature to store information about items in this site.", SPListTemplateType.GenericList);
            web.Update();
            var list = web.Lists[id];

            // fields
            list.Fields.Add("Subtitle", SPFieldType.Text, false);
            AddNoteField(list, "Description", true);
            AddCategoryLookupField(web, list);
            list.Fields.Add("Value", SPFieldType.Currency, false);
            // Starting Bid must be greater than or equal to 1.00
            list.Fields.Add("StartingBid", SPFieldType.Currency, true);
            // Minimum Bid Increment must be greater than or equal to 0.50 
            list.Fields.Add("MinimumBidIncrement", SPFieldType.Currency, true);
            AddPictureLookupField(web, list);
            AddDonorLookupField(web, list);

            // internal fields for tracking
            list.Fields.Add("NumberOfBids", SPFieldType.Number, false);
            list.Fields.Add("Bid", SPFieldType.Currency, false);
            list.Fields.Add("Bidder", SPFieldType.User, false);
            list.Fields.Add("IsSold", SPFieldType.Boolean, false);
            list.Fields.Add("IsPaid", SPFieldType.Boolean, false);

            // commit the update
            list.Update();
        }

        private static void AddDonorLookupField(SPWeb web, SPList list)
        {
            list.Fields.AddLookup("Donor", web.Lists[Constants.DonorListName].ID, false);
            list.Update();

            var field = list.Fields["Donor"] as SPFieldLookup;
            if (field == null) return;

            field.LookupField = "Title";
            field.Update();
        }

        private static void AddItemLookupToBidderList(SPWeb web, SPList list)
        {
            list.Fields.AddLookup("Item", web.Lists[Constants.ItemsListName].ID, false);
            list.Update();

            var field = list.Fields["Item"] as SPFieldLookup;
            if (field == null) return;

            field.LookupField = "Title";
            field.Update();
        }

        private static void AddCategoryLookupField(SPWeb web, SPList list)
        {
            list.Fields.AddLookup("Categories", web.Lists[Constants.CategoryListName].ID, true);
            list.Update();

            var field = list.Fields["Categories"] as SPFieldLookup;
            if (field == null) return;

            field.LookupField = "Title";
            field.AllowMultipleValues = true;
            field.Update();
        }

        private static void BuildPictureList(SPWeb web)
        {
            if (web.Lists.TryGetList(Constants.PicturesListName) != null) return;
            web.Lists.Add(Constants.PicturesListName, "This picture library was created by the Auction feature to store images that are used on pages in this site.", SPListTemplateType.PictureLibrary);
            web.Update();
        }

        private static void BuildCategoryList(SPWeb web)
        {
            if (web.Lists.TryGetList(Constants.CategoryListName) != null) return;

            var listId = web.Lists.Add(Constants.CategoryListName, "This custom list was created by the Auction feature to store categories that are used in items in this site.", SPListTemplateType.GenericList);
            web.Update();

            // Items must be related to at least one category so give them a head start
            var list = web.Lists[listId];
            var newItem = list.Items.Add();
            newItem["Title"] = "Other";
            newItem.Update();
        }

        private static void AddNoteField(SPList list, string fieldName, bool isRequired)
        {
            list.Fields.Add(fieldName, SPFieldType.Note, isRequired);
            list.Update();

            var field = list.Fields[fieldName] as SPFieldMultiLineText;
            if (field == null) return;

            field.NumberOfLines = 10;
            field.RichText = false;
            field.Update();
        }

        private static void AddNoteField(SPList list, string fieldName)
        {
            AddNoteField(list, fieldName, false);
        }

        private static void AddPictureLookupField(SPWeb web, SPList list)
        {
            list.Fields.AddLookup("Pictures", web.Lists[Constants.PicturesListName].ID, false);
            list.Update();

            var field = list.Fields["Pictures"] as SPFieldLookup;
            if (field == null) return;

            field.LookupField = "Title";
            field.AllowMultipleValues = true;
            field.Update();
        }
    }
}
