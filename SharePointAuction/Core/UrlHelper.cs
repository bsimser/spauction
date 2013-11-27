using System;

namespace SharePointAuction.Core
{
    public static class UrlHelper
    {
        public static string GetUrl(Uri request)
        {
            var rc = request.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
            rc += "/" + request.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            return rc;
        }
    }
}