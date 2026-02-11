using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Pessoa
    {
        // Identificador único gerado automaticamente
        public Guid Id { get; set; } = Guid.NewGuid();

        // Nome da pessoa (máximo 200 caracteres)
        [Required]
        [MaxLength(200)]
        public string Nome { get; set; } = string.Empty;

        // Idade da pessoa
        public int Idade { get; set; }

        // Lista de transações vinculadas à pessoa
        // Ignorada para evitar ciclo de serialização
        [JsonIgnore]
        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
