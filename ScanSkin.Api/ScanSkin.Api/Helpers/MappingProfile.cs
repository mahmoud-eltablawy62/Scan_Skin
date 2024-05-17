using AutoMapper;
using ScanSkin.Api.Dtos;
using ScanSkin.Core.Entites.Identity_User;

namespace ScanSkin.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Users, DoctorToReturnDto>()
                .ForMember(d => d.Price, o => o.MapFrom(S => S.Price))
                .ForMember(d => d.Speciality, o => o.MapFrom(S => S.Speciality))
                .ForMember(d => d.AddressDescription, o => o.MapFrom(S => S.AddressDescription))
                .ForMember(d => d.DisplayName, o => o.MapFrom(S => S.UserName))
                //.ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Profile_Picture))
                .ForMember(d => d.PictureUrl, s => s.MapFrom<ItemResolver>());


        }
    }
}

