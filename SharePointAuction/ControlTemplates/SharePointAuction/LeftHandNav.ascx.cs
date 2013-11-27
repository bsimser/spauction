using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using SharePointAuction.Core;

namespace SharePointAuction.ControlTemplates.SharePointAuction
{
    public partial class LeftHandNav : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BuildItemLinks();
            BuildCategoryList();
            InfoLink.NavigateUrl = string.Format("{0}?uicontrol=AuctionInformation", Page.Request.Path);
        }
        
        private void BuildItemLinks()
        {
            var list = SPContext.Current.Web.Lists.TryGetList(Constants.ItemsListName);
            if (list == null) return;
            ItemNavigationList.Items.Clear();
            ItemNavigationList.Items.Add(new ListItem(string.Format("All Items ({0})", list.Items.Count), Page.Request.Path));
        }

        private void BuildCategoryList()
        {
            CategoryList.Items.Clear();
            var list = SPContext.Current.Web.Lists.TryGetList(Constants.CategoryListName);
            if (list == null) return;
            var items = list.Items;
            foreach (SPListItem item in items)
            {
                CategoryList.Items.Add(new ListItem(item.Title, string.Format("{0}?catid={1}", Page.Request.Path, item.ID)));
            }
        }
    }
}
