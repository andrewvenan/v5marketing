using System.ComponentModel.DataAnnotations;

namespace v5marketing.Models
{
    public class AuditoriaLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UsuarioLogin { get; set; } = "Sistema";

        [Required]
        [StringLength(100)]
        public string Acao { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Entidade { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Ip { get; set; }

        public DateTime DataHora { get; set; } = DateTime.Now;
    }
}
