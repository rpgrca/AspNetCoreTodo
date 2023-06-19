using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using TodoApi.DTO;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace TodoApi.Services {
    public class GroupService : IGroupService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GroupService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GroupDTO> DeleteGroupAsync(long id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return _mapper.Map<GroupDTO>(group);
        }

        public async Task<GroupDTO> GetGroupAsync(long id)
        {
            Group group = await _context.Groups.FindAsync(id);
            if (group == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            GroupDTO groupDTO = _mapper.Map<GroupDTO>(group);
            return groupDTO;
        }

        public async Task<List<GroupDTO>> GetGroupsAsync()
        {
            var groups = await _context.Groups.ToListAsync();
            var groupsDTO = _mapper.Map<List<GroupDTO>>(groups);

            return groupsDTO;
        }

        public async Task<Group> PatchGroupAsync(long id, JsonPatchDocument<GroupDTO> patch)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            var groupDTO = _mapper.Map<GroupDTO>(group);
            patch.ApplyTo(groupDTO);
            CopyFromDTO(group, groupDTO);

            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<Group> PostGroupAsync(GroupDTO itemDTO)
        {
            var item = _mapper.Map<Group>(itemDTO);
            _context.Groups.Add(item);

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Group> PutGroupAsync(long id, GroupDTO itemDTO)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) {
                throw new ArgumentException("Invalid id", nameof(id));
            }

            CopyFromDTO(group, itemDTO);

            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<List<GroupDTO>> SearchGroupsAsync(string text)
        {
            List<Group> result = null;
            if (string.IsNullOrEmpty(text)) {
                result = _context.Groups.ToList();
            }
            else {
                result = await _context.Groups.Where(x => x.Name.ToLowerInvariant().Contains(text.ToLowerInvariant())).ToListAsync();
            }

            var groupsDTO = _mapper.Map<List<GroupDTO>>(result);
            return groupsDTO;
        }

        private void CopyFromDTO(Group group, GroupDTO groupDTO)
        {
            group.Name = groupDTO.Name;
        }
    }
}