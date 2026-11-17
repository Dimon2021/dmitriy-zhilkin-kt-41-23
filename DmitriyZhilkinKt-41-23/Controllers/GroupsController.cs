using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.GroupFilters;
using DmitriyZhilkinKt_41_23.Interfaces.GroupInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DmitriyZhilkinKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("GetGroups")]
        public async Task<IActionResult> GetGroupsAsync(GroupFilter filter, CancellationToken cancellationToken = default)
        {
            var groups = await _groupService.GetGroupsAsync(filter, cancellationToken);
            return Ok(groups);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddGroupAsync(GroupRequest request, CancellationToken cancellationToken = default)
        {
            var groupId = await _groupService.AddGroupAsync(request, cancellationToken);
            return Ok(groupId);
        }

        [HttpPut("Update/{groupId:int}")]
        public async Task<IActionResult> UpdateGroupAsync(int groupId, GroupRequest request, CancellationToken cancellationToken = default)
        {
            await _groupService.UpdateGroupAsync(groupId, request, cancellationToken);
            return Ok();
        }

        [HttpDelete("Delete/{groupId:int}")]
        public async Task<IActionResult> DeleteGroupAsync(int groupId, CancellationToken cancellationToken = default)
        {
            await _groupService.DeleteGroupAsync(groupId, cancellationToken);
            return Ok();
        }
    }
}
