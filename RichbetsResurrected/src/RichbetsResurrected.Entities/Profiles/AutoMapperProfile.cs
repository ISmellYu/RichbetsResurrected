using AutoMapper;
using RichbetsResurrected.Entities.DatabaseEntities.Identity.Models;
using RichbetsResurrected.Entities.ViewModels;
using RichbetsResurrected.Entities.ViewModels.Identity;

namespace RichbetsResurrected.Entities.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser, AppUserViewModel>();
    }
}