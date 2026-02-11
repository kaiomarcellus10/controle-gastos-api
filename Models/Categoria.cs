using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Enums;

namespace Backend.Models
{
    public class Categoria
    {
        // Identificador único gerado automaticamente
        public Guid Id { get; set; } = Guid.NewGuid();

        // Descrição da categoria (máximo 400 caracteres)
        [Required]
        [MaxLength(400)]
        public string Descricao { get; set; } = string.Empty;

        // Define se é despesa, receita ou ambas
        [Required]
        public FinalidadeCategoria Finalidade { get; set; }

        // Lista de transações vinculadas à categoria
        // Ignorada para evitar ciclo de serialização
        [JsonIgnore]
        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
