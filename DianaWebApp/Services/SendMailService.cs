using System.Net;
using System.Net.Mail;

namespace DianaWebApp.Services
{
    public static class SendMailService
    {
        public static void SendMessage(string toUser, string userName)
        {
            using(var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("bd7h15zbx@code.edu.az", "brxv pfya jcaz xfot");
                client.EnableSsl = true;
                
                var mailMessage = new MailMessage() 
                {
                    From = new MailAddress("bd7h15zbx@code.edu.az"),
                    Subject = "Welcome to Diana Website",
                    Body = $"<h1>Hello {userName}, " +
                    $"Thank you for visiting our website. " +
                    $"On weekends, you will be sent a message " +
                    $"about discounts\r\nSee you soon!!!",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toUser);
                client.Send(mailMessage);
            }
        }
    }
}
