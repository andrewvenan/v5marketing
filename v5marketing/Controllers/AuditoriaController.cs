using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using v5marketing.Data;
using v5marketing.Filters;

namespace v5marketing.Controllers
{
    [RequireRole("Administrador")]
    public class AuditoriaController : Controller
    {
        private readonly AppDbContext _context;

        public AuditoriaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? usuario,
            string? acao,
            string? entidade,
            int pagina = 1,
            int tamanhoPagina = 20)
        {
            if (pagina < 1) pagina = 1;

            var query = _context.AuditoriaLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                query = query.Where(x => x.UsuarioLogin.Contains(usuario));
            }

            if (!string.IsNullOrWhiteSpace(acao))
            {
                query = query.Where(x => x.Acao.Contains(acao));
            }

            if (!string.IsNullOrWhiteSpace(entidade))
            {
                query = query.Where(x => x.Entidade.Contains(entidade));
            }

            query = query.OrderByDescending(x => x.DataHora);

            var totalRegistros = await query.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina);

            if (totalPaginas == 0) totalPaginas = 1;
            if (pagina > totalPaginas) pagina = totalPaginas;

            var logs = await query
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            ViewBag.UsuarioFiltro = usuario;
            ViewBag.AcaoFiltro = acao;
            ViewBag.EntidadeFiltro = entidade;
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.TamanhoPagina = tamanhoPagina;

            return View(logs);
        }
    }
}
