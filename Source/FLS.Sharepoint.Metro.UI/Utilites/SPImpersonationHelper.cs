using System;
using Microsoft.SharePoint;

namespace FLS.Sharepoint.Metro.UI.Utilites
{
    public static class SPImpersonationHelper
    {
        public delegate void SPSiteAction(SPSite site);

        public delegate void SPWebAction(SPWeb web);

        public delegate void SPListAction(SPList list);

        private delegate void SPSiteActionInternal(SPSite site, bool isNewSite);

        public static void DoAsSystemUser(this SPSite site, SPSiteAction action)
        {
            var systemUser = site.SystemAccount;
            SPSecurity.RunWithElevatedPrivileges(() => DoAsUser(site, systemUser, action));
        }

        public static void DoAsSystemUser(this SPWeb web, SPWebAction action)
        {
            var systemUser = web.Site.SystemAccount;
            SPSecurity.RunWithElevatedPrivileges(() => DoAsUser(web, systemUser, action));
        }

        public static void DoUnsafeUpdateAsSystemUser(this SPWeb web, SPWebAction action)
        {
            DoAsSystemUser(web, (systemWeb => DoUnsafeUpdate(systemWeb, () => action(systemWeb))));
        }

        public static void DoUnsafeUpdate(this SPWeb web, Action action)
        {
            var allowUnsafeUpdates = web.AllowUnsafeUpdates;
            web.AllowUnsafeUpdates = true;
            action();
            web.AllowUnsafeUpdates = allowUnsafeUpdates;
        }

        public static void DoAsUser(this SPSite site, SPUser user, SPSiteAction action)
        {
            DoAsUser(site, user, (impersonatedSite, isNewSite) => action(impersonatedSite));
        }

        public static void DoAsUser(this SPWeb web, SPUser user, SPWebAction action)
        {
            DoAsUser(
                web.Site, 
                user, 
                (impersonatedSite, isNewSite) =>
                {
                    if (!isNewSite)
                    {
                        action(web);
                    }
                    else
                    {
                        using (var resource = impersonatedSite.OpenWeb(web.ID))
                        {
                            action(resource);
                        }
                    }
                });
        }

        private static void DoAsUser(SPSite site, SPUser user, SPSiteActionInternal action)
        {
            if (site.UserToken != null && site.UserToken.CompareUser(user.UserToken))
            {
                action(site, false);
            }
            else
            {
                using (new SPSite(site.ID, user.UserToken))
                {
                    action(site, true);
                }
            }
        }
    }
}
