namespace My_MVCBlog_Project.Core
{
    public interface IMailHelper
    {
        public void SendEmail(string subject, string body, params string[] mails);
        public void SendChangePasswordEmail(string subject,  string mail);
    }
}
