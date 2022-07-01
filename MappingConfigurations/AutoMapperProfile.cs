using AutoMapper;
using APIDBLayer.Entities;
using WebAPI_2.ApiModel;

namespace WebAPI_2.MappingConfigurations
{
    public class AutoMapperProfile : Profile
    {
       public  AutoMapperProfile()
        {
            CreateMap<Teacher, TeacherApiModel>().ReverseMap();
            CreateMap<ClassRoom, ClassRoomApiModel>().ReverseMap();


        }
    }
}
