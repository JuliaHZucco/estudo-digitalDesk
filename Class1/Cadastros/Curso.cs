using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Capitulo01.Models.Cadastros
{
    public class Curso
    {
        public int CursoID { get; set; }
        public string Nome { get; set; } = string.Empty;


        public int DepartamentoID { get; set; }
        public Departamento? Departamento { get; set; }

        public ICollection<CursoDisciplina> CursosDisciplinas { get; set; } = new List<CursoDisciplina>();
    }

}
