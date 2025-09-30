using Capitulo01.Modelo.Cadastros;
using Capitulo01.Models.Infra;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modelo.Discente;
using Modelo.Docente;

namespace Capitulo01.Data
{
    public class IESContext : IdentityDbContext<UsuarioDaAplicacao>
    {
        public IESContext(DbContextOptions<IESContext> options) : base(options) { }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Academico> Academicos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<CursoProfessor> CursosProfessores { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Departamento>()
                .HasOne(d => d.Instituicao)
                .WithMany(i => i.Departamentos)
                .HasForeignKey(d => d.InstituicaoID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Curso>()
                .HasOne(c => c.Departamento)
                .WithMany(d => d.Cursos)
                .HasForeignKey(c => c.DepartamentoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Disciplina>()
                .HasOne(d => d.Departamento)
                .WithMany() 
                .HasForeignKey(d => d.DepartamentoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CursoDisciplina>()
                .HasKey(cd => new { cd.CursoID, cd.DisciplinaID });

            modelBuilder.Entity<CursoDisciplina>()
                .HasOne(c => c.Curso)
                .WithMany(cd => cd.CursosDisciplinas)
                .HasForeignKey(c => c.CursoID);

            modelBuilder.Entity<CursoDisciplina>()
                .HasOne(d => d.Disciplina)
                .WithMany(cd => cd.CursosDisciplinas)
                .HasForeignKey(d => d.DisciplinaID);

            modelBuilder.Entity<CursoProfessor>()
                .HasKey(cp => new { cp.CursoID, cp.ProfessorID });

            modelBuilder.Entity<CursoProfessor>()
                .HasOne(c => c.Curso)
                .WithMany(cd => cd.CursosProfessores)
                .HasForeignKey(c => c.CursoID);

            modelBuilder.Entity<CursoProfessor>()
                .HasOne(p => p.Professor)
                .WithMany(cd => cd.CursosProfessores)
                .HasForeignKey(p => p.ProfessorID);

            base.OnModelCreating(modelBuilder);
        }
    }
}