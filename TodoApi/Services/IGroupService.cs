using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.DTO;
using Microsoft.AspNetCore.JsonPatch;

namespace TodoApi.Services {
    public interface IGroupService {
        Task<GroupDTO> GetGroupAsync(long id);
        Task<List<GroupDTO>> GetGroupsAsync();
        Task<Group> PutGroupAsync(long id, GroupDTO itemDTO);
        Task<Group> PostGroupAsync(GroupDTO itemDTO);
        Task<Group> PatchGroupAsync(long id, JsonPatchDocument<GroupDTO> patch);
        Task<GroupDTO> DeleteGroupAsync(long id);
        Task<List<GroupDTO>> SearchGroupsAsync(string text);
    }
}