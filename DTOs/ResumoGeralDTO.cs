namespace Backend.DTOs
{
    public class ResumoGeralDTO
    {
        public IEnumerable<ResumoPessoaDTO> Pessoas { get; set; } = new List<ResumoPessoaDTO>();

        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal SaldoGeral { get; set; }
    }
}
