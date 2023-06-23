using My_MVCBlog_Project.Entities;
using My_MVCBlog_Project.Models;
using My_MVCBlog_Project.Models.DTO;

namespace My_MVCBlog_Project.Services.Abstract
{
    public interface INoteService
    {
        ServiceResponse<Note> CreateNote(NoteViewModel model,HttpContext httpContext);
        ServiceResponse<List<Note>> GetAllNotes(int? userId);
        ServiceResponse<Note> Edit(int id,NoteEditViewModel model,HttpContext httpContext);
        ServiceResponse<Note> Find(int? id);
        ServiceResponse<object> Remove(int id);
        ServiceResponse<List<Note>> ListNotes();
        ServiceResponse<List<Note>> List(int? categoryId, string mode);
    }
}
