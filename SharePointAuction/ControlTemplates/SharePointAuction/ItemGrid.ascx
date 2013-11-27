<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemGrid.ascx.cs" Inherits="SharePointAuction.ControlTemplates.SharePointAuction.ItemGrid" %>
<div>
    <div>
        <SharePoint:SPSecurityTrimmedControl ID="AddNewItemPanelTop" PermissionsString="ManageWeb" runat="server">
            <div class="auction-add-new-item">
                <a href="javascript:auctionOpenModalURL(L_Menu_BaseUrl + '/Lists/SPAuctionItems/NewForm.aspx', 'New Auction Information', 645, 800, 'refreshCallback', false)">Add new item</a>
            </div>
        </SharePoint:SPSecurityTrimmedControl>
        <div>
            <asp:Label runat="server" ID="AuctionEnds" Text="Auction Ends: October 10, 2012 07:00 PM"></asp:Label>
        </div>
    </div>
    <div>
        <asp:Label runat="server" ID="SearchDisplay" Text="[Category: Displaying 1 item]"></asp:Label>
    </div>
    <div>
        <SharePoint:SPGridView runat="server" ID="ItemsGrid" EnableViewState="false" AutoGenerateColumns="false"
            AllowPaging="false" AllowSorting="false" EnableTheming="true">
            <HeaderStyle HorizontalAlign="Left" ForeColor="Navy" Font-Bold="true" />
            <Columns>
                <asp:BoundField DataField="Image" HtmlEncode="false" />
                <asp:HyperLinkField DataNavigateUrlFields="Page,Id" DataNavigateUrlFormatString="{0}?uicontrol=ItemDetails&ItemId={1}"
                    DataTextField="Title" />
                <asp:BoundField DataField="Bid" HeaderText="Bid" HtmlEncode="False" />
            </Columns>
        </SharePoint:SPGridView>
    </div>
</div>
<SharePoint:SPSecurityTrimmedControl ID="AddNewItemPanelBottom" PermissionsString="ManageWeb" runat="server">
    <div class="auction-add-new-item-bottom">
        <a href="javascript:auctionOpenModalURL(L_Menu_BaseUrl + '/Lists/SPAuctionItems/NewForm.aspx', 'New Auction Information', 645, 800, 'refreshCallback', false)">Add new item</a>
    </div>
</SharePoint:SPSecurityTrimmedControl>
