using CloneIntime.Models;
using CloneIntime.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Services.Interfaces
{
    public interface IAdminService
    {
        Task Login(CredentialsModel loginCredentials);
        Task Logout(HttpContext httpContext);
        Task AddTeacher(ProffessorDTO newTeacher);
        Task DeleteTeacher(string teacherId);
        Task UpdateTeacher(string teacherId);
        Task SetPair(SetTimeSlotModel newPairData);
        Task DeletePair(string pairId);
        Task UpdatePair(string id, SetTimeSlotModel PairNewData);
    }
}
