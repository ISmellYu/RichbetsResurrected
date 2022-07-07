using AutoMapper;
using RichbetsResurrected.Entities.Identity.Models;
using RichbetsResurrected.Entities.ViewModels;

namespace RichbetsResurrected.Entities.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser, AppUserViewModel>();
    }
}