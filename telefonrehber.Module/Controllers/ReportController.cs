using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Reports;

using telefonrehber.Module.Reports;

namespace telefonrehber.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
    public partial class ReportController : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ReportController()
        {
            InitializeComponent();
            SimpleAction generateReport = new SimpleAction(this, "GenerateReport", "Report")
            {
                Caption = "Rapor Oluştur",
                ImageName = "Action_Report"
            };
            //generateReport.Execute += GenerateReport_Execute;
        }
        //private void GenerateReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    // Parametreleri almak için dialog göster
        //    var report = new ReportsModule().ReportDataProvider.CreateReport(
        //        typeof(telefonrehber.Module.Reports.XtraReport1),
        //        ReportDataProvider.GetReportStorage().GetReportContainerHandle(typeof(XtraReport1))
        //    );

        //    // Tarih parametrelerini kullanıcıdan al
        //    DateTime startDate = DateTime.Now.AddMonths(-1); 
        //    DateTime endDate = DateTime.Now; 

        //    // Rapor parametrelerini ata
        //    report.Parameters["StartDate"].Value = startDate;
        //    report.Parameters["EndDate"].Value = endDate;

        //    // Raporu göster
        //    ReportServiceController controller = Frame.GetController<ReportServiceController>();
        //    if (controller != null)
        //    {
        //        controller.ShowPreview(report);
        //    }
        //}
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
