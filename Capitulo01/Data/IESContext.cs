using Capitulo01.Models.Cadastros;
using Microsoft.EntityFrameworkCore;

namespace Capitulo01.Data
{
    public class IESContext : DbContext
    {
        public IESContext(DbContextOptions<IESContext> options) : base(options) { }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Instituicao> Instituicoes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departamento>()
                .HasOne(d => d.Instituicao)
                .WithMany(i => i.Departamentos)
                .HasForeignKey(d => d.InstituicaoID)
                .OnDelete(DeleteBehavior.Restrict);  
            base.OnModelCreating(modelBuilder);
        }
    }
}
