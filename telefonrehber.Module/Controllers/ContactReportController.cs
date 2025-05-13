using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using telefonrehber.Module.BusinessObjects;

namespace telefonrehber.Module.Controllers
{
    public partial class ContactReportController : ObjectViewController<ListView, Contact>
    {
        private DateTime _startDate = DateTime.Today.AddDays(-30); // Varsayılan: Son 30 gün
        private DateTime _endDate = DateTime.Today; // Varsayılan: Bugün

        public ParametrizedAction StartDateAction { get; }
        public ParametrizedAction EndDateAction { get; }
        public SimpleAction GenerateReportAction { get; }

        public ContactReportController()
        {
            TargetObjectType = typeof(Contact);

            // Başlangıç tarihi seçimi
            StartDateAction = new ParametrizedAction(this, "StartDate", PredefinedCategory.Edit, typeof(DateTime))
            {
                Caption = "Başlangıç Tarihi",
                ImageName = "Action_DateTime",
                SelectionDependencyType = SelectionDependencyType.Independent
            };
            StartDateAction.Execute += (s, e) =>
            {
                if (e.ParameterCurrentValue is DateTime selectedDate)
                {
                    _startDate = selectedDate.Date; // Tarih formatı
                }
            };
            Actions.Add(StartDateAction);

            // Bitiş tarihi seçimi
            EndDateAction = new ParametrizedAction(this, "EndDate", PredefinedCategory.Edit, typeof(DateTime))
            {
                Caption = "Bitiş Tarihi",
                ImageName = "Action_DateTime",
                SelectionDependencyType = SelectionDependencyType.Independent
            };
            EndDateAction.Execute += (s, e) =>
            {
                if (e.ParameterCurrentValue is DateTime selectedDate)
                {
                    _endDate = selectedDate.Date; // Tarih formatı
                }
            };
            Actions.Add(EndDateAction);

            // Rapor alma işlemi
            GenerateReportAction = new SimpleAction(this, "GenerateReport", PredefinedCategory.View)
            {
                Caption = "Rapor Al",
                ImageName = "BO_Report",
                SelectionDependencyType = SelectionDependencyType.Independent
            };
            GenerateReportAction.Execute += GenerateReportAction_Execute;
            Actions.Add(GenerateReportAction);
        }

        private void GenerateReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Contact));

            // Seçilen tarihlerin doğruluğunu loglayalım
            System.Diagnostics.Trace.WriteLine($"Seçilen Başlangıç: {_startDate:dd.MM.yyyy}");
            System.Diagnostics.Trace.WriteLine($"Seçilen Bitiş: {_endDate:dd.MM.yyyy}");

            XPQuery<Contact> contacts = new XPQuery<Contact>(((XPObjectSpace)objectSpace).Session);

            // Verileri filtrele
            var filteredContacts = contacts.Where(c => c.DateAdded >= _startDate && c.DateAdded <= _endDate);
            int filteredCount = filteredContacts.Count();

            // CollectionSource oluştur
            var collectionSource = new CollectionSource(objectSpace, typeof(Contact));
            collectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateAdded >= ? AND DateAdded <= ?", _startDate, _endDate);

            // Yeni ListView oluştur
            ListView listView = Application.CreateListView(
                Application.FindListViewId(typeof(Contact)), // ListView ID al
                collectionSource,
                false
            );

            listView.Caption = $"📅 {_startDate:dd.MM.yyyy} - {_endDate:dd.MM.yyyy} | Toplam {filteredCount} kişi bulundu.";

            // Modal pencere olarak aç
            e.ShowViewParameters.CreatedView = listView;
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            e.ShowViewParameters.Context = TemplateContext.View;
        }

    }
}
