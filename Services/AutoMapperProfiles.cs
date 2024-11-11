using AutoMapper;
using MVCTask.Models;

namespace MVCTask.Services {

    public class AutoMapperProfiles: Profile {
        public AutoMapperProfiles() {
            CreateMap<Entities.Task, TaskDTO>()
                .ForMember(dto => dto.TotalSteps, static ent => ent.MapFrom(t => t.Steps.Count))
                .ForMember(dto => dto.CompletedSteps, static ent => ent.MapFrom(t => t.Steps.Count(s => s.IsCompleted)));
        }
    }
}

