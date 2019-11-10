using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using TodoApi.DTO;
using TodoApi.Services;
using TodoApi.Mappings;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;

namespace TodoApi.Controllers {
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService) {
            _groupService = groupService;
        }

        [Authorize]
        [HttpGet("{id:regex(^[[0-9]]+$)}")]
        public async Task<ActionResult<GroupDTO>> GetGroup(long id) {
            try {
                GroupDTO groupDTO = await _groupService.GetGroupAsync(id);
                return Ok(groupDTO);
            }
            catch (ArgumentException) {
                return NotFound();
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups() {
            var list = await _groupService.GetGroupsAsync();
            return Ok(list);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Group>> PostGroup(GroupDTO itemDTO) {
            var item = await _groupService.PostGroupAsync(itemDTO);
            return CreatedAtAction(nameof(GetGroup), new { id = item.Id}, item);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Group>> PutGroup(long id, GroupDTO itemDTO) {
            try {
                Group group = await _groupService.PutGroupAsync(id, itemDTO);
            }
            catch (ArgumentException) {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<Group>> PatchGroup(long id, JsonPatchDocument<GroupDTO> patch) {
            try {
                Group group = await _groupService.PatchGroupAsync(id, patch);
            }
            catch (ArgumentException) {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupDTO>> DeleteGroup(long id) {
            try {
                 GroupDTO group = await _groupService.DeleteGroupAsync(id);
            }
            catch (ArgumentException) {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("search/{searchString}")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> SearchGroups(string searchString) {
            List<GroupDTO> result = await _groupService.SearchGroupsAsync(searchString);
            return Ok(result);
        }
    }
}