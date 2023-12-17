using System.Net;
using System.Net.Mail;

namespace DianaWebApp.Services
{
    public static class SendMailService
    {
        public static void SendMessage(string toUser, string userName, string pinCode = "")
        {
            if(userName == "Diana Team")
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Credentials = new NetworkCredential("bd7h15zbx@code.edu.az", "brxv pfya jcaz xfot");
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage()
                    {
                        From = new MailAddress("bd7h15zbx@code.edu.az"),
                        Subject = "Welcome to Diana Website",
                        Body = $"Hello I am from {userName}" +
                        $"<p>Welcome to Diana, your ultimate destination for style inspiration and exclusive updates! As a valued subscriber, you're in for a treat. Get ready to elevate your inbox with a curated selection of fashion insights, lifestyle trends, and insider news tailored specifically for you. Delight in early access to our latest collections, limited-time offers, and exclusive promotions, all delivered directly to your inbox. Your preferences matter, and we're committed to providing a personalized experience that matches your unique taste. Join our vibrant community, share your thoughts, and engage with fellow subscribers. Your privacy is important to us, and we ensure the security of your information. Expect surprises – from exciting giveaways to special treats, we love to spoil our subscribers. Get set for a subscription journey filled with elegance, sophistication, and endless style possibilities. Embrace your individuality with Diana, where every newsletter is a celebration of your distinct persona.<p>",
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toUser);
                    client.Send(mailMessage);
                }
            }
            else
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Credentials = new NetworkCredential("bd7h15zbx@code.edu.az", "brxv pfya jcaz xfot");
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage()
                    {
                        From = new MailAddress("bd7h15zbx@code.edu.az"),
                        Subject = "Welcome to Diana Website",
                        Body = $"Hello {userName}, " +
                        $"Thank you for visiting our website. " +
                        $"Please write this code to confirmation section\n" +
                        $"\n" +
                        $"Pincode: {pinCode}",
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toUser);
                    client.Send(mailMessage);
                }

            }
        }
    }
}
