using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitulo01.Modelo.Cadastros
{
    public class CursoDisciplina
    {
        public int CursoID { get; set; }
        public Curso Curso { get; set; } = null!;

        public int DisciplinaID { get; set; }
        public Disciplina Disciplina { get; set; } = null!;
    }
}
