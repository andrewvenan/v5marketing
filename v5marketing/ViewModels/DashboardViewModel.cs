using v5marketing.Models;

namespace v5marketing.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalClientes { get; set; }
        public int TotalCidades { get; set; }
        public int TotalComEmail { get; set; }
        public int TotalComTelefone { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalUsuariosAtivos { get; set; }

        public List<Cliente> UltimosClientes { get; set; } = new();
        public List<ClientesPorCidadeViewModel> ClientesPorCidade { get; set; } = new();
        public List<UsuariosPorPerfilViewModel> UsuariosPorPerfil { get; set; } = new();
    }

    public class ClientesPorCidadeViewModel
    {
        public string Cidade { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }

    public class UsuariosPorPerfilViewModel
    {
        public string Perfil { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}