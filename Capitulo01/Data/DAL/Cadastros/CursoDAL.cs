using Capitulo01.Modelo.Cadastros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelo.Docente;

namespace Capitulo01.Data.DAL.Cadastros
{
    public class CursoDAL
    {
        private IESContext _context;
        public CursoDAL(IESContext context)
        {
            _context = context;
        }

        public IQueryable<Curso> ObterCursosPorDepartamento(long departamentoID)
        {
            return _context.Cursos
                           .Where(c => c.DepartamentoID == departamentoID)
                           .OrderBy(c => c.Nome);
        }

        public IQueryable<Professor> ObterProfessoresForaDoCurso(long cursoID)
        {
            var curso = _context.Cursos
                .Where(c => c.CursoID == cursoID)
                .Include(cp => cp.CursosProfessores)
                .First();

            var professoresDoCurso = curso.CursosProfessores.Select(cp => cp.ProfessorID).ToArray();

            return _context.Professores
                           .Where(p => !professoresDoCurso.Contains(p.ProfessorID))
                           .OrderBy(p => p.Nome);
        }

        public void RegistrarProfessor(long cursoID, long professorID)
        {
            var curso = _context.Cursos
                .Where(c => c.CursoID == cursoID)
                .Include(cp => cp.CursosProfessores)
                .First();

            var professor = _context.Professores.Find(professorID);

            curso.CursosProfessores.Add(new CursoProfessor() { Curso = curso, Professor = professor });

            _context.SaveChanges();
        }

        public IQueryable<Curso> ObterCursosClassificadosPorNome()
        {
            return _context.Cursos
                          .Include(c => c.Departamento)
                          .ThenInclude(d => d.Instituicao)
                          .OrderBy(c => c.Nome);
        }

        public Curso ObterCursoPorId(long id)
        {
            return _context.Cursos
                          .Include(c => c.Departamento)        
                          .ThenInclude(d => d.Instituicao)   
                          .FirstOrDefault(c => c.CursoID == id);
        }

        public void Inserir(Curso curso)
        {
            _context.Cursos.Add(curso);
            _context.SaveChanges();
        }

        public void Atualizar(Curso curso)
        {
            _context.Cursos.Update(curso);
            _context.SaveChanges();
        }

        public void Excluir(long id)
        {
            var curso = _context.Cursos.Find(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                _context.SaveChanges();
            }
        }
    }
}