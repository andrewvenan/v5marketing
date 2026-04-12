using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using v5marketing.Data;
using v5marketing.Filters;
using v5marketing.ViewModels;

namespace v5marketing.Controllers
{
    [RequireLogin]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            var clientesPorCidade = clientes
                .Where(c => !string.IsNullOrWhiteSpace(c.Cidade))
                .GroupBy(c => c.Cidade)
                .Select(g => new ClientesPorCidadeViewModel
                {
                    Cidade = g.Key ?? string.Empty,
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Quantidade)
                .ThenBy(x => x.Cidade)
                .Take(10)
                .ToList();

            var dashboard = new DashboardViewModel
            {
                TotalClientes = clientes.Count,
                TotalCidades = clientes
                    .Where(c => !string.IsNullOrWhiteSpace(c.Cidade))
                    .Select(c => c.Cidade)
                    .Distinct()
                    .Count(),

                TotalComEmail = clientes.Count(c => !string.IsNullOrWhiteSpace(c.Email)),
                TotalComTelefone = clientes.Count(c => !string.IsNullOrWhiteSpace(c.Telefone)),

                UltimosClientes = clientes.Take(5).ToList(),
                ClientesPorCidade = clientesPorCidade
            };

            return View(dashboard);
        }
    }
}