using System.Globalization;
using AutoMapper;
using BackEnd.DTOs;
using BackEnd.Models;

namespace BackEnd.Utilidades;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
        CreateMap<Empleado, EmpleadoDTO>()
            .ForMember(destino => destino.NombreDepartamento,
                opt => opt.MapFrom(origen => origen.IdDepartamentoNavigation.Nombre))
            .ForMember(destino => destino.FechaContrato,
                opt => opt.MapFrom(origen => origen.FechaContrato.GetValueOrDefault().ToString("dd/MM/yyyy")));

        CreateMap<EmpleadoDTO, Empleado>()
            .ForMember(destino => destino.IdDepartamentoNavigation,
                opt => opt.Ignore())
            .ForMember(destino => destino.FechaContrato,
                opt => opt.MapFrom(origen => DateTime.ParseExact(origen.FechaContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
    }
}