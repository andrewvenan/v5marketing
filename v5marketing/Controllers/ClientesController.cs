using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using v5marketing.Data;
using v5marketing.Filters;
using v5marketing.Models;
using v5marketing.Services;

namespace v5marketing.Controllers
{
    [RequireLogin]
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuditoriaService _auditoriaService;

        public ClientesController(AppDbContext context, IAuditoriaService auditoriaService)
        {
            _context = context;
            _auditoriaService = auditoriaService;
        }

        private string? PerfilUsuario()
        {
            return HttpContext.Session.GetString("UsuarioPerfil");
        }

        private bool UsuarioAdministrador()
        {
            return PerfilUsuario() == "Administrador";
        }

        public async Task<IActionResult> Index(
            string? busca,
            string? ordenacao,
            int pagina = 1,
            int tamanhoPagina = 10)
        {
            if (pagina < 1) pagina = 1;

            var clientesQuery = _context.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                clientesQuery = clientesQuery.Where(c =>
                    c.RazaoSocial.Contains(busca) ||
                    c.NomeFantasia.Contains(busca) ||
                    c.Cnpj.Contains(busca) ||
                    c.NomeContato.Contains(busca) ||
                    c.Email.Contains(busca) ||
                    c.Cidade.Contains(busca));
            }

            ViewBag.Busca = busca;
            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.TamanhoPagina = tamanhoPagina;
            ViewBag.UsuarioAdministrador = UsuarioAdministrador();

            ViewBag.OrdenarPorRazao = ordenacao == "razao_asc" ? "razao_desc" : "razao_asc";
            ViewBag.OrdenarPorCidade = ordenacao == "cidade_asc" ? "cidade_desc" : "cidade_asc";
            ViewBag.OrdenarPorCnpj = ordenacao == "cnpj_asc" ? "cnpj_desc" : "cnpj_asc";

            clientesQuery = ordenacao switch
            {
                "razao_asc" => clientesQuery.OrderBy(c => c.RazaoSocial),
                "razao_desc" => clientesQuery.OrderByDescending(c => c.RazaoSocial),
                "cidade_asc" => clientesQuery.OrderBy(c => c.Cidade),
                "cidade_desc" => clientesQuery.OrderByDescending(c => c.Cidade),
                "cnpj_asc" => clientesQuery.OrderBy(c => c.Cnpj),
                "cnpj_desc" => clientesQuery.OrderByDescending(c => c.Cnpj),
                _ => clientesQuery.OrderByDescending(c => c.Id)
            };

            var totalRegistros = await clientesQuery.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina);

            if (totalPaginas == 0) totalPaginas = 1;
            if (pagina > totalPaginas) pagina = totalPaginas;

            var clientes = await clientesQuery
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalRegistros = totalRegistros;

            return View(clientes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [RequireRole("Administrador", "Operador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireRole("Administrador", "Operador")]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            _context.Add(cliente);
            await _context.SaveChangesAsync();

            await _auditoriaService.RegistrarAsync("Criar", "Cliente", $"Cliente {cliente.RazaoSocial} cadastrado");

            TempData["Sucesso"] = "Cliente cadastrado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [RequireRole("Administrador", "Operador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireRole("Administrador", "Operador")]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(cliente);

            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.Id == cliente.Id))
                    return NotFound();

                throw;
            }

            await _auditoriaService.RegistrarAsync("Editar", "Cliente", $"Cliente {cliente.RazaoSocial} atualizado");

            TempData["Sucesso"] = "Cliente atualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [RequireRole("Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RequireRole("Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                await _auditoriaService.RegistrarAsync("Excluir", "Cliente", $"Cliente {cliente.RazaoSocial} excluído");

                TempData["Sucesso"] = "Cliente excluído com sucesso.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}