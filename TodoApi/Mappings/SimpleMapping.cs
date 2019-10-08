using AutoMapper;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Mappings {
    public class SimpleMapping : Profile {
        public SimpleMapping() {
            CreateMap<TodoItemDTO, TodoItem>();
            CreateMap<TodoItem, TodoItemDTO>();
        }
    }
}