using My_MVCBlog_Project.Context;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Services.Abstract;

namespace My_MVCBlog_Project.Services.Concrete
{
    public class EBulletinService : IEBulletinService
    {
        private readonly ApplicationDbContext _context;
        public EBulletinService(ApplicationDbContext context)
        {
            _context = context; 
        }

        public ServiceResponse<object> Create(string email)
        {
            ServiceResponse<object> result = new ServiceResponse<object>();
            email = email.Trim().ToLower();
            if (_context.EBulletins.Any(x=>x.Email.ToLower() == email.ToLower()))
            {
                result.AddError($"{email} adresi zaten kayıtlıdır");
                return result;
            }
            EBulletin ebulletin = new()
            {
                Email = email,
                Banned = false,
                CreatedDate = DateTime.Now,
            };
            _context.EBulletins.Add(ebulletin);
            if (_context.SaveChanges() == 0)
            {
                result.AddError("Kayıt yapılamadı");
            }
            return result;
        }

        public ServiceResponse<List<EBulletin>> ListEBulletin()
        {
            ServiceResponse<List<EBulletin>> response = new ServiceResponse<List<EBulletin>>();
            var result = _context.EBulletins.ToList();
            if (result != null)
            {
                response.Data = result;
                return response;
            }
            return response;
        }

        public ServiceResponse<List<EBulletin>> ListExceptBanned()
        {
            List<EBulletin> ebulletins = _context.EBulletins.Where(x => x.Banned == false).ToList();
            ServiceResponse<List<EBulletin>> result = new ServiceResponse<List<EBulletin>>();
            result.Data = ebulletins;
            return result;
        }
    }
}
