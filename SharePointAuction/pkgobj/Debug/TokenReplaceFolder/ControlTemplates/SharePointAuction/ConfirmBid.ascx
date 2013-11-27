<%@ Assembly Name="SharePointAuction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7246e83c03e2ae90" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmBid.ascx.cs" Inherits="SharePointAuction.ControlTemplates.SharePointAuction.ConfirmBid" %>
<div class="auction-title">
    <h1>
        <asp:Label ID="AuctionTitle" runat="server" Text="Label" />
    </h1>
    <asp:Panel runat="server" ID="AuctionEndedPanel">
        Sorry, the auction has ended.
        <div class="auction-return-link-bottom">
            <asp:HyperLink ID="HomeLink" runat="server">Return to Auction</asp:HyperLink>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="ConfirmBidPanel">
        <asp:Label ID="Title" runat="server" />
        <table class="auction-bid">
            <tr>
                <td>
                    <div class="auction-placeBidLabel">
                        Current Bid:</div>
                </td>
                <td>
                    <div class="auction-placeBid">
                        <asp:Label ID="CurrentBid" runat="server" /></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="auction-yourBidLabel">
                        Your Bid:</div>
                </td>
                <td>
                    <div class="auction-yourBid">
                        <asp:Label ID="YourBid" runat="server" /></div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="ContinueButton" runat="server" Text="Submit Bid" OnClick="OnBidClick" />
                    <asp:HyperLink ID="CancelLink" CssClass="auction-cancelbutton" runat="server">Cancel</asp:HyperLink>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
