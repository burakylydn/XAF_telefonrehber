using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraReports.UI;

namespace telefonrehber.Module.Reports
{
    public partial class XtraReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport1(DateTime startDate, DateTime endDate)
        {
            InitializeComponent();

            // StartDate parametresi yoksa ekle
            if (this.Parameters["StartDate"] == null)
            {
                var startParam = new DevExpress.XtraReports.Parameters.Parameter
                {
                    Name = "StartDate",
                    Type = typeof(DateTime),
                    Value = startDate
                };
                this.Parameters.Add(startParam);
            }
            else
            {
                this.Parameters["StartDate"].Value = startDate;
            }

            // EndDate parametresi yoksa ekle
            if (this.Parameters["EndDate"] == null)
            {
                var endParam = new DevExpress.XtraReports.Parameters.Parameter
                {
                    Name = "EndDate",
                    Type = typeof(DateTime),
                    Value = endDate
                };
                this.Parameters.Add(endParam);
            }
            else
            {
                this.Parameters["EndDate"].Value = endDate;
            }

            this.FilterString = "[DateAdded] >= ?StartDate AND [DateAdded] <= ?EndDate";
        }
    }
}
