using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitulo01.Modelo.Cadastros
{
    public class Disciplina
    {
        public int DisciplinaID { get; set; }
        public string Nome { get; set; } = string.Empty;

        public int DepartamentoID { get; set; }
        public Departamento? Departamento { get; set; }

        public ICollection<CursoDisciplina> CursosDisciplinas { get; set; } = new List<CursoDisciplina>();
    }
}

