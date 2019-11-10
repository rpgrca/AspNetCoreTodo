using AutoMapper;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Mappings {
    public class SimpleTodoItemMapping : Profile {
        public SimpleTodoItemMapping() {
            CreateMap<TodoItemDTO, TodoItem>();
            CreateMap<TodoItem, TodoItemDTO>();
        }
    }
}