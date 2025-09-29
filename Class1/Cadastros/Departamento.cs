using System.ComponentModel.DataAnnotations;

namespace Capitulo01.Modelo.Cadastros;
    public class Departamento
    {
        public long? DepartamentoID { get; set; }
        [Required(ErrorMessage = "O nome do departamento é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Selecione uma instituição.")]
        public long? InstituicaoID { get; set; }  
        public Instituicao? Instituicao { get; set; }
        public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}

