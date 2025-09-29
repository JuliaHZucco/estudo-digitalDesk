
using Capitulo01.Data;
using Capitulo01.Modelo.Cadastros;
using System.Linq;

namespace Capitulo01.Data
{
    public class IESDbInitializer
    {
        public static void Initialize(IESContext context)
        {
            context.Database.EnsureCreated();

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