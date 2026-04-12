using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using v5marketing.Models;

namespace v5marketing.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.Migrate();

            var passwordHasher = new PasswordHasher<Usuario>();

            if (!context.Usuarios.Any(u => u.Login == "admin"))
            {
                var admin = new Usuario
                {
                    Nome = "Administrador do Sistema",
                    Email = "admin@v5marketing.com",
                    Login = "admin",
                    Perfil = "Administrador",
                    Ativo = true,
                    DataCriacao = DateTime.Now
                };

                admin.SenhaHash = passwordHasher.HashPassword(admin, "123456");

                context.Usuarios.Add(admin);
            }

            if (!context.Usuarios.Any(u => u.Login == "operador"))
            {
                var operador = new Usuario
                {
                    Nome = "Operador do Sistema",
                    Email = "operador@v5marketing.com",
                    Login = "operador",
                    Perfil = "Operador",
                    Ativo = true,
                    DataCriacao = DateTime.Now
                };

                operador.SenhaHash = passwordHasher.HashPassword(operador, "123456");

                context.Usuarios.Add(operador);
            }

            context.SaveChanges();
        }
    }
}