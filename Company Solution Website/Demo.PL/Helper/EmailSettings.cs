using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.sendgrid.net", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("apikey", "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");
            client.Send("amednaser975@gmail.com", email.To, email.Title, email.Body);
        }
    }
}
