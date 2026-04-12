using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace v5marketing.Filters
{
    public class RequireLoginAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var usuarioLogado = context.HttpContext.Session.GetString("UsuarioLogado");

            if (string.IsNullOrEmpty(usuarioLogado))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            await next();
        }
    }
}