using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Enums;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Transacao
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(400)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public decimal Valor { get; set; }

        [Required]
        public TipoTransacao Tipo { get; set; }

        // 🔹 RELAÇÃO COM CATEGORIA

        [Required]
        public Guid CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        [JsonIgnore] // evita ciclo
        public Categoria? Categoria { get; set; }

        // 🔹 RELAÇÃO COM PESSOA

        [Required]
        public Guid PessoaId { get; set; }

        [ForeignKey("PessoaId")]
        [JsonIgnore] // evita ciclo
        public Pessoa? Pessoa { get; set; }
    }
}
