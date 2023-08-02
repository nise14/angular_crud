using BackEnd.Models;
using BackEnd.Services.Contrato;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Implementacion;

public class EmpleadoService : IEmpleadoService
{
    private readonly DbempleadoContext _dbContext;

    public EmpleadoService(DbempleadoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Empleado> Add(Empleado modelo)
    {
        try
        {
            _dbContext.Empleados.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> Delete(Empleado modelo)
    {
        try
        {
            _dbContext.Empleados.Remove(modelo);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<Empleado> Get(int idEmpleado)
    {
        try
        {
            Empleado? empleado = new Empleado();

            empleado = await _dbContext.Empleados.Include(dpt => dpt.IdDepartamentoNavigation)
                .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);

            return empleado ?? new Empleado();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<List<Empleado>> GetList()
    {
        try
        {
            return await _dbContext.Empleados.Include(dpt => dpt.IdDepartamentoNavigation).ToListAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> Update(Empleado modelo)
    {
        try
        {
            _dbContext.Empleados.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}