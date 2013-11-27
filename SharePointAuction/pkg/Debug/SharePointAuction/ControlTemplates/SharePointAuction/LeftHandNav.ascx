<%@ Assembly Name="SharePointAuction, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7246e83c03e2ae90" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftHandNav.ascx.cs"
    Inherits="SharePointAuction.ControlTemplates.SharePointAuction.LeftHandNav" %>
<!--
    <div class="auction-image">
        <a href="#">
            <img alt="SharePointAuctions" width="150px" height="150px"/>
        </a>
    </div>
    -->
<div class="auction-shaded-box">
    <!--
        <div class="auction-left-nav-section">
            <div>Search</div>
            <div><input type="search"/></div>
        </div>
        -->
    <div class="auction-left-nav-section">
        <asp:BulletedList runat="server" ID="ItemNavigationList" DisplayMode="HyperLink">
        </asp:BulletedList>
    </div>
    <div class="auction-left-nav-section">
        <asp:BulletedList runat="server" ID="CategoryList" DisplayMode="HyperLink">
        </asp:BulletedList>
    </div>
    <div class="auction-left-nav-section">
        <asp:HyperLink ID="InfoLink" runat="server" Text="Auction Information"></asp:HyperLink>
    </div>
</div>
