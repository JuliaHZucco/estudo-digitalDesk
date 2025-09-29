using Capitulo01.Data;
using Capitulo01.Modelo.Cadastros;
using Capitulo01.Models.Infra;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Capitulo01.Data
{
    public class IESDbInitializer
    {
        public static async Task Initialize(IESContext context, UserManager<UsuarioDaAplicacao> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (await userManager.FindByEmailAsync("admin@ies.edu.br") == null)
            {
                var adminUser = new UsuarioDaAplicacao
                {
                    UserName = "admin@ies.edu.br",
                    Email = "admin@ies.edu.br",
                    NomeCompleto = "Administrador do Sistema",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            if (context.Departamentos.Any() || context.Instituicoes.Any())
            {
                return;
            }

            var uniParana = new Instituicao { Nome = "UniParaná", Endereco = "Paraná" };
            var uniAcre = new Instituicao { Nome = "UniAcre", Endereco = "Acre" };

            context.Instituicoes.AddRange(uniParana, uniAcre);
            context.SaveChanges();

            var departamentos = new Departamento[]
            {
                new Departamento { Nome = "Ciência da Computação", InstituicaoID = uniParana.InstituicaoID },
                new Departamento { Nome = "Ciência de Alimentos", InstituicaoID = uniAcre.InstituicaoID }
            };

            context.Departamentos.AddRange(departamentos);
            context.SaveChanges();
        }
    }
}