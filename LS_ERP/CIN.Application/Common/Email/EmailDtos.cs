using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.Common.Email
{
  public class EmailDtos
    {
        public long EmailSendingId { get; set; }
        public string MessageBody { get; set; }
        public long TemplateID { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string BCc { get; set; }
        public DateTime AddedDate { get; set; }
        public string ScreenName { get; set; }
        public string ProcessName { get; set; }
        public int Status { get; set; }
        public DateTime SentDate { get; set; }
        public string LoggedInUser { get; set; }
        public string ConfigFromAddress { get; set; }
        public string ConfigAdminAddress { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string Password { get; set; }
        public bool SSLEnabled { get; set; }
        public string[] Attachments { get; set; }
        public string Client { get; set; }
        public string Domain { get; set; }
        public string TargetName { get; set; }
    }
}
