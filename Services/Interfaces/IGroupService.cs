using CloneIntime.Entities;
using CloneIntime.Models.DTO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.RegularExpressions;

namespace CloneIntime.Services.Interfaces
{
    public interface IGroupService
    {
        Task<List<GroupDTO>> GetGroups(string facultyId);
    }
}
