using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Enums;
using Backend.DTOs;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR todas (DTO)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransacaoDTO>>> GetTransacoes()
        {
            var transacoes = await _context.Transacoes
                .Include(t => t.Pessoa)
                .Include(t => t.Categoria)
                .Select(t => new TransacaoDTO
                {
                    Id = t.Id,
                    Descricao = t.Descricao,
                    Valor = t.Valor,
                    Tipo = t.Tipo.ToString(),
                    NomePessoa = t.Pessoa.Nome,
                    CategoriaDescricao = t.Categoria.Descricao
                })
                .ToListAsync();

            return Ok(transacoes);
        }

        // BUSCAR por Id
        [HttpGet("{id}")]
        public async Task<ActionResult<TransacaoDTO>> GetTransacao(Guid id)
        {
            var transacao = await _context.Transacoes
                .Include(t => t.Pessoa)
                .Include(t => t.Categoria)
                .Where(t => t.Id == id)
                .Select(t => new TransacaoDTO
                {
                    Id = t.Id,
                    Descricao = t.Descricao,
                    Valor = t.Valor,
                    Tipo = t.Tipo.ToString(),
                    NomePessoa = t.Pessoa.Nome,
                    CategoriaDescricao = t.Categoria.Descricao
                })
                .FirstOrDefaultAsync();

            if (transacao == null)
                return NotFound("Transação não encontrada.");

            return Ok(transacao);
        }

        // CRIAR usando DTO
        [HttpPost]
        public async Task<ActionResult<Transacao>> CreateTransacao(CreateTransacaoDTO dto)
        {
            var pessoa = await _context.Pessoas.FindAsync(dto.PessoaId);
            if (pessoa == null)
                return BadRequest("PessoaId inválido. Pessoa não encontrada.");

            var categoria = await _context.Categorias.FindAsync(dto.CategoriaId);
            if (categoria == null)
                return BadRequest("CategoriaId inválido. Categoria não encontrada.");

            if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
                return BadRequest("Pessoa menor de 18 anos só pode registrar despesas.");

            if (dto.Tipo == TipoTransacao.Despesa &&
                categoria.Finalidade == FinalidadeCategoria.Receita)
                return BadRequest("Categoria incompatível: transação de despesa não pode usar categoria de receita.");

            if (dto.Tipo == TipoTransacao.Receita &&
                categoria.Finalidade == FinalidadeCategoria.Despesa)
                return BadRequest("Categoria incompatível: transação de receita não pode usar categoria de despesa.");

            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                Tipo = dto.Tipo,
                CategoriaId = dto.CategoriaId,
                PessoaId = dto.PessoaId
            };

            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransacao), new { id = transacao.Id }, transacao);
        }

        // DELETAR
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransacao(Guid id)
        {
            var transacao = await _context.Transacoes.FindAsync(id);

            if (transacao == null)
                return NotFound("Transação não encontrada.");

            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
