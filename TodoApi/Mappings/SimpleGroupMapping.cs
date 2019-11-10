using AutoMapper;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Mappings {
    public class SimpleGroupMapping : Profile {
        public SimpleGroupMapping() {
            CreateMap<GroupDTO, Group>();
            CreateMap<Group, GroupDTO>();
        }
    }
}