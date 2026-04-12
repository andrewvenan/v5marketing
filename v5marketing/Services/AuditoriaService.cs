using v5marketing.Data;
using v5marketing.Models;

namespace v5marketing.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditoriaService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task RegistrarAsync(string acao, string entidade, string descricao)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var usuarioLogin = httpContext?.Session.GetString("UsuarioLogado") ?? "Sistema";
            var ip = httpContext?.Connection?.RemoteIpAddress?.ToString();

            var log = new AuditoriaLog
            {
                UsuarioLogin = usuarioLogin,
                Acao = acao,
                Entidade = entidade,
                Descricao = descricao,
                Ip = ip,
                DataHora = DateTime.Now
            };

            _context.AuditoriaLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
