using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoDepartamento.Api.Data;
using ProjetoDepartamento.Api.DTOs;
using ProjetoDepartamento.Api.Models;

namespace ProjetoDepartamento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
       private readonly AppDbContext _context;

       public FuncionariosController(AppDbContext context)
       {
          _context = context;
       }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioDepartamentoDto>>>
        GetFuncionarios()
        {
            return await _context.Funcionarios.Include(f => f.Departamento).Select(f => new FuncionarioDepartamentoDto
            { NomeFuncionario = f.Nome, NomeDepartamento = f.Departamento.Nome }).ToListAsync();
        }
        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<FuncionarioInfoDto>>> 
        GetInfoFuncionarios()
        {
            return await _context.Funcionarios.Include(f => f.Departamento).Select(f => new FuncionarioInfoDto
            {
                Nome = f.Nome,
                Salario = f.Salario,
                NomeDepartamento = f.Departamento.Nome,
                DepartamentoId = f.DepartamentoId

            })
            .ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> CriarFuncionario(CriarFuncionarioDTO funcionarioDTo)
        {
            var newfuncionario = new Funcionario
            {
                Cpf = funcionarioDTo.Cpf,
                Nome = funcionarioDTo.Nome,
                Salario = funcionarioDTo.Salario,
                DepartamentoId = funcionarioDTo.DepartamentoId,
            };

            _context.Funcionarios.Add(newfuncionario);
            await _context.SaveChangesAsync();
            return Ok("Funcionario criado com sucesso!");
        }
    }
}
