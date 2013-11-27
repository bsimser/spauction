<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Home.ascx.cs" Inherits="SharePointAuction.ControlTemplates.SharePointAuction.Home" %>
<%@ Register src="LeftHandNav.ascx" tagName="LeftHandNav" tagPrefix="uc2" %>
<%@ Register src="ItemGrid.ascx" tagName="ItemGrid" tagPrefix="uc1" %>
<div id="auction-show">
    <div>
        <asp:Literal ID="AuctionTitle" runat="server" Text="Label" />
    </div>
    <div class="auction-left-nav">
        <uc2:lefthandnav id="LeftHandNav1" runat="server" />
    </div>
    <div class="auction-right-content">
        <uc1:itemgrid id="ItemGrid1" runat="server" />
    </div>
</div>
