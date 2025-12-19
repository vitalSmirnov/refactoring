using CloneIntime.Models.DTO;

namespace CloneIntime.Services.Interfaces
{
    public interface IGroupService
    {
        Task<List<GroupDTO>> GetGroups(string facultyId);
    }
}
