using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Docente
{
    public class Professor
    {
        public long ProfessorID { get; set; }

        [Required(ErrorMessage = "O nome do professor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        public virtual ICollection<CursoProfessor> CursosProfessores { get; set; } = new List<CursoProfessor>();
    }

}
