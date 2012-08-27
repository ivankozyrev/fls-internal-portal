using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLS.SharePoint.BdcModel.BdcModel1
{
    /// <summary>
    /// This class contains the properties for Entity1. The properties keep the data for Entity1.
    /// If you want to rename the class, don't forget to rename the entity in the model xml as well.
    /// </summary>
    public partial class ExternalUser
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
