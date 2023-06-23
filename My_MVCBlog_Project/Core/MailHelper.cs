using MailKit.Net.Smtp;
using MimeKit;

namespace My_MVCBlog_Project.Core
{
    public class MailHelper
    {
        public void SendEmail(string subject,string body,params string[] mails)
        {
			try
			{
				var emailToSend = new MimeMessage();
				emailToSend.From.Add(MailboxAddress.Parse("MyMvcMailService@gmail.com"));
				foreach (var email in mails)
				{
					emailToSend.To.Add(MailboxAddress.Parse(email));
				}
				emailToSend.Subject = subject;
				emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

				//send email
				using var emailClient = new SmtpClient();
				emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
				emailClient.Authenticate("veznedaroglufayik2@gmail.com", "xpnhwuewqxxwwihh");
				emailClient.Send(emailToSend);
				emailClient.Disconnect(true);
			}
			catch (Exception ex)
			{

				throw ex;
			}
        }
    }
}
