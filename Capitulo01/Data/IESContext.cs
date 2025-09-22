using Capitulo01.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace Capitulo01.Data
{
    public class IESContext : DbContext
    {
        public IESContext(DbContextOptions<IESContext> options) : base(options)
        {
        }
        public DbSet<Departamento> Departamentos { get; set; }
    }
}
