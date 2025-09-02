using AutoMapper;
using Flow.Tasks.Api.Domain;
using Flow.Tasks.Api.DTOs;

namespace Flow.Tasks.Api.Mappings
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskRequest, TaskEntity>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status ?? Domain.TaskStatus.Todo));

            CreateMap<TaskEntity, TaskResponse>()
                .ForMember(dest => dest.RowVersion,
                    opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));
        }
    }
}
