using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using CIN.Application.Common.Email;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;



namespace CIN.Application.Common.Email
{
  public class SentEmailNotification 
    {
        public static void Send(EmailDtos email,IConfiguration _Config)
        {

            //email.ConfigFromAddress = Convert.ToString(_Config["AppSettings:EmailAddress"]);
            //email.SMTPPort = Convert.ToInt32(_Config["AppSettings:SMTPPort"]);
            //email.SMTPServer = Convert.ToString(_Config["AppSettings:SMTPServer"]);
            //email.SSLEnabled = Convert.ToBoolean(_Config["AppSettings:SSLEnabled"]);
            //email.Password = Convert.ToString(_Config["AppSettings:Password"]);
            //email.Client = Convert.ToString(_Config["AppSettings:ClientName"]);
            //email.Domain = Convert.ToString(_Config["AppSettings:SMTPClientDomain"]);
            //email.TargetName = Convert.ToString(_Config["AppSettings:TargetName"]);

            //try
            //{
            //    if (!string.IsNullOrEmpty(email.To) || !string.IsNullOrEmpty(email.Cc) || !string.IsNullOrEmpty(email.BCc))
            //    {
            //        System.Net.Mail.MailMessage newmsg = new System.Net.Mail.MailMessage();

            //        MailAddress mailfrom = new MailAddress(email.ConfigFromAddress);
            //        if (!string.IsNullOrWhiteSpace(email.To))
            //        {
            //            email.To = email.To.Replace("<br>", ",");
            //            if (email.To.EndsWith(","))
            //                email.To = email.To.Remove(email.To.Length - 1);
            //            string[] _toAddress = email.To.Split(',');
            //            for (int i = 0; i < _toAddress.Length; i++)
            //            {
            //                newmsg.To.Add(new MailAddress(_toAddress[i]));
            //            }
            //        }
            //        if (!string.IsNullOrWhiteSpace(email.Cc))
            //        {
            //            MailAddress Cc = new MailAddress(email.Cc);
            //            newmsg.CC.Add(Cc);
            //        }
            //        if (!string.IsNullOrWhiteSpace(email.BCc))
            //        {
            //            string[] bccID = email.BCc.Split(',');
            //            foreach (string bcmail in bccID)
            //            {
            //                if (bcmail != "")
            //                {
            //                    MailAddress bcc = new MailAddress(bcmail);
            //                    newmsg.Bcc.Add(bcc);
            //                }
            //            }
            //        }
            //        if (email.Attachments != null && email.Attachments.Count() > 0)
            //        {
            //            foreach (string file in email.Attachments)
            //            {
            //                Attachment data = new Attachment(file);
            //                newmsg.Attachments.Add(data);
            //            }
            //        }

            //        newmsg.From = mailfrom;
            //        newmsg.Subject = email.Subject;
            //        newmsg.Body = email.MessageBody;
            //        newmsg.IsBodyHtml = false;
            //        SmtpClient smtp = new SmtpClient(email.SMTPServer, email.SMTPPort);
            //        smtp.UseDefaultCredentials = false;
            //        //if (email.Client.Equals("sahir"))
            //        //{
            //        //    smtp.Credentials = new System.Net.NetworkCredential(email.ConfigFromAddress, email.Password, email.Domain);
            //        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //        //    smtp.TargetName = email.TargetName;
            //        //    smtp.EnableSsl = email.SSLEnabled;
            //        //    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //        //}
            //        //else
            //        //{
            //            smtp.Credentials = new System.Net.NetworkCredential(email.ConfigFromAddress, email.Password);
            //            smtp.EnableSsl = email.SSLEnabled;
            //        //}
            //        smtp.Send(newmsg);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Error("Error in Send Method");
            //    Log.Error("Error occured time : " + DateTime.UtcNow);
            //    Log.Error("Error message : " + ex.Message);
            //    Log.Error("Error StackTrace : " + ex.StackTrace);


            //    throw;
            //}

            // this message is belongs to Send Email Through arasmotec 
            //try { 
            //MailMessage message = new MailMessage();
            //message.From = new MailAddress("mohsin@arasmotech.com");
            //message.To.Add("mohsin@arasmotech.com");
            //message.Subject = "Subject of the email Test";
            //message.Body = "Content of the email";

            //// Create a new instance of the SmtpClient class
            //SmtpClient smtpClient = new SmtpClient("webcloud4.uk.syrahost.com",465);
            //smtpClient.EnableSsl = true;
            //smtpClient.Credentials = new NetworkCredential("mohsin@arasmotech.com", "Mohsin1@3");

            //// Send the email
            //smtpClient.Send(message);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error("Error in Send Method");
            //    Log.Error("Error occured time : " + DateTime.UtcNow);
            //    Log.Error("Error message : " + ex.Message);
            //    Log.Error("Error StackTrace : " + ex.StackTrace);
            //}

  
         
                // Gmail SMTP server settings
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;
                string senderEmail = "alilogicsystems@gmail.com";
                string senderPassword = "iyfhgsragpbawxid";

                // Recipient email address
                string recipientEmail = "ali@logicsystems-me.com";

                // Create a new instance of MailMessage
                MailMessage message = new MailMessage(senderEmail, senderEmail);
             // MailMessage message1 = new MailMessage(senderEmail, recipientEmail);

                // Set the subject and body of the email
                message.Subject = "Hello Gmail from logicsystems";
                message.Body = "This is a test email sent from Gmail.";
                 MailAddress bcc = new MailAddress(recipientEmail);
                message.Bcc.Add(bcc);
            // Create an instance of SmtpClient
            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);

                // Enable SSL/TLS encryption
                smtpClient.EnableSsl = true;

                // Set the credentials for the Gmail account
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                try
                {
                smtpClient.Timeout = 50000;
                // Send the email
                smtpClient.Send(message);
                   
                    //Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                }

                //// Dispose of the SmtpClient and MailMessage objects
                //smtpClient.Dispose();
                //message.Dispose();

                //Console.ReadLine();
            }
        

    }
}

