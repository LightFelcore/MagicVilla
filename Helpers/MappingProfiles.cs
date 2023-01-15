using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dtos;

namespace MagicVilla.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Villa, VillaOutputDto>();
            CreateMap<VillaInputDto, Villa>();
        }
    }
}