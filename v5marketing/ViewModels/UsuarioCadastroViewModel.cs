using System.ComponentModel.DataAnnotations;

namespace v5marketing.ViewModels
{
    public class UsuarioCadastroViewModel
    {
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

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação da senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem")]
        public string ConfirmarSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O perfil é obrigatório")]
        [Display(Name = "Perfil")]
        public string Perfil { get; set; } = "Administrador";

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}
