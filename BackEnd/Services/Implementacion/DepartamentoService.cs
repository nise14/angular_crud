using BackEnd.Models;
using BackEnd.Services.Contrato;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Implementacion;

public class DepartamentoService : IDepartamentoService
{
    private readonly DbempleadoContext _dbContext;

    public DepartamentoService(DbempleadoContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Departamento>> GetList()
    {
        try
        {
            List<Departamento> list = new List<Departamento>();
            list = await _dbContext.Departamentos.ToListAsync();
            return list;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}