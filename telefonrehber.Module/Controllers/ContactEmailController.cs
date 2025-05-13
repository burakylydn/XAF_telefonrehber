using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using telefonrehber.Module.BusinessObjects;
using telefonrehber.Module.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace telefonrehber.Module.Controllers
{
    public class ContactEmailController : ObjectViewController<ObjectView, Contact>
    {
        public ContactEmailController()
        {
            SimpleAction sendEmailAction = new SimpleAction(this, "SendEmail", DevExpress.Persistent.Base.PredefinedCategory.Edit)
            {
                Caption = "E-Posta Gönder",
                ImageName = "Mail",
                //IconUrl = "images/Mail.svg",  // İkon URL'si
                //IconCssClass = "my-style",  // CSS sınıfı (varsa)
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject
            };


            sendEmailAction.Execute += async (s, e) => await SendEmailAsync(e.CurrentObject as Contact);
        }

        private async Task SendEmailAsync(Contact contact)
        {
            if (contact == null || string.IsNullOrEmpty(contact.Email))
            {
                throw new UserFriendlyException("E-posta adresi bulunamadı!");
            }

            // Rastgele bir kod üretelim
            Random random = new Random();
            int randomCode = random.Next(1000, 9999);

            string subject = "Rehber Uygulaması Test E-postası";
            string message = $"Merhaba {contact.FullName},<br/><br/>Bu bir test e-postasıdır. Rastgele kodunuz: <b>{randomCode}</b>";

            // EmailService kullanarak e-posta gönder
            var emailService = Application.ServiceProvider.GetRequiredService<EmailService>();
            if (emailService != null)
            {
                await emailService.SendEmail(contact.Email, subject, message);
                Application.ShowViewStrategy.ShowMessage($"E-posta başarıyla gönderildi! ({contact.Email})");
            }
            else
            {
                throw new UserFriendlyException("E-posta servisi başlatılamadı!");
            }
        }
    }
}
