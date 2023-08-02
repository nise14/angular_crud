using BackEnd.Models;

namespace BackEnd.Services.Contrato;

public interface IDepartamentoService
{
    Task<List<Departamento>> GetList();
}