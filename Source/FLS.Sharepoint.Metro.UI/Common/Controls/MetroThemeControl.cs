using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FLS.Sharepoint.Metro.UI.Utilites;
using Microsoft.SharePoint;

namespace FLS.Sharepoint.Metro.UI.Common.Controls
{
    public class MetroThemeControl : WebControl
    {
        private const string DefaultSiteLogo = "/_layouts/FLS.Sharepoint.Metro/images/blue/siteIcon.png";
        private const string DefaultSiteCss = "/_layouts/FLS.Sharepoint.Metro/css/Blue.css";
        private const string MetroAccordionJs = "/_layouts/FLS.Sharepoint.Metro/js/MetroAccordion.js";
        private const string MetroAccordionClickJs = "/_layouts/FLS.Sharepoint.Metro/js/MetroAccordionClick.js";
        private bool _locallyStoredInheritedColourValue;
        private bool _globalStoredAccordionValue;
        private bool _locallyStoredAccordionValue;
        private bool _activateAccordion;
        private bool _globallyStoredFloatedRibbon;
        private bool _globalUseThemeIcon;
        private string _themeCss = string.Empty;
        private string _globalQuickLaunchPosition = string.Empty;
        private string _globalAccordionTypeValue = string.Empty;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            var siteId = SPContext.Current.Site.ID;
            var webId = SPContext.Current.Web.ID;
            SPSecurity.RunWithElevatedPrivileges(
                () =>
                {
                    using (var site = new SPSite(siteId))
                    {
                        using (var web = site.OpenWeb(webId))
                        {
                            if (web.IsRootWeb)
                            {
                                _locallyStoredInheritedColourValue = Convert.ToBoolean(web.GetPropertyValue("InheritMetroThemeColor"));
                            }

                            _globalStoredAccordionValue = Convert.ToBoolean(site.RootWeb.GetPropertyValue("GlobalMetroAccordionActivated"));
                            _globallyStoredFloatedRibbon = Convert.ToBoolean(site.RootWeb.GetPropertyValue("GlobalMetroFloatedRibbon"));
                            _globalUseThemeIcon = Convert.ToBoolean(site.RootWeb.GetPropertyValue("GlobalUseThemeIcon"));
                            _globalAccordionTypeValue = web.GetPropertyValue("GlobalMetroAccordionType");
                            _globalQuickLaunchPosition = site.RootWeb.GetPropertyValue("GlobalMetroQuickLaunchPosition");
                            _locallyStoredAccordionValue = Convert.ToBoolean(web.GetPropertyValue("MetroAccordionActivated"));
                            _activateAccordion = _globalStoredAccordionValue || _locallyStoredAccordionValue;
                            _themeCss = _locallyStoredInheritedColourValue
                                ? web.ParentWeb.GetPropertyValue("MetroThemeColor")
                                : web.GetPropertyValue("MetroThemeColor");
                            ManageQuicklaunch(_globalQuickLaunchPosition);
                            ManageTheme(_themeCss, web);
                            ManageAccordion(_activateAccordion);
                            ManageRibbon();
                        }
                    }
                });
        }

        private void ManageRibbon()
        {
            if (!_globallyStoredFloatedRibbon)
            {
                return;
            }

            Controls.Add(new LiteralControl("<style>body #ribbonWrapper {  position: fixed; top:0; z-index:1000; width:100%; }#s4-bodyContainer {margin-top:80px;}</style>"));
        }

        private void ManageAccordion(bool storedAccordionValue)
        {
            if (!storedAccordionValue)
            {
                return;
            }

            Controls.Add(
                new LiteralControl(
                    "<script type='text/javascript' src='" +
                    SPContext.Current.Web.Url +
                    (_globalAccordionTypeValue == "Click" ? MetroAccordionClickJs : MetroAccordionJs) +
                    "'></script>"));
        }

        private void ManageQuicklaunch(string globalQuickLaunchPosition)
        {
            if (globalQuickLaunchPosition == null)
            {
                return;
            }

            switch (globalQuickLaunchPosition)
            {
                case "Left":
                    Controls.Add(new LiteralControl("<style>body #sideColumn{float:left;margin:0px 0px 0px 30px;}.rightpanel {margin:0 20px 272px !important; clear:right;}</style>"));
                    break;
                case "Right":
                    Controls.Add(new LiteralControl("<style>body #sideColumn{float:right;margin:0px 20px 0px 0px;}.rightpanel {margin:0 272px 30px !important; clear:left;}</style>"));
                    break;
            }
        }

        private void ManageTheme(string storedColorValue, SPWeb web)
        {
            try
            {
                if (string.IsNullOrEmpty(storedColorValue))
                {
                    Controls.Add(
                        new LiteralControl(
                            "<link rel='stylesheet' type='text/css' href='" +
                            SPContext.Current.Web.Url +
                            DefaultSiteCss +
                            "'/>"));
                    if (_globalUseThemeIcon)
                    {
                        return;
                    }

                    web.AllowUnsafeUpdates = true;
                    web.SiteLogoUrl = DefaultSiteLogo;
                    web.Update();
                }
                else
                {
                    var itemById = SPContext.Current.Site.RootWeb.Lists["Metro UI Themes"].GetItemById(Convert.ToInt32(storedColorValue));
                    var cssUrl = itemById["CssUrl"] == null ? string.Empty : itemById["CssUrl"].ToString();
                    var siteLogoUrl = itemById["SiteIconUrl"] == null ? string.Empty : itemById["SiteIconUrl"].ToString();
                    Controls.Add(new LiteralControl("<link rel='stylesheet' type='text/css' href='" + SPContext.Current.Web.Url + cssUrl + "'/>"));
                    if (_globalUseThemeIcon)
                    {
                        web.AllowUnsafeUpdates = true;
                        web.SiteLogoUrl = siteLogoUrl;
                        web.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Controls.Add(new Label { Text = ex.Message });
            }
        }
    }
}
