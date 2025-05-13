using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using static DevExpress.CodeParser.CodeStyle.Formatting.Rules;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using System;
using DevExpress.XtraRichEdit.Model;



namespace telefonrehber.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(FullName))]
    public class Contact : BaseObject
    {
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private DateTime dateAdded;

        public Contact(Session session) : base(session)
        {
            if (!session.IsObjectsLoading)
            {
                if (SecuritySystem.CurrentUserId != null)
                {
                    Owner = session.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);
                }
                DateAdded = DateTime.Now.Date;
            }
        }


        [Size(100)]
        public string FirstName
        {
            get { return firstName; }
            set { SetPropertyValue(nameof(FirstName), ref firstName, value); }
        }

        [Size(100)]
        public string LastName
        {
            get { return lastName; }
            set { SetPropertyValue(nameof(LastName), ref lastName, value); }
        }

        [Size(50)]
        [EditorAlias("PhoneNumberEditor")]
        
        [RuleRegularExpression("PhoneNumberRule", DefaultContexts.Save, @"^(\+90|0)\s?\(?\d{3}\)?\s?\d{3}\s?\d{2}\s?\d{2}$", "Lütfen geçerli bir telefon numarasý girin.")]
        [Appearance("PhoneNumberAppearance", Context = "DetailView")]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value); }
        }

        [Size(255)]
        [RuleRegularExpression("EmailRule", DefaultContexts.Save, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", "Lütfen geçerli bir e-posta adresi girin.")]
        public string Email
        {
            get { return email; }
            set { SetPropertyValue(nameof(Email), ref email, value); }
        }

        [PersistentAlias("Concat(FirstName, ' ', LastName)")]
        [Appearance("FullNameInDetailView", Context = "DetailView", Visibility = ViewItemVisibility.Hide)]
        [Appearance("FullNameInListView", Context = "ListView", Visibility = ViewItemVisibility.Show)]
        public string FullName
        {
            get { return Convert.ToString(EvaluateAlias(nameof(FullName))); }
        }

        [Association("User-Contacts")]
        [Appearance("HideOwnerField", Context = "DetailView", Visibility = ViewItemVisibility.Hide)]
        [Appearance("HideOwnerInListView", Context = "ListView", Visibility = ViewItemVisibility.Hide)]
        public ApplicationUser Owner { get; set; }

        [Persistent]
        [Appearance("HideDateAddedField", Context = "DetailView", Visibility = ViewItemVisibility.Hide)]
        [Appearance("HideDateAddedInListView", Context = "ListView", Visibility = ViewItemVisibility.Hide)]
        public DateTime DateAdded
        {
            get { return dateAdded; }
            set { SetPropertyValue(nameof(DateAdded), ref dateAdded, value); }
        }
    }
}

