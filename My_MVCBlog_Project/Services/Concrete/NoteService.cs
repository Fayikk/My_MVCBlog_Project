using Microsoft.EntityFrameworkCore;
using My_MVCBlog_Project.Context;
using My_MVCBlog_Project.Core;
using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Models.DTO;
using My_MVCBlog_Project.Services.Abstract;

namespace My_MVCBlog_Project.Services.Concrete
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        public NoteService(ApplicationDbContext context)
        {
            _context = context; 
        }

        public ServiceResponse<Note> CreateNote(NoteViewModel model, HttpContext httpContext)
        {
            ServiceResponse<Note> result = new ServiceResponse<Note>();
            Note note = new()
            {
                CategoryId = model.CategoryId,
                Title = model.Title,
                Summary = model.Summary,
                Detail = model.Detail,
                OwnerId = httpContext.Session.GetInt32(Constants.UserId).Value,
                CreatedDate = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.Username),
                ModifiedDate = DateTime.Now,
                ModifiedUser = httpContext.Session.GetString(Constants.Username)
            };
            _context.Notes.Add(note);
            if (_context.SaveChanges()==0)
            {
                result.AddError("Not eklenirken bir hata oluştu");
            }
            return result;  
        }

        public ServiceResponse<Note> Edit(int id, NoteEditViewModel model, HttpContext httpContext)
        {
            ServiceResponse<Note> result = new ServiceResponse<Note>();
            var note = _context.Notes.Find(id);
            note.CategoryId = model.CategoryId;
                note.Title = model.Title;
            note.Summary = model.Summary;
            note.Detail = model.Detail;
            note.OwnerId = httpContext.Session.GetInt32(Constants.UserId).Value;
            note.CreatedDate = DateTime.Now;
            note.CreatedUser = httpContext.Session.GetString(Constants.Username);
            note.ModifiedDate = DateTime.Now;
            note.ModifiedUser = httpContext.Session.GetString(Constants.Username);

            if (_context.SaveChanges() == 0)
            {
                result.AddError("Not güncellenirken bir hata oluştu");
            }
            return result;
        }

        public ServiceResponse<Note> Find(int? id)
        {
            ServiceResponse<Note> result = new ServiceResponse<Note>
            {
                Data = _context.Notes.Include(x => x.Comments).Include(x => x.Category).Include(x => x.Owner).SingleOrDefault(x => x.Id == id)

            };
            if (result.Data != null)
            {
                result.AddError("Kayıt Bulunamadı");
            }
            return result;
        }

        public ServiceResponse<List<Note>> GetAllNotes(int? userId)
        {
            IQueryable<Note> notes;
            notes = _context.Notes.
                    Include(x => x.Category)
                    .Include(x => x.Comments)
                    .Include(x => x.Owner)
                    .AsQueryable();
            if (userId != null)
            {
                notes = notes.Where(x => x.OwnerId == userId);
            }
            ServiceResponse<List<Note>> result = new ServiceResponse<List<Note>>();
            result.Data = notes.ToList();
            return result;
        }

        public ServiceResponse<List<Note>> List(int? categoryId, string mode)
        {
            IQueryable<Note> notes =
                _context.Notes.Include(x => x.Category).Include(x => x.Comments).Include(x => x.Owner).AsQueryable();
            if (categoryId != null)
            {
                notes = notes.Where(x => x.CategoryId == categoryId);
            }
            List<Note> list = notes.AsNoTracking().ToList();
            list.ForEach(x => x.ModifiedDate = (x.ModifiedDate == null ? x.CreatedDate : x.ModifiedDate));
            if (string.IsNullOrEmpty(mode) == false)
            {
                switch (mode)
                {
                    case "last":
                        list = list.OrderByDescending(x => x.CreatedDate).ToList();
                        break;
                    default:
                        break;
                }
            }
            ServiceResponse<List<Note>> result = new ServiceResponse<List<Note>>();
            result.Data = notes.ToList();
            return result;
        }

        public ServiceResponse<List<Note>> ListNotes()
        {
            ServiceResponse<List<Note>> result = new ServiceResponse<List<Note>>();
            var notes = _context.Notes.ToList();
            result.Data = notes;
            return result;
        }

        public ServiceResponse<object> Remove(int id)
        {
            ServiceResponse<object> result = new ServiceResponse<object>();
            var note = _context.Notes.Find(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                if (_context.SaveChanges() == 0)
                {
                    result.AddError("Not silinirken bir hata oluştu");
                }
                else
                {
                    result.Data = note;
                }
            }
            return result;
        }
    }
}
