using GalleryBlog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GalleryBlog.Controllers
{
    [Authorize]
    public class SendMailerController : Controller
    {
        [AllowAnonymous]
        public bool SendAdminContactPageEmail(string messageName, string messageEmail, string messageBody, string ccEmail, string scheme, string authority)
        {
            try
            {
                // send email here
        
                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailContactFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    Cc = ccEmail,
                    Subject = ConfigurationManager.AppSettings["EmailContactPageSubject"],


                    Body = string.Format("<br /><br />A person named <b>{0}</b> with email {1} has sent a message from the contact page:<br /><br /><br />{2}", messageName, messageEmail, messageBody)
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }

        }
        [AllowAnonymous]
        public bool SendAccountRecoveryEmail(string toEmail, string RecoveryHash)
        {
            try
            {
                // send email here
                var recoveryUrlHost = ConfigurationManager.AppSettings["RecoveryURLHost"];
                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = toEmail,
                    Subject = ConfigurationManager.AppSettings["EmailRecoverySubject"],

                    Body = "<br /><br />Recovery for your Country Yarns Account:<br /><br />Please click <a href='" + recoveryUrlHost + "/Account/RecoverAccount/?message=2&m=" + RecoveryHash + "' >here</a> to continue recovering your account.<br /><br />"
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }

        }

        public bool SendAdminHasOrderEmail(string toEmail, string ccEmail, string orderUser, decimal orderTotal, int webOrderId, string scheme, string authority)
        {
            try
            {
                // send email here
                var serverPath = string.Format("{0}://{1}{2}{3}", scheme, authority, "/WebOrder/Edit/", webOrderId);

                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailOrderFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = toEmail,
                    Cc = ccEmail,
                    Subject = ConfigurationManager.AppSettings["EmailOrderAddedSubject"],


                    Body = string.Format("<br /><br />The user {0} has submitted an order for {1:C}<br /><br />You can view it <a href='{2}'>with this link</a>.", orderUser, orderTotal, serverPath)
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }

        }

        public bool SendAdminHasPreppedOrderEmail(string toEmail, string ccEmail, string orderUser, decimal orderTotal, int webOrderId, string scheme, string authority)
        {
            try
            {
                // send email here
                var serverPath = string.Format("{0}://{1}{2}{3}", scheme, authority, "/WebOrder/Edit/", webOrderId);

                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailOrderFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = toEmail,
                    Cc = ccEmail,
                    Subject = ConfigurationManager.AppSettings["EmailOrderPreppedSubject"],


                    Body = string.Format("<br /><br />The user {0} has prepared their order and it's ready for shipping calculation.<br /><br />You can view it <a href='{2}'>with this link</a>.", orderUser, orderTotal, serverPath)
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }

        }

        public bool SendAdminCustomerUpdatedOrderEmail(string toEmail, string ccEmail, string orderUser, decimal orderTotal, int webOrderId, string scheme, string authority)
        {
            try
            {
                // send email here
                var serverPath = string.Format("{0}://{1}{2}{3}", scheme, authority, "/WebOrder/Edit/", webOrderId);

                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailOrderFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = toEmail,
                    Cc = ccEmail,
                    Subject = ConfigurationManager.AppSettings["EmailOrderUpdatedSubject"],

                    Body = string.Format("<br /><br />The user {0} has updated their order. You may see a few of these emails, for the same customer and order, if they save the order more than once.<br /><br />You can view it <a href='{2}'>with this link</a>.", orderUser, orderTotal, serverPath)
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }
        }

        public bool SendCustomerOrderUpdatedEmail(string toEmail, string orderUser, int webOrderId, string scheme, string authority)
        {
            try
            {
                // send email here
                var serverPath = string.Format("{0}://{1}{2}{3}", scheme, authority, "/ShoppingCart/EditOrder/", webOrderId);

                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailOrderFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = toEmail,
                    Subject = ConfigurationManager.AppSettings["EmailUpdatedOrderSubject"],

                    Body = string.Format("<br />Hello {0}!<br /><br />Your order has been updated. If you have any questions, please call Country Yarns.<br /><br />You can view your order  <a href='{1}'>with this link</a>.<br /><br /><br />Thank you!<br /><br />Jeanne<br />Country Yarns<br />519-882-8740", orderUser, serverPath)
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }
        }

        public bool SendCustomerOrderShippedEmail(string toEmail, string orderUser, int webOrderId, string scheme, string authority)
        {
            try
            {
                // send email here
                var serverPath = string.Format("{0}://{1}{2}{3}", scheme, authority, "/ShoppingCart/EditOrder/", webOrderId);

                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailOrderFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = toEmail,
                    Subject = ConfigurationManager.AppSettings["EmailReadyForPaymentSubject"],

                    Body = string.Format("<br />Hello {0}!<br /><br />Your order has been shipped! If you have any questions, please call Country Yarns.<br /><br />You can view your order  <a href='{1}'>with this link</a>.<br /><br /><br />Thank you!<br /><br />Jeanne<br />Country Yarns<br />519-882-8740", orderUser, serverPath)
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }
        }

        public bool SendCustomerCanPayNowEmail(string toEmail, string orderUser, int webOrderId, string scheme, string authority)
        {
            try
            {
                // send email here
                var serverPath = string.Format("{0}://{1}{2}{3}", scheme, authority, "/ShoppingCart/EditOrder/", webOrderId);

                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailOrderFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = toEmail,
                    Subject = ConfigurationManager.AppSettings["EmailReadyForPaymentSubject"],

                    Body = string.Format("<br />Hello {0}!<br /><br />Your order is now ready for payment.<br /><br />You can view it, and arrange for payment, <a href='{1}'>with this link</a>.<br /><br /><br />Thank you!<br /><br />Jeanne<br />Country Yarns", orderUser, serverPath)
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }
        }

        public bool SendCustomerJustPaidEmail(string toEmail, string orderUser, int webOrderId, string scheme, string authority)
        {
            try
            {
                // send email here
                var serverPath = string.Format("{0}://{1}{2}{3}", scheme, authority, "/ShoppingCart/EditOrder/", webOrderId);

                var mailMsg = new SendMailMessage()
                {
                    From = ConfigurationManager.AppSettings["SystemEmailFrom"],
                    FromDisplayName = ConfigurationManager.AppSettings["SystemEmailOrderFromDisplayName"],
                    FromPassword = ConfigurationManager.AppSettings["SystemEmailPassword"],
                    To = toEmail,
                    Subject = ConfigurationManager.AppSettings["EmailCustomerJustPaid"],

                    Body = string.Format("<br />Customer : {0}<br /><br />The order was paid.<br /><br />You can view it, and arrange for shipping, <a href='{1}'>with this link</a>.<br /><br /><br /><br />Craft Store System<br />Country Yarns", orderUser, serverPath)
                };
                var result = Index(mailMsg);
                return result.Model != null;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public ViewResult Index(SendMailMessage objModelMail)
        {
            if (ModelState.IsValid)
            {
                var mail = new MailMessage();
                mail.To.Add(objModelMail.To);
                mail.From = new MailAddress(objModelMail.From, objModelMail.FromDisplayName);
                mail.Subject = objModelMail.Subject;
                mail.Body = objModelMail.Body; ;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Priority = MailPriority.Normal;
                if (!string.IsNullOrWhiteSpace(objModelMail.Cc))
                {
                    mail.CC.Add(objModelMail.Cc);
                }
                if (objModelMail.From.Contains("gmail"))
                {
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new System.Net.NetworkCredential(objModelMail.From, objModelMail.FromPassword),
                        EnableSsl = true

                    };

                    client.Send(mail);
                }
                else if (objModelMail.From.Contains("mjggallery.com"))
                {
                    var client = new SmtpClient("smtp.mjggallery.com", 587)
                    {
                        Credentials = new System.Net.NetworkCredential(objModelMail.From, objModelMail.FromPassword),
                        EnableSsl = true

                    };

                    client.Send(mail);
                }

                mail = null;
                return View("Index", objModelMail);
            }
            else
            {
                return View();
            }
        }
    }
}


