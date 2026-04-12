using System.ComponentModel.DataAnnotations;

namespace v5marketing.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O login é obrigatório")]
        [Display(Name = "Login")]
        [StringLength(50, ErrorMessage = "O login deve ter no máximo 50 caracteres")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [Display(Name = "Hash da Senha")]
        public string SenhaHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "O perfil é obrigatório")]
        [Display(Name = "Perfil")]
        [StringLength(30, ErrorMessage = "O perfil deve ter no máximo 30 caracteres")]
        public string Perfil { get; set; } = "Administrador";

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}