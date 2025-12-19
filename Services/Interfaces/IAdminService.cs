using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Models.ModelTypes;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ActionResult<TokenModel>> Login(CredentialsModel model);
        Task<IActionResult> Logout(string userToken);
        Task<IActionResult> AddTeacher(ProffessorDTO newTeacher);
        Task<IActionResult> DeleteTeacher(string teacherId);
        Task<IActionResult> UpdateTeacher(string teacherId, ProffessorDTO newTeacher);
        Task<IActionResult> SetPair(SetTimeSlotModel newPairData);
        Task<IActionResult> DeletePair(string pairId);
        Task<IActionResult> UpdatePair(string id, SetTimeSlotModel PairNewData);
    }
}
