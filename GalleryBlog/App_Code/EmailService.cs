using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;

namespace GalleryBlog.App_Code
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {

            return sendMail(message);
        }

        void sendMail(MailMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.From))
            {
                message.From = 
            }
            if (string.IsNullOrWhiteSpace(message.Sender.DisplayName))
            {
                message.Sender.DisplayName = ;
            }
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = ConfigurationManager.AppSettings["EmailRecoverySubject"];
            }

            var mailMsg = new SendMailMessage()
            {

                From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                FromDisplayName = ConfigurationManager.AppSettings["SystemEmailFromDisplayName"],
                FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                
                Subject = message.Subject,
            };

            #region formatter
            string text = string.Format("Please click on this link to {0}: {1}", message.Subject, message.Body);
            string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br/>";

            html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser:" + message.Body);
            #endregion
           
            // send email here

            var result = Index(mailMsg);
            return result.Model != null;


            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(mailMsg.From);
            msg.To.Add(message.To);
            msg.CC = ConfigurationManager.AppSettings["AdminEmailCC"];
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("joe@contoso.com", "XXXXXX");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
    }
}