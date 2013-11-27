using Microsoft.SharePoint;

namespace SharePointAuction.Core
{
    public static class RunAsAdmin
    {
        public delegate void RunWithAdminDelegate(SPSite site, SPWeb web);

        public static void Run(RunWithAdminDelegate myDelegate)
        {
            var webId = SPContext.Current.Web.ID;
            var siteId = SPContext.Current.Site.ID;

            SPSecurity.RunWithElevatedPrivileges(
                delegate
                {
                    using (var site = new SPSite(siteId))
                    {
                        site.AllowUnsafeUpdates = true;
                        using (var web = site.OpenWeb(webId))
                        {
                            web.AllowUnsafeUpdates = true;
                            myDelegate.Invoke(site, web);
                        }
                    }
                }
            );
        }
    }
}