using Capitulo01.Data;
using Capitulo01.Modelo.Cadastros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Capitulo01.Data.DAL.Cadastros
{
    [Authorize]
    public class InstituicaoDAL
    {
        private readonly IESContext _context;

        public InstituicaoDAL(IESContext context)
        {
            _context = context;
        }

        public IQueryable<Instituicao> ObterInstituicoesClassificadasPorNome()
        {
            return _context.Instituicoes.OrderBy(b => b.Nome);
        }

        public async Task<Instituicao> ObterInstituicaoPorId(long id)
        {
            return await _context.Instituicoes
                                 .Include(d => d.Departamentos) 
                                 .SingleOrDefaultAsync(m => m.InstituicaoID == id);
        }

        public async Task<Instituicao> GravarInstituicao(Instituicao instituicao)
        {
            if (instituicao.InstituicaoID == null)
            {
                _context.Instituicoes.Add(instituicao);
            }
            else
            {
                _context.Instituicoes.Update(instituicao);
            }

            await _context.SaveChangesAsync();
            return instituicao;
        }

        public async Task<Instituicao?> EliminarInstituicaoPorId(long id)
        {
            var instituicao = await _context.Instituicoes
                .Include(i => i.Departamentos)
                .SingleOrDefaultAsync(i => i.InstituicaoID == id);

            if (instituicao == null) return null;

            if (instituicao.Departamentos.Any())
            {
                throw new InvalidOperationException("Não é possível excluir a instituição pois existem departamentos vinculados.");
            }

            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();

            return instituicao;
        }
    }
}