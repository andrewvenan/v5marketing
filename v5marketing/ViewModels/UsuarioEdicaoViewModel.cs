using System.ComponentModel.DataAnnotations;

namespace v5marketing.ViewModels
{
    public class UsuarioEdicaoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome")]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O login é obrigatório")]
        [Display(Name = "Login")]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Display(Name = "Nova Senha")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A nova senha deve ter no mínimo 6 caracteres")]
        public string? NovaSenha { get; set; }

        [Display(Name = "Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem")]
        public string? ConfirmarNovaSenha { get; set; }

        [Required(ErrorMessage = "O perfil é obrigatório")]
        [Display(Name = "Perfil")]
        public string Perfil { get; set; } = "Administrador";

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}