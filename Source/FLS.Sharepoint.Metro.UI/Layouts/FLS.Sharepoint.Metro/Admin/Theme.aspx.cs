using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using FLS.Sharepoint.Metro.UI.Utilites;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace FLS.Sharepoint.Metro.UI.Layouts.FLS.Sharepoint.Metro.Admin
{
    public partial class Theme : LayoutsPageBase
    {
        private const string ListName = "Metro UI Themes";
        private string _storedAccordionTypeValue = string.Empty;
        private string _storedColorValue = string.Empty;
        private string _storedQuicklaunchPositionValue = string.Empty;
        private bool _storedGlobalAccordionValue;
        private bool _storedInheritedColourValue;
        private bool _storedAccordionValue;
        private bool _storedGlobalRibbonValue;

        protected override void OnInit(EventArgs e)
        {
            Load += OnLoad;
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            DisableControlChecker();
            if (SPContext.Current.Web.IsRootWeb)
            {
                ApplyAccordionGloballyCB.Checked = _storedGlobalAccordionValue;
            }

            InheritThemeFromParent.Checked = _storedInheritedColourValue;
            if (!Page.IsPostBack)
            {
                PopulateThemeDropDown(SiteMetroBrandingDdb, _storedColorValue);
                PopulateAccordionTypeDropDown(AccordionTypeDropdown, _storedAccordionTypeValue);
                PopulateQuickLaunchPositionDropDown(QuickLaunchPosition, _storedQuicklaunchPositionValue);
            }

            SetThemeValue(_storedColorValue);
            AccordionCheckbox.Checked = _storedAccordionValue;
            FloatRibbonCB.Checked = _storedGlobalRibbonValue;
            if (!_storedGlobalAccordionValue)
            {
                return;
            }

            AccordionStatusMessage.InnerHtml = "Accordion Settings are Inherited. Please navigate to Site Collection Root to manage. <br />";
        }

        private static void PopulateThemeDropDown(ListControl dropDownList, string themeColor)
        {
            var items = SPContext.Current.Site.RootWeb.Lists[ListName].GetItems(new SPQuery { Query = "<where></where>" });

            var list = items
                .Cast<SPListItem>()
                .Select(item => new ListItem { Text = item.Title, Value = item.ID.ToString() })
                .ToList();

            dropDownList.DataSource = list;
            dropDownList.DataValueField = "Value";
            dropDownList.DataTextField = "Text";
            dropDownList.DataBind();
            for (var index = 0; index < dropDownList.Items.Count; ++index)
            {
                if (dropDownList.Items[index].Value == themeColor)
                {
                    dropDownList.SelectedIndex = index;
                }
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            BtnApply.Click += OnBtnApplyClick;
            GetPropertyBagValues();
        }

        private void PopulateAccordionTypeDropDown(ListControl dropDownList, string storedAccordionValue)
        {
            var accordeonTypes = new[] { "Hover", "Click" };
            foreach (var item in accordeonTypes)
            {
                dropDownList.Items.Add(item);
            }

            QuickLaunchPositionLabel.Text = !string.IsNullOrEmpty(storedAccordionValue) ? storedAccordionValue : accordeonTypes[0];

            for (var index = 0; index < dropDownList.Items.Count; ++index)
            {
                if (dropDownList.Items[index].Value == storedAccordionValue)
                {
                    dropDownList.SelectedIndex = index;
                }
            }
        }

        private void PopulateQuickLaunchPositionDropDown(ListControl dropDownList, string themeQuicklaunchPosition)
        {
            var quickLaunchPositions = new[] { "Right", "Left" };
            foreach (var item in quickLaunchPositions)
            {
                dropDownList.Items.Add(item);
            }

            QuickLaunchPositionLabel.Text = !string.IsNullOrEmpty(themeQuicklaunchPosition) ? themeQuicklaunchPosition : quickLaunchPositions[0];

            for (var index = 0; index < dropDownList.Items.Count; ++index)
            {
                if (dropDownList.Items[index].Value == themeQuicklaunchPosition)
                {
                    dropDownList.SelectedIndex = index;
                }
            }
        }

        private void GetPropertyBagValues()
        {
            _storedInheritedColourValue = Convert.ToBoolean(SPContext.Current.Web.GetPropertyValue("InheritMetroThemeColor"));
            _storedColorValue = SPContext.Current.Web.GetPropertyValue("MetroThemeColor");
            _storedAccordionValue = Convert.ToBoolean(SPContext.Current.Web.GetPropertyValue("MetroAccordionActivated"));
            _storedGlobalAccordionValue = Convert.ToBoolean(SPContext.Current.Site.RootWeb.GetPropertyValue("GlobalMetroAccordionActivated"));
            _storedGlobalRibbonValue = Convert.ToBoolean(SPContext.Current.Site.RootWeb.GetPropertyValue("GlobalMetroFloatedRibbon"));
            _storedQuicklaunchPositionValue = SPContext.Current.Site.RootWeb.GetPropertyValue("GlobalMetroQuickLaunchPosition");
            _storedAccordionTypeValue = SPContext.Current.Web.GetPropertyValue("GlobalMetroAccordionType");
        }

        private void DisableControlChecker()
        {
            if (SPContext.Current.Web.IsRootWeb)
            {
                InheritThemeFromParent.Visible = false;
                AccordionStatusMessage.Visible = false;
            }
            else
            {
                QuickLaunchPosition.Visible = false;
                if (_storedGlobalAccordionValue)
                {
                    AccordionCheckbox.Visible = false;
                    ApplyAccordionGloballyCB.Visible = false;
                    AccordionTypeDropdown.Visible = false;
                    FloatRibbonCB.Visible = false;
                }
            }

            if (!_storedInheritedColourValue)
            {
                return;
            }

            SiteMetroBrandingDdb.Enabled = false;
        }

        private void SetThemeValue(string storedColorValue)
        {
            ThemeLabel.Text = !string.IsNullOrEmpty(storedColorValue)
                ? SPContext.Current.Site.RootWeb.Lists[ListName].GetItemById(Convert.ToInt32(storedColorValue)).Title
                : "No Theme Set";
        }

        private void OnBtnApplyClick(object sender, EventArgs e)
        {
            SPContext.Current.Web.AddPropertyAndUpdate("MetroThemeColor", SiteMetroBrandingDdb.SelectedValue);
            SPContext.Current.Web.AddPropertyAndUpdate("MetroAccordionActivated", AccordionCheckbox.Checked.ToString());
            SPContext.Current.Web.AddPropertyAndUpdate("GlobalMetroAccordionType", AccordionTypeDropdown.SelectedValue);
            SPContext.Current.Web.AddPropertyAndUpdate("InheritMetroThemeColor", InheritThemeFromParent.Checked.ToString());
            if (SPContext.Current.Web.IsRootWeb)
            {
                SPContext.Current.Site.RootWeb.AddPropertyAndUpdate("GlobalMetroAccordionActivated", ApplyAccordionGloballyCB.Checked.ToString());
                SPContext.Current.Site.RootWeb.AddPropertyAndUpdate("GlobalMetroFloatedRibbon", FloatRibbonCB.Checked.ToString());
                SPContext.Current.Site.RootWeb.AddPropertyAndUpdate("GlobalMetroQuickLaunchPosition", QuickLaunchPosition.SelectedValue);
            }

            SPUtility.Redirect("settings.aspx", SPRedirectFlags.RelativeToLayoutsPage | SPRedirectFlags.UseSource, HttpContext.Current);
        }
    }
}
