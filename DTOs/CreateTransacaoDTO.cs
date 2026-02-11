using Backend.Enums;

namespace Backend.DTOs
{
    public class CreateTransacaoDTO
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }

        public Guid CategoriaId { get; set; }
        public Guid PessoaId { get; set; }
    }
}
