using AutoMapper;
using BackEnd.DTOs;
using BackEnd.Models;
using BackEnd.Services.Contrato;
using BackEnd.Services.Implementacion;
using BackEnd.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbempleadoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});

builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/departamento/lista", async (IDepartamentoService _departamentoServicio,
    IMapper _mapper) =>
{
    var listaDepartamento = await _departamentoServicio.GetList();
    var listaDepartamentoDto = _mapper.Map<List<DepartamentoDTO>>(listaDepartamento);

    if (listaDepartamentoDto.Any())
    {
        return Results.Ok(listaDepartamentoDto);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapGet("/empleado/lista", async (IEmpleadoService _empleadoService,
    IMapper _mapper) =>
{
    var listaEmpleado = await _empleadoService.GetList();
    var listaEmpleadoDTO = _mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

    if (listaEmpleadoDTO.Any())
    {
        return Results.Ok(listaEmpleadoDTO);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("empleado/guardar", async (EmpleadoDTO modelo, IEmpleadoService _empleadoService, IMapper _mapper) =>
{
    var empleado = _mapper.Map<Empleado>(modelo);
    var empleadoCreado = await _empleadoService.Add(empleado);

    if (empleadoCreado.IdEmpleado != default(int))
    {
        return Results.Ok(_mapper.Map<EmpleadoDTO>(empleadoCreado));
    }

    return Results.StatusCode(StatusCodes.Status500InternalServerError);
});


app.MapPut("empleado/actualizar/{idEmpleado}", async (int idEmpleado,
    EmpleadoDTO modelo,
    IEmpleadoService _empleadoService,
    IMapper _mapper) =>
{
    var encontrado = await _empleadoService.Get(idEmpleado);

    if (encontrado is null)
    {
        return Results.NotFound();
    }

    var empleado = _mapper.Map<Empleado>(modelo);

    encontrado.NombreCompleto = empleado.NombreCompleto;
    encontrado.IdDepartamento = empleado.IdDepartamento;
    encontrado.Sueldo = empleado.Sueldo;
    encontrado.FechaContrato = empleado.FechaContrato;

    var respuesta = await _empleadoService.Update(encontrado);

    if (respuesta)
    {
        return Results.Ok(_mapper.Map<EmpleadoDTO>(encontrado));
    }

    return Results.StatusCode(StatusCodes.Status500InternalServerError);
});

app.MapDelete("empleado/eliminar/{idEmpleado}", async (int idEmpleado,
    IEmpleadoService _empleadoService,
    IMapper mapper) =>
{
    var encontrado = await _empleadoService.Get(idEmpleado);

    if (encontrado is null)
    {
        return Results.NotFound();
    }

    var respuesta = await _empleadoService.Delete(encontrado);

    if (respuesta)
    {
        return Results.Ok();
    }

    return Results.StatusCode(StatusCodes.Status500InternalServerError);
});

app.UseCors("NuevaPolitica");

app.Run();