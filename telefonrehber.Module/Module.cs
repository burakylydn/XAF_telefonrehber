using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using telefonrehber.Module.BusinessObjects;
using Microsoft.Extensions.DependencyInjection;
using telefonrehber.Module.Services;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using static DevExpress.ExpressApp.ListView;
using DevExpress.ExpressApp.SystemModule;
using telefonrehber.Module.Controllers;
using DevExpress.ExpressApp.ReportsV2;
using telefonrehber.Module.Reports;

// using telefonrehber.Module.Controllers;

namespace telefonrehber.Module;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class telefonrehberModule : ModuleBase
{
    public telefonrehberModule()
    {
        //
        // telefonrehberModule
        //
        // Modül yapılandırması
        AdditionalExportedTypes.Add(typeof(ReportDataV2));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifference));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifferenceAspect));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.BaseObject));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.FileData));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.FileAttachmentBase));
        //RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.ReportsModuleV2));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
    }

    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
    {
        ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
        return new ModuleUpdater[] { updater };
    }
    //public override void RegisterReportsModule(ReportsModuleV2 reportsModule)
    //{
    //    base.RegisterReportsModule(reportsModule);
    //    reportsModule.AdditionalExportedTypes.Add(typeof(ContactReport)); // "ContactReport" yerine gerçek rapor sınıfınızı yazın
    //}
    //public override void Setup(ApplicationModulesManager moduleManager)
    //{
    //    base.Setup(moduleManager);
    //    ReportsModule reportsModule = moduleManager.Modules.FindModule<ReportsModule>();
    //    if (reportsModule != null)
    //    {
    //        reportsModule.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportData);
    //    }
    //}
    public override void Setup(XafApplication application)
    {
        base.Setup(application);
        AdditionalExportedTypes.Add(typeof(Contact));

        // Servisler burada eklenecek
        application.ServiceProvider?.GetRequiredService<EmailService>();
        application.SetupComplete += Application_SetupComplete;
        // ListViewCreating olayına abone olalım
        //application.CreateController<ContactReportController>();
    }
    private void Application_SetupComplete(object sender, EventArgs e)
    {
        var application = (XafApplication)sender;

        // ContactReportController'ı alıp GenerateReportAction butonunu aktif hale getiriyoruz.
        var controller = application.MainWindow?.GetController<ContactReportController>();
        if (controller != null)
        {
            controller.GenerateReportAction.Active.SetItemValue("Enabled", true);
        }
    }
    public override void CustomizeTypesInfo(ITypesInfo typesInfo)
    {
        base.CustomizeTypesInfo(typesInfo);
        CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
    }

}

