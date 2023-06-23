using My_MVCBlog_Project.Entities;

namespace My_MVCBlog_Project.Services.Abstract
{
    public interface IEBulletinService
    {
        ServiceResponse<object> Create(string email);
        ServiceResponse<List<EBulletin>> ListExceptBanned();
        public ServiceResponse<List<EBulletin>> ListEBulletin();
    }
}
