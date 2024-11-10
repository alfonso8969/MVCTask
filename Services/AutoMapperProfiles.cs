using AutoMapper;
using MVCTask.Models;

namespace MVCTask.Services {

    public class AutoMapperProfiles: Profile {
        public AutoMapperProfiles() {
            CreateMap<Entities.Task, TaskDTO>();
        }
    }
}

