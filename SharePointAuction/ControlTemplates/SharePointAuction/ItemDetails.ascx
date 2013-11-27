<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemDetails.ascx.cs" Inherits="SharePointAuction.ControlTemplates.SharePointAuction.ItemDetails" %>
<div class="auction_item_page">
    <h1>
        <asp:Literal ID="AuctionTitle" runat="server" Text="Label" />
    </h1>
    <div class="page_auction-title">
        <div class="auction-return-link">
            <asp:HyperLink ID="HomeLinkTop" runat="server">Return to Auction</asp:HyperLink>
        </div>
        <div class="auction-title">
            <asp:Literal runat="server" ID="ItemTitle" Text="Title"></asp:Literal>
        </div>
        <div class="auction-subtitle">
            <asp:Literal runat="server" ID="ItemSubtitle" Text="Subtitle"></asp:Literal>
        </div>
    </div>
    <asp:Panel runat="server" ID="PicturePanel">
        <div id="ai-pics-box">
            <asp:Literal runat="server" ID="PictureList"></asp:Literal>
        </div>
    </asp:Panel>
    <div class="auction-bid-area">
        <asp:Panel runat="server" ID="YouWonPanel">
            <div class="formRow">
                <div class="auction-form-won">
                    <asp:Literal ID="Literal1" runat="server" Text="Congratulations! You won this item." />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="StartingBidPanel">
            <div class="formRow">
                <div class="auction-form-label">
                    <asp:Literal runat="server" ID="BidSellLabel" Text="Starting Bid:"></asp:Literal>
                </div>
                <div class="auction-form-field bold">
                    <asp:Literal runat="server" ID="BidSellValue" Text="$120.00"></asp:Literal>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="StartPanel">
            <div class="formRow">
                <div class="auction-form-label">
                    <asp:Literal runat="server" ID="StartLabelText" Text="Starts:"></asp:Literal>
                </div>
                <div class="auction-form-field">
                    <asp:Literal runat="server" ID="StartLabelValue" Text="October 29, 2012 03:00 PM"></asp:Literal>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="HighestBidderPanel">
            <div class="formRow">
                <div class="auction-form-leading">
                    <asp:Literal ID="Literal2" runat="server" Text="You are the leading bidder"></asp:Literal>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="CurrentBidPanel">
            <div class="formRow">
                <div class="auction-form-label">
                    <asp:Literal runat="server" ID="CurrentBidLabel" Text="Current Bid:"></asp:Literal>
                </div>
                <div class="auction-form-field">
                    <asp:Literal runat="server" ID="CurrentBidValue" Text="$5.00"></asp:Literal>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="PlaceBidPanel">
            <div class="auction-bid-box">
                <div class="formRow">
                    <div class="auction-form-label">
                        My Bid Amount:</div>
                    <div class="auction-form-field">
                        <span class="auction-bid-currency-symbol">$</span>
                        <asp:TextBox runat="server" ID="YourBid" Columns="8">
                        </asp:TextBox>
                        <div class="auction-form-hint">
                            <asp:Literal runat="server" ID="MinimumBidLabel" Text="(Enter $2.00 or more)"></asp:Literal>
                        </div>
                        <div class="auction-validation-message">
                            <asp:CustomValidator ID="BidValidator" ControlToValidate="YourBid" OnServerValidate="ValidateBid"
                                CssClass="ms-error" runat="server" ErrorMessage="Bid amount must be a number and your bid must be higher than the current bid." />
                        </div>
                    </div>
                    <div class="auction-form-field">
                        <asp:Button runat="server" CssClass="auction-button" ID="YourBidButton" Text="Place My Bid"
                            OnClick="PlaceBid" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="formRow">
            <div class="auction-form-label">
                <asp:Literal runat="server" ID="EndLabelText" Text="Ends:"></asp:Literal>
            </div>
            <div class="auction-form-field">
                <asp:Literal runat="server" ID="EndLabelValue" Text="October 30, 2012 03:00 PM"></asp:Literal>
            </div>
        </div>
        <asp:Panel runat="server" ID="MarketValuePanel">
            <div class="formRow">
                <div class="auction-form-label">
                    <asp:Literal ID="Literal3" runat="server" Text="Fair Market Value:"></asp:Literal>
                </div>
                <div class="auction-form-field">
                    <asp:Literal runat="server" ID="MarketValue" Text="$800.00"></asp:Literal>
                </div>
            </div>
        </asp:Panel>
        <div class="formRow">
            <div class="auction-form-label">
                Number of Bids:
            </div>
            <div class="auction-form-field">
                <asp:Literal runat="server" ID="NumberOfBidsLabel" Text="0"></asp:Literal>
            </div>
            <div id="auction-bid-history">
                <asp:Literal runat="server" ID="BidHistory"></asp:Literal>
            </div>
        </div>
    </div>
    <div id="auction-right-hand-box">
        <SharePoint:SPSecurityTrimmedControl ID="SPSecurityTrimmedControlName" PermissionsString="ManageWeb"
            runat="server">
            <div id="auction-admin-links">
                <div class="auction-listbox-title">
                    Admin Controls</div>
                <ul class="auction-menu-items">
                    <li><a href="javascript:newAuctionItem()">Add Another Item</a></li>
                    <li>
                        <asp:HyperLink runat="server" ID="EditLink" Text="Edit Item"></asp:HyperLink></li>
                    <%--                    <li><a href="#">Duplicate Item</a></li>--%>
                    <%--                    <li><a href="#">Hide Item</a></li>--%>
                    <%--                    <li><a href="#">Remove Item</a></li>--%>
                </ul>
            </div>
        </SharePoint:SPSecurityTrimmedControl>
        <div id="auction-category-links">
            <div class="auction-listbox-title">
                Categories</div>
            <asp:BulletedList runat="server" ID="CategoryList" DisplayMode="Text" CssClass="auction-menu-items">
            </asp:BulletedList>
        </div>
    </div>
    <div class="auction-item-section">
        <strong>Description:</strong>
        <div class="ai_description">
            <asp:Literal runat="server" ID="Description" Text="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean quis tellus quam. Quisque sollicitudin luctus tincidunt. Quisque nisl lacus, vulputate eget consequat a, vestibulum sed urna. In hendrerit, nibh vitae facilisis semper, felis massa hendrerit ante, vel dictum enim orci eget eros. Curabitur sit amet quam eget erat porttitor pellentesque. Nulla egestas eleifend ipsum, id luctus libero facilisis sed. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nulla gravida vestibulum malesuada. Pellentesque quis congue dolor." />
        </div>
    </div>
    <div class="auction-item-section">
        <strong>Pickup Instructions:</strong>
        <div class="ai_description">
            <asp:Literal runat="server" ID="PickupInstructions" Text="Not specified"></asp:Literal>
        </div>
    </div>
    <asp:Panel runat="server" ID="DonorPanel">
        <div class="auction-item-section">
            <strong>Donated By:</strong>
            <div class="ai_description">
                <asp:HyperLink runat="server" ID="DonorLink" Text="Donor Name"></asp:HyperLink>
            </div>
        </div>
    </asp:Panel>
    <!--
    <div class="auction-item-section">
        <strong>Contacts:</strong> <a class="aa-edit-link" href="javascript:void(0)">edit</a>
        <div class="ai_description">
            <div>
                <a href="javascript:void(0)" title="View Contact Information" data-tip-text="&lt;div class=&quot;contact-info&quot;&gt;&lt;div&gt;&lt;span&gt;Name:&lt;/span&gt;Bil Simser&lt;/div&gt;&lt;div&gt;&lt;span&gt;Email:&lt;/span&gt;bil.simser@fortisalberta.com&lt;/div&gt;&lt;/div&gt;"
                    data-tip-auction-title="Contact Information">Bil Simser </a>
            </div>
        </div>
    </div>
       -->
    <SharePoint:SPSecurityTrimmedControl ID="SPSecurityTrimmedControl1" PermissionsString="ManageWeb"
        runat="server">
        <div class="admin_panel">
            <div class="admin_panel_auction-title">
                Viewable by auction administrators only:</div>
            <div class="admin_panel_row">
                <strong>Starting Bid:</strong>
                <asp:Literal runat="server" ID="StartingBid"></asp:Literal>
            </div>
            <div class="admin_panel_row">
                <strong>Minimum Bid Increment:</strong>
                <asp:Literal runat="server" ID="MinimumBidIncrement"></asp:Literal>
            </div>
        </div>
    </SharePoint:SPSecurityTrimmedControl>
    <div class="auction-return-link-bottom">
        <asp:HyperLink ID="HomeLinkBottom" runat="server">Return to Auction</asp:HyperLink>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('#myGallery').galleryView({
            enable_overlays: false,
            panel_width: 320,
            panel_height: 240,
            panel_scale: 'fit',
            show_captions: true
        });
        $('#auction-bid-history').hide();
    });
</script>
