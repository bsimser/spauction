using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using SharePointAuction.Core;

namespace SharePointAuction.SharePoint.WebParts.AuctionWebPart
{
    [ToolboxItemAttribute(false)]
    public class AuctionWebPart : WebPart
    {
        private bool _error;

        protected override void CreateChildControls()
        {
            var userControlName = Constants.DefaultUserControl;

            if (_error) return;

            try
            {
                base.CreateChildControls();

                Controls.Add(new CssRegistration
                    {
                        After = "corev4.css",
                        Name = "/_layouts/SharePointAuction/css/auction.css"
                    });

                // if(config.UseJQueryGalleryView)
                // CssRegistration.Register("jquery.galleryview.css");

                if (Page.Request.QueryString["uicontrol"] != null)
                {
                    userControlName = Page.Request.QueryString["uicontrol"];
                }

                // if(config.UseExternalJQuery)
                // Controls.Add(new LiteralControl("location"));
                // else
                // ScriptLink.Register(this.Page, "js/jquery.js");

//                Controls.Add(new LiteralControl("<script src=\"/Style Library/jquery-1.9.0/js/jquery-1.9.0.js\" type=\"text/javascript\"></script>"));

                Controls.Add(LoadUserControl(userControlName));

                Controls.Add(new ScriptLink
                    {
                        Name = "/_layouts/SharePointAuction/js/auction.js",
                        Localizable = false
                    });

//                Controls.Add(new LiteralControl("<script src=\"/Style Library/thewire/unitedway/js/jquery.timers-1.2.js\" type=\"text/javascript\"></script>"));
//                Controls.Add(new LiteralControl("<script src=\"/Style Library/thewire/unitedway/js/jquery.galleryview-3.0-dev.js\" type=\"text/javascript\"></script>"));
//                Controls.Add(new LiteralControl("<script src=\"/Style Library/thewire/unitedway/js/jquery.easing.1.3.js\" type=\"text/javascript\"></script>"));
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private Control LoadUserControl(string userControlName)
        {
            const string controlTemplatePath = @"~/_CONTROLTEMPLATES/SharePointAuction/{0}.ascx";
            var path = string.Format(controlTemplatePath, userControlName);
            return Page.LoadControl(path);
        }

        private void HandleException(Exception exception)
        {
            _error = true;
            Controls.Clear();
            Controls.Add(new LiteralControl(string.Format("<h3>{0}</h3>", Constants.ErrorMessage)));
            Controls.Add(new LiteralControl(string.Format("<div class=\"ms-error\">{0}</div>", exception.Message)));
        }
    }
}
