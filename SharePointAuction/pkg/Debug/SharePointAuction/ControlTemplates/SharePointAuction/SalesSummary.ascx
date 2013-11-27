<%@ Assembly Name="SharePointAuction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7246e83c03e2ae90" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesSummary.ascx.cs" Inherits="SharePointAuction.ControlTemplates.SharePointAuction.SalesSummary" %>
<h1>
    <asp:Literal ID="AuctionTitle" runat="server" Text="Label" />
</h1>
<SharePoint:SPGridView runat="server" ID="ItemsGrid" EnableViewState="false" AutoGenerateColumns="false"
    AllowPaging="false" AllowSorting="false" EnableTheming="true">
    <HeaderStyle HorizontalAlign="Left" ForeColor="Navy" Font-Bold="true" />
    <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="Page,Id" DataNavigateUrlFormatString="{0}?uicontrol=ItemDetails&ItemId={1}"
            DataTextField="Title" HeaderText="Item" />
        <asp:BoundField DataField="Bid" HeaderText="Bid" HtmlEncode="False" />
        <asp:BoundField DataField="IsSold" HeaderText="Sold?" HtmlEncode="False" />
        <asp:BoundField DataField="IsPaid" HeaderText="Paid?" HtmlEncode="False" />
        <asp:BoundField DataField="Bidder" HeaderText="Bidder" HtmlEncode="False" />
        <asp:BoundField DataField="Action" HtmlEncode="False" />
    </Columns>
</SharePoint:SPGridView>
<div class="auction-return-link-bottom">
    <asp:HyperLink ID="HomeLink" runat="server">Return to Auction</asp:HyperLink>
</div>
