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
    public partial class TrainingEvent
    {
        public string Title { get; set; }
        public string LoginName { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int TrainingEventID { get; set; }
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public int StudentID { get; set; }
        public int TrainingID { get; set; }
    }
}
