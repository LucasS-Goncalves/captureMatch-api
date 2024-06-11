using AutoMapper;
using CaptureMatchApi.DTOs;
using CaptureMatchApi.Entities;

namespace CaptureMatchApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<User, MemberDto>()
                .ForMember(dest => dest.ProfilePhotoUrl, options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsProfilePicture).Url));
            CreateMap<Photo, PhotoDto>();
        }
    }
}
