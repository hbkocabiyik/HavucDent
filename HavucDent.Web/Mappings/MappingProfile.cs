using AutoMapper;
using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Identity;
using HavucDent.Web.Models;

namespace HavucDent.Application.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<LaboratoryViewModel, Laboratory>().ReverseMap();
            CreateMap<User, AppUser>().ReverseMap();
        }
	}
}