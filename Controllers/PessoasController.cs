using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;
using Backend.Enums;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // LISTAR PESSOAS + RESUMO GERAL
        // ===============================
        [HttpGet]
        public async Task<ActionResult<ResumoGeralDTO>> GetPessoas()
        {
            var pessoas = await _context.Pessoas
                .Include(p => p.Transacoes)
                .ToListAsync();

            var resumoPessoas = pessoas.Select(p =>
            {
                var totalReceitas = p.Transacoes?
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor) ?? 0;

                var totalDespesas = p.Transacoes?
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor) ?? 0;

                return new ResumoPessoaDTO
                {
                    PessoaId = p.Id,
                    Nome = p.Nome,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = totalReceitas - totalDespesas
                };
            }).ToList();

            var totalReceitasGeral = resumoPessoas.Sum(p => p.TotalReceitas);
            var totalDespesasGeral = resumoPessoas.Sum(p => p.TotalDespesas);

            var resultado = new ResumoGeralDTO
            {
                Pessoas = resumoPessoas,
                TotalReceitas = totalReceitasGeral,
                TotalDespesas = totalDespesasGeral,
                SaldoGeral = totalReceitasGeral - totalDespesasGeral
            };

            return Ok(resultado);
        }

        // ===============================
        // CRIAR PESSOA
        // ===============================
        [HttpPost]
        public async Task<ActionResult<Pessoa>> CreatePessoa(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPessoas), new { id = pessoa.Id }, pessoa);
        }

        // ===============================
        // EDITAR PESSOA
        // ===============================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(Guid id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
                return BadRequest();

            _context.Entry(pessoa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ===============================
        // DELETAR PESSOA (Cascade Delete)
        // ===============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(Guid id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
                return NotFound();

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
