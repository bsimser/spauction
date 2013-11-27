namespace SharePointAuction.Core
{
    public static class Constants
    {
        public static string ConfigListName = "SPAuctionConfig";
        public static string DonorListName = "SPAuctionDonors";
        public static string BidderListName = "SPAuctionBidders";
        public static string ItemsListName = "SPAuctionItems";
        public static string CategoryListName = "SPAuctionCategory";
        public static string PicturesListName = "SPAuctionPictures";

        public static string DefaultUserControl = "Home";

        public static string ErrorMessage = "We're sorry, the SharePoint Auction web part has experience an unexpected error.";

        public static string NoImageSrc = "<img alt=\"No Image\" title=\"No Image\" src=\"/_layouts/SharePointAuction/img/160x160.gif\"/>";
        public static string NoImageGridSrc = "<img alt=\"No Image\" class=\"auction-item-grid-picture\" title=\"No Image\" src=\"/_layouts/SharePointAuction/img/160x160.gif\"/>";
    }
}