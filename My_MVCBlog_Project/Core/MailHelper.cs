using MailKit.Net.Smtp;
using MimeKit;
using My_MVCBlog_Project.Services.Abstract;

namespace My_MVCBlog_Project.Core
{
    public class MailHelper : IMailHelper
    {
        private readonly IUserService _userSerivce;
        public MailHelper(IUserService userService)
        {
            _userSerivce = userService; 
        }

        public void SendChangePasswordEmail(string subject, string mail)
        {

            var userDetail = _userSerivce.FindEmail(mail);

            try
            {

                string changePasswordPath = $"https://localhost:7049/Home/ChangePassword?changePasswordId={userDetail.Data.ChangePasswordId}";
                var emailToSend = new MimeMessage();
                emailToSend.From.Add(MailboxAddress.Parse("MyMvcMailService@gmail.com"));
           
                    emailToSend.To.Add(MailboxAddress.Parse(mail));
              
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = changePasswordPath };

                //send email
                using var emailClient = new SmtpClient();
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("veznedaroglufayik2@gmail.com", "wfqvpjvukemfkiam");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

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
