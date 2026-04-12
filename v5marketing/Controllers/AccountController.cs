using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using v5marketing.Data;
using v5marketing.Models;
using v5marketing.Services;
using v5marketing.ViewModels;

namespace v5marketing.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;
        private readonly IAuditoriaService _auditoriaService;

        public AccountController(AppDbContext context, IAuditoriaService auditoriaService)
        {
            _context = context;
            _auditoriaService = auditoriaService;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogado")))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == model.Usuario);

            if (usuario == null)
            {
                ViewBag.Erro = "Usuário ou senha inválidos.";
                return View(model);
            }

            if (!usuario.Ativo)
            {
                ViewBag.Erro = "Usuário inativo. Procure um administrador.";
                return View(model);
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha);

            if (resultado == PasswordVerificationResult.Failed)
            {
                ViewBag.Erro = "Usuário ou senha inválidos.";
                return View(model);
            }

            HttpContext.Session.SetString("UsuarioLogado", usuario.Login);
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioPerfil", usuario.Perfil);

            await _auditoriaService.RegistrarAsync("Login", "Usuario", $"Login realizado por {usuario.Login}");

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            var login = HttpContext.Session.GetString("UsuarioLogado") ?? "Desconhecido";

            await _auditoriaService.RegistrarAsync("Logout", "Usuario", $"Logout realizado por {login}");

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}