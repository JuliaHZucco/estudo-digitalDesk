using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;


namespace Modelo.Discente
{
    public class Academico
    {
        [DisplayName("Id")]
        public long? AcademicoID { get; set; }

        [Required(ErrorMessage = "O RA é obrigatório.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "O RA deve ter exatamente 10 dígitos.")]
        [RegularExpression("([0-9]{10})", ErrorMessage = "O RA deve conter apenas números.")]
        [DisplayName("RA")]
        public string RegistroAcademico { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        [DisplayName("Nascimento")]
        public DateTime Nascimento { get; set; }

        [DisplayName("Tipo da Foto")]
        public string FotoMimeType { get; set; }

        [DisplayName("Foto")]
        public byte[] Foto { get; set; }

        [NotMapped]
        [DisplayName("Selecionar Foto")]
        public IFormFile formFile { get; set; }
    }

}
