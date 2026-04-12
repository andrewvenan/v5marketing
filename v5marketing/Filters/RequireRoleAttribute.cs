using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace v5marketing.Filters
{
    public class RequireRoleAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string[] _rolesPermitidos;

        public RequireRoleAttribute(params string[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var usuarioLogado = context.HttpContext.Session.GetString("UsuarioLogado");
            var perfil = context.HttpContext.Session.GetString("UsuarioPerfil");

            if (string.IsNullOrEmpty(usuarioLogado))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (string.IsNullOrEmpty(perfil) || !_rolesPermitidos.Contains(perfil))
            {
                context.Result = new RedirectToActionResult("Index", "Dashboard", null);
                return;
            }

            await next();
        }
    }
}