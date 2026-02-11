namespace Backend.DTOs
{
    public class TransacaoDTO
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; }

        public string NomePessoa { get; set; }
        public string CategoriaDescricao { get; set; }
    }
}
