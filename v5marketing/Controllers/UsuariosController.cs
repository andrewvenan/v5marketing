using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using v5marketing.Data;
using v5marketing.Filters;
using v5marketing.Models;
using v5marketing.Services;
using v5marketing.ViewModels;

namespace v5marketing.Controllers
{
    [RequireRole("Administrador")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;
        private readonly IAuditoriaService _auditoriaService;

        public UsuariosController(AppDbContext context, IAuditoriaService auditoriaService)
        {
            _context = context;
            _auditoriaService = auditoriaService;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        private string? LoginUsuarioAtual()
        {
            return HttpContext.Session.GetString("UsuarioLogado");
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .OrderBy(u => u.Nome)
                .ToListAsync();

            return View(usuarios);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        public IActionResult Create()
        {
            return View(new UsuarioCadastroViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCadastroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool emailJaExiste = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
            if (emailJaExiste)
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                return View(model);
            }

            bool loginJaExiste = await _context.Usuarios.AnyAsync(u => u.Login == model.Login);
            if (loginJaExiste)
            {
                ModelState.AddModelError("Login", "Este login já está cadastrado.");
                return View(model);
            }

            var usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                Login = model.Login,
                Perfil = model.Perfil,
                Ativo = model.Ativo,
                DataCriacao = DateTime.Now
            };

            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, model.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            await _auditoriaService.RegistrarAsync("Criar", "Usuario", $"Usuário {usuario.Login} cadastrado");

            TempData["Sucesso"] = "Usuário cadastrado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            var model = new UsuarioEdicaoViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Login = usuario.Login,
                Perfil = usuario.Perfil,
                Ativo = usuario.Ativo
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioEdicaoViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            bool emailJaExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == model.Email && u.Id != id);

            if (emailJaExiste)
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                return View(model);
            }

            bool loginJaExiste = await _context.Usuarios
                .AnyAsync(u => u.Login == model.Login && u.Id != id);

            if (loginJaExiste)
            {
                ModelState.AddModelError("Login", "Este login já está cadastrado.");
                return View(model);
            }

            usuario.Nome = model.Nome;
            usuario.Email = model.Email;
            usuario.Login = model.Login;
            usuario.Perfil = model.Perfil;
            usuario.Ativo = model.Ativo;

            if (!string.IsNullOrWhiteSpace(model.NovaSenha))
            {
                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, model.NovaSenha);
            }

            _context.Update(usuario);
            await _context.SaveChangesAsync();

            await _auditoriaService.RegistrarAsync("Editar", "Usuario", $"Usuário {usuario.Login} atualizado");

            TempData["Sucesso"] = "Usuário atualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            ViewBag.UsuarioAtual = LoginUsuarioAtual();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return RedirectToAction(nameof(Index));

            if (usuario.Login == LoginUsuarioAtual())
            {
                TempData["Erro"] = "Você não pode excluir o próprio usuário logado.";
                return RedirectToAction(nameof(Index));
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            await _auditoriaService.RegistrarAsync("Excluir", "Usuario", $"Usuário {usuario.Login} excluído");

            TempData["Sucesso"] = "Usuário excluído com sucesso.";
            return RedirectToAction(nameof(Index));
        }
    }
}