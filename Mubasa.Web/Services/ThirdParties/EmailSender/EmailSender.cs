using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Web.Services.ThirdParties.EmailSender
{
    public class EmailSender : IEmailSender
    {
        //private readonly IOptions<MailGun> _mailgun;

        //public EmailSender(IOptions<MailGun> mailgun)
        //{
        //    _mailgun = mailgun;
        //}

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                //using (var mail = new MimeMessage())
                //{
                //    mail.From.Add(new MailboxAddress("Mubasa.Com", _mailgun.Value.Address));
                //    mail.To.Add(MailboxAddress.Parse(email));
                //    mail.Subject = subject;
                //    mail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                //    {
                //        Text = htmlMessage
                //    };

                //    using (MailKit.Net.Smtp.SmtpClient smtp = new MailKit.Net.Smtp.SmtpClient())
                //    {
                //        smtp.Connect(
                //            _mailgun.Value.Smtp,
                //            _mailgun.Value.Port,
                //            false);

                //        smtp.AuthenticationMechanisms.Remove("XOAUTH2");

                //        smtp.Authenticate(
                //            _mailgun.Value.Address,
                //            _mailgun.Value.Password);

                //        smtp.Send(mail);
                //        smtp.Disconnect(true);
                //    }
                //}

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
