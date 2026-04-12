using System.ComponentModel.DataAnnotations;

namespace v5marketing.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [Display(Name = "CNPJ")]
        [StringLength(18)]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "A razão social é obrigatória")]
        [Display(Name = "Razão Social")]
        [StringLength(150)]
        public string RazaoSocial { get; set; }

        [Display(Name = "Nome Fantasia")]
        [StringLength(150)]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "O nome do contato é obrigatório")]
        [Display(Name = "Nome do Contato")]
        [StringLength(100)]
        public string NomeContato { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Display(Name = "Telefone")]
        [StringLength(15)]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [Display(Name = "CEP")]
        [StringLength(9)]
        public string Cep { get; set; }

        [Display(Name = "Rua")]
        [StringLength(150)]
        public string Rua { get; set; }

        [Display(Name = "Número")]
        [StringLength(10)]
        public string Numero { get; set; }

        [Display(Name = "Bairro")]
        [StringLength(100)]
        public string Bairro { get; set; }

        [Display(Name = "Cidade")]
        [StringLength(100)]
        public string Cidade { get; set; }

        [Display(Name = "Complemento")]
        [StringLength(150)]
        public string Complemento { get; set; }

        [Display(Name = "Observação")]
        [StringLength(500)]
        public string Observacao { get; set; }
    }
}
