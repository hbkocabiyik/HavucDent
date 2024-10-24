using AutoMapper;
using HavucDent.Domain.Entities;
using HavucDent.Web.Models;

namespace HavucDent.Application.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Laboratory için mapping
			CreateMap<LaboratoryViewModel, Laboratory>();


		}
	}
}