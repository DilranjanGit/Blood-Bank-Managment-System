using AutoMapper;
using BloodBank.Api.DTOs;
using BloodBank.Api.Models;

namespace BloodBank.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Donor, DonorDto>();
        CreateMap<CreateDonorDto, Donor>();
    }
}