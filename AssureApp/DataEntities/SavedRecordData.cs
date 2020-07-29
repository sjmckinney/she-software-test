using System;
using System.Collections.Generic;
using System.Text;

namespace AssureApp.DataEntities
{
    public class SavedRecordData
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string OrgUnit { get; set; }
        public string Description { get; set; }
        public string SampleDate { get; set; }
        public string TestPassed { get; set; }
        public string EnvironmentalAssessmentReference { get; set; } = "";

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }
}
