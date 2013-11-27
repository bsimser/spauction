using System;
using System.Web.UI;
using Microsoft.SharePoint;
using SharePointAuction.Core;

namespace SharePointAuction.ControlTemplates.SharePointAuction
{
    public partial class Home : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var list = SPContext.Current.Web.Lists.TryGetList(Constants.ConfigListName);
            if (list == null) return;
            if (list.ItemCount == 0) return;
            var config = list.Items[0];
            AuctionTitle.Text = config["Title"].ToString();
        }
    }
}
