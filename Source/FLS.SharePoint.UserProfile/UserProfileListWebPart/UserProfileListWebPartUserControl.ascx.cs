using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace FLS.SharePoint.UserProfile.UserProfileListWebPart
{
    public partial class UserProfileListWebPartUserControl : UserControl
    {
        public IEnumerable<Microsoft.Office.Server.UserProfiles.UserProfile> UserProfileList { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserProfileList != null)
                {
                    userProfileListRpt.DataSource = UserProfileList.Select(up => new
                                                                                     {
                                                                                         DisplayName = up.DisplayName,
                                                                                         PublicUrl = up.PublicUrl,
                                                                                         Email = up["WorkEmail"].Value ?? string.Empty,
                                                                                         EditLink = "#"
                                                                                     });
                    userProfileListRpt.DataBind();
                }
            }
        }
    }
}
