using Microsoft.AspNetCore.Mvc;
using Modelo.Docente;

namespace Capitulo01.Data.DAL.Docente
{
    public class ProfessorDAL
    {
        private IESContext _context;
        public ProfessorDAL(IESContext context)
        {
            _context = context;
        }

        public IQueryable<Professor> ObterProfessoresClassificadosPorNome()
        {
            return _context.Professores.OrderBy(p => p.Nome);
        }

        public Professor ObterProfessorPorId(long id)
        {
            return _context.Professores.Find(id);
        }

        public void Inserir(Professor professor)
        {
            _context.Professores.Add(professor);
            _context.SaveChanges();
        }

        public void Atualizar(Professor professor)
        {
            _context.Professores.Update(professor);
            _context.SaveChanges();
        }

        public void Excluir(long id)
        {
            var professor = _context.Professores.Find(id);
            if (professor != null)
            {
                _context.Professores.Remove(professor);
                _context.SaveChanges();
            }
        }
    }
}