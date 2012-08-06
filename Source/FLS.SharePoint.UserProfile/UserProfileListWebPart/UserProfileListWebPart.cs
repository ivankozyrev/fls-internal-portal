using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;

namespace FLS.SharePoint.UserProfile.UserProfileListWebPart
{
    [ToolboxItemAttribute(false)]
    public class UserProfileListWebPart : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/FLS.SharePoint.UserProfile/UserProfileListWebPart/UserProfileListWebPartUserControl.ascx";

        protected override void CreateChildControls()
        {
            var control = Page.LoadControl(_ascxPath) as UserProfileListWebPartUserControl;
            if (control != null)
            {
                // full access
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    var profileManager = new UserProfileManager(SPServiceContext.Current);
                    control.UserProfileList = GetUserProfileList(profileManager);
                });

                Controls.Add(control);
            }
        }

        private static IEnumerable<Microsoft.Office.Server.UserProfiles.UserProfile> GetUserProfileList(UserProfileManager manager)
        {
            foreach (Microsoft.Office.Server.UserProfiles.UserProfile userProfile in manager)
            {
                yield return userProfile;
            }
        }
    }
}
