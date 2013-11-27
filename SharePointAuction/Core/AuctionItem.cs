namespace SharePointAuction.Core
{
    public class AuctionItem
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Bids { get; set; }
        public string Bid { get; set; }
        public string Value { get; set; }
        public string Page { get; set; }
        public string TimeLeft { get; set; }
        public string Bidder { get; set; }
        public string IsSold { get; set; }
        public string IsPaid { get; set; }
        public string Action { get; set; }
    }
}