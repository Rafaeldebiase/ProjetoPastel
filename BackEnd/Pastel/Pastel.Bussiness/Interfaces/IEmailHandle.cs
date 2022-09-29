using Pastel.Domain.Entities;
using Pastel.Domain.ValuesObject;

namespace Pastel.Handles.Interfaces
{
    public interface IEmailHandle
    {
        bool Send(string? email, string subject, string template);
        string CreatedTaskEmailTemplate(TaskModel task, FullName fullName);
        string CompletedTaskEmailTemplate(TaskModel task, FullName manager, FullName user);
    }
}
