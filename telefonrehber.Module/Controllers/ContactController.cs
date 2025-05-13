//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Actions;
//using DevExpress.ExpressApp.Editors;
//using DevExpress.ExpressApp.Layout;
//using DevExpress.ExpressApp.Model.NodeGenerators;
//using DevExpress.ExpressApp.SystemModule;
//using DevExpress.ExpressApp.Templates;
//using DevExpress.ExpressApp.Utils;
//using DevExpress.Persistent.Base;
//using DevExpress.Persistent.Validation;
//using telefonrehber.Module.BusinessObjects;
//using System;
//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Actions;
//using DevExpress.ExpressApp.Model;
//using telefonrehber.Module.BusinessObjects;

//namespace telefonrehber.Module.Controllers
//{
//    public partial class ContactController : ObjectViewController<ListView, Contact>
//    {
//        public ContactController()
//        {
//            // Yeni kişi eklemek için bir Action tanımlıyoruz.
//            SimpleAction newContactAction = new SimpleAction(this, "NewContact", PredefinedCategory.Edit)
//            {
//                Caption = "Yeni Kişi Ekle",
//                ConfirmationMessage = "Yeni kişi eklemek istediğinizden emin misiniz?",
//                ImageName = "Action_New_32x32"
//            };

//            newContactAction.Execute += NewContactAction_Execute;
//        }

//        private void NewContactAction_Execute(object sender, SimpleActionExecuteEventArgs e)
//        {
//            var currentUser = ObjectSpace.GetObjectByKey<ApplicationUser>((Guid)SecuritySystem.CurrentUserId);
//            var newContact = ObjectSpace.CreateObject<Contact>();
//            newContact.Owner = currentUser;  // User'ı Owner olarak atıyoruz
//            ObjectSpace.CommitChanges();

//            var detailView = Application.CreateDetailView(ObjectSpace, newContact);
//            Application.ShowViewStrategy.ShowViewInPopupWindow(detailView);
//        }

//        // OnActivated metodunu burada override ediyoruz
//        protected override void OnActivated()
//        {
//            base.OnActivated();

//            if (View.CurrentObject is Contact contact)
//            {
//                // IModelClass üzerinden model üyelerini alıyoruz
//                var modelClass = (IModelClass)View.Model;

//                // 'Owner' özelliğini buluyoruz
//                var ownerProperty = modelClass.FindMember(nameof(contact.Owner)) as IModelMember;

//                if (ownerProperty != null)
//                {
//                    // 'Owner' özelliğini gizli yapmak için 'Visible' özelliğini değiştirelim
//                    //ownerProperty.Visible = false; // Owner alanını gizliyoruz
//                }
//            }
//        }
//    }
//}
