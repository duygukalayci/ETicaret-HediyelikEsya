using GiftShop.Entity.Entities;
using System.Net;
using System.Net.Mail;

namespace GiftShop.WebUI.Utils
{
    public class MailHelper
    {
        public static async Task SendMailAsync(ContactMessage contactMessage)
        {
            using (var smtpClient = new SmtpClient("mail.siteadi.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("bilgi@hediyestore.com", "7896969");
                smtpClient.EnableSsl = true;

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("bilgi@hediyestore.com");
                    mailMessage.To.Add(new MailAddress("infohediyestore@gmail.com")); 
                    mailMessage.Subject = "Siteden Mesaj Geldi";
                    mailMessage.IsBodyHtml = true;

                    mailMessage.Body = $@"
                        <h2>Yeni İletişim Mesajı</h2>
                        <p><strong>Adı:</strong> {contactMessage.Name}</p>
                        <p><strong>Email:</strong> {contactMessage.Email}</p>
                        <p><strong>Konu:</strong> {contactMessage.Subject}</p>
                        <p><strong>Mesaj:</strong> {contactMessage.Message}</p>
                        <p><strong>Tarih:</strong> {(contactMessage.CreatedAt.HasValue ? contactMessage.CreatedAt.Value.ToString("dd/MM/yyyy HH:mm") : "Bilinmiyor")}</p>
                    ";

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
