using Microsoft.AspNetCore.Mvc;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using CloneIntime.Services;

namespace CloneIntime.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet("{faluctyId}")]
        public async Task<List<GroupDTO>> GetGroups(string faluctyId)
        {
            return await _groupService.GetGroups(faluctyId);
        }

    }

}
