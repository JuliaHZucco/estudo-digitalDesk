using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Capitulo01.Modelo.Cadastros
{
    public class Disciplina
    {
        public long DisciplinaID { get; set; }  

        [Required(ErrorMessage = "O nome da disciplina é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecione um departamento.")]
        public long DepartamentoID { get; set; }  
        public Departamento? Departamento { get; set; }

        public ICollection<CursoDisciplina> CursosDisciplinas { get; set; } = new List<CursoDisciplina>();
    }
}