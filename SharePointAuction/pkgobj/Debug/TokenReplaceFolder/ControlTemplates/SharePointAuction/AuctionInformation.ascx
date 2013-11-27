<%@ Assembly Name="SharePointAuction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7246e83c03e2ae90" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuctionInformation.ascx.cs" Inherits="SharePointAuction.ControlTemplates.SharePointAuction.AuctionInformation" %>
<div class="auction_item_page">
    <h1>
        <asp:Label ID="AuctionTitle" runat="server" Text="Label" />
    </h1>
    <div class="auction-item-section">
        <div>
            <strong>Starts:</strong>
            <asp:Label runat="server" ID="StartDateTime" Text="September 28, 2012 06:15 AM MDT" />
        </div>
        <div>
            <strong>Ends:</strong>
            <asp:Label runat="server" ID="EndDateTime" Text="September 29, 2012 06:15 AM MDT" />
        </div>
    </div>
    <div class="auction-item-section">
        <strong>Description:</strong>
        <div>
            <asp:Label runat="server" ID="Description" Text="Auction Description" />
        </div>
    </div>
    <div class="auction-item-section">
        <strong>Pickup Instructions:</strong>
        <div>
            <asp:Label runat="server" ID="PickupInstructions" Text="Pickup Instructions" />
        </div>
    </div>
    <div class="clear" />
    <div id="auction-right-hand-box">
        <SharePoint:SPSecurityTrimmedControl ID="SPSecurityTrimmedControlName" PermissionsString="ManageWeb"
            runat="server">
            <div id="auction-admin-links">
                <div class="auction-listbox-title">
                    Admin Controls</div>
                <ul class="auction-menu-items">
                    <li><asp:LinkButton runat="server" ID="CloseLink" Text="Close Auction"/></li>
                    <li><asp:HyperLink ID="EditLink" runat="server" Text="Edit Auction" NavigateUrl="javascript:auctionOpenModalURL(L_Menu_BaseUrl + '/Lists/SPAuctionConfig/EditForm.aspx?ID=1', 'Edit Auction Information', 645, 800, 'refreshCallback', false)"></asp:HyperLink></li>
                    <li><asp:HyperLink ID="HyperLink1" runat="server" Text="Edit Categories" NavigateUrl="javascript:auctionOpenModalURL(L_Menu_BaseUrl + '/Lists/SPAuctionCategory', 'Edit Categories', 640, 480, 'SilentCallback', false)"></asp:HyperLink></li>
                    <li><asp:HyperLink ID="HyperLink2" runat="server" Text="Edit Donors" NavigateUrl="javascript:auctionOpenModalURL(L_Menu_BaseUrl + '/Lists/SPAuctionDonors', 'Edit Donors', 640, 480, 'SilentCallback', false)"></asp:HyperLink></li>
                    <li><asp:HyperLink ID="HyperLink3" runat="server" Text="Edit Pictures" NavigateUrl="javascript:GoToPage(L_Menu_BaseUrl + '/SPAuctionPictures')"></asp:HyperLink></li>
<%--                    <li><asp:HyperLink runat="server" Text="View Analytics"></asp:HyperLink></li>--%>
<%--                    <li><asp:HyperLink runat="server" Text="View Administrators"></asp:HyperLink></li>--%>
<%--                    <li><asp:HyperLink runat="server" Text="View Participants List"></asp:HyperLink></li>--%>
                    <li><asp:LinkButton runat="server" ID="ResetItems" Text="Reset Items"></asp:LinkButton></li>
                    <li><asp:LinkButton runat="server" ID="ResetAuction" Text="Reset Auction"></asp:LinkButton></li>
                    <li><asp:HyperLink runat="server" Text="View Sales Summary" ID="SalesSummaryLink"></asp:HyperLink></li>
                </ul>
                <div class="auction-amountRaised-label">Amount Raised:</div>
                <div class="auction-amountRaised-value"><asp:Label runat="server" ID="AmountRaised"></asp:Label></div>
            </div>
        </SharePoint:SPSecurityTrimmedControl>
    </div>
    <div class="auction-return-link-bottom">
        <asp:HyperLink ID="HomeLink" runat="server">Return to Auction</asp:HyperLink>
    </div>
</div>
