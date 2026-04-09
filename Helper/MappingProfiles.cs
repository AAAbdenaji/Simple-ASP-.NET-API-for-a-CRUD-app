using AutoMapper;
using RentaCarAPI.Dto;
using RentaCarAPI.Models;

namespace RentaCarAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleCreateDto, Vehicle>();
            CreateMap<VehicleUpdateDto, Vehicle>();
            CreateMap<Client, ClientDto>();
            CreateMap<Client, RegisterDto>();
            CreateMap<RegisterDto, Client>();
            CreateMap<rental,rentalDto>();
            CreateMap<Client,ClientIncludeDto>();
            CreateMap<Vehicle, VehicleIncludeDto>();
            CreateMap<Location,locationDto>();
            CreateMap<LocationCreateDto, Location>();
            CreateMap<LocationUpdateDto, Location>();
            CreateMap<Depot,DepotDto>();
            CreateMap<DepotCreateDto,Depot>();    
            CreateMap<DepotUpdateDto,Depot>();
            CreateMap<DepotVehCreateDto, Depot>();
        }
    }
}
