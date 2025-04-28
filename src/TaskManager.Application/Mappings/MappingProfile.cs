using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskItem, TaskDTO>()
                .ForMember(dest => dest.Comments, opt => opt.Ignore());

            CreateMap<TaskComment, TaskCommentDTO>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore());

            CreateMap<TaskHistoryEntry, TaskHistoryDTO>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore());
        }
    }
}
