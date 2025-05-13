using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using telefonrehber.Module.BusinessObjects;
using Microsoft.Extensions.DependencyInjection;
using telefonrehber.Module.Controllers;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp;
using telefonrehber.Module.Reports;

namespace telefonrehber.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        
#if !RELEASE
        // If a role doesn't exist in the database, create this role
        var defaultRole = CreateDefaultRole();
        var adminRole = CreateAdminRole();

        ObjectSpace.CommitChanges(); //This line persists created object(s).

        UserManager userManager = ObjectSpace.ServiceProvider.GetRequiredService<UserManager>();

        // If a user named 'User' doesn't exist in the database, create this user
        if(userManager.FindUserByName<ApplicationUser>(ObjectSpace, "User") == null) {
            // Set a password if the standard authentication type is used
            string EmptyPassword = "";
            _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "User", EmptyPassword, (user) => {
                // Add the Users role to the user
                user.Roles.Add(defaultRole);
            });
        }

        // If a user named 'Admin' doesn't exist in the database, create this user
        if(userManager.FindUserByName<ApplicationUser>(ObjectSpace, "Admin") == null) {
            // Set a password if the standard authentication type is used
            string EmptyPassword = "";
            _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "Admin", EmptyPassword, (user) => {
                // Add the Administrators role to the user
                user.Roles.Add(adminRole);
            });
        }
        var reportData = ObjectSpace.FirstOrDefault<ReportDataV2>(r => r.DisplayName == "Kişi Raporu");
        if (reportData == null)
        {
            var newReport = ObjectSpace.CreateObject<ReportDataV2>();
            newReport.DisplayName = "Kişi Raporu";
            newReport.PredefinedReportType = typeof(XtraReport1);
            ObjectSpace.CommitChanges();
        }

        ObjectSpace.CommitChanges(); //This line persists created object(s).
#endif
    }
    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
        //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
        //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
    private PermissionPolicyRole CreateAdminRole() {
        PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if(adminRole == null) {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
            adminRole.IsAdministrative = true;
        }
        return adminRole;
    }
    private PermissionPolicyRole CreateDefaultRole()
    {
        PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if (defaultRole == null)
        {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            // Kullanıcının sadece kendi verilerini okumasına izin ver
            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(
                SecurityOperations.Read,
                cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(),
                SecurityPermissionState.Allow
            );

            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);

            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(
                SecurityOperations.Write,
                "ChangePasswordOnFirstLogon",
                cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(),
                SecurityPermissionState.Allow
            );

            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(
                SecurityOperations.Write,
                "StoredPassword",
                cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(),
                SecurityPermissionState.Allow
            );
            defaultRole.AddTypePermissionsRecursively<Contact>(SecurityOperations.Navigate, SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/Contact_ListView", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermissionFromLambda<Contact>(
    SecurityOperations.Read,
    c => true,
    SecurityPermissionState.Allow
);
            defaultRole.AddObjectPermissionFromLambda<Contact>(
                SecurityOperations.Write,
                c => true,
                SecurityPermissionState.Allow
            );
            defaultRole.AddObjectPermissionFromLambda<Contact>(
                SecurityOperations.Create,
                c => true,
                SecurityPermissionState.Allow
            );
            defaultRole.AddObjectPermissionFromLambda<Contact>(
                SecurityOperations.Delete,
                c => true,
                SecurityPermissionState.Allow
            );

            // 🚀 Contact nesnesi için yetkileri ekle
            defaultRole.AddTypePermissionsRecursively<Contact>(SecurityOperations.Read, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<Contact>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<Contact>(SecurityOperations.Write, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<Contact>(SecurityOperations.Delete, SecurityPermissionState.Allow);

            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddObjectPermission<ModelDifference>(SecurityOperations.ReadWriteAccess, "UserId = ToStr(CurrentUserId())", SecurityPermissionState.Allow);
            defaultRole.AddObjectPermission<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, "Owner.UserId = ToStr(CurrentUserId())", SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
    public void CreateNewContact(ApplicationUser currentUser)
    {
        var contact = ObjectSpace.CreateObject<Contact>();

        // Otomatik olarak giriş yapan kullanıcıyı owner olarak ayarla
        contact.Owner = currentUser;

        // İstenilen diğer bilgileri ekle
        //contact.Name = "Yeni Kişi";

        // Kaydet
        ObjectSpace.CommitChanges();
    }
    
}