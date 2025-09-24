using System.ComponentModel.DataAnnotations;
namespace Capitulo01.Models
{
    public class Instituicao
    {
        public long? InstituicaoID { get; set; }
        [Required(ErrorMessage = "O nome da instituição é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(200, ErrorMessage = "O endereço não pode exceder 200 caracteres.")]
        public string Endereco { get; set; }
        public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();
    }
}