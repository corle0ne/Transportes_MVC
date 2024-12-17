using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Camiones_DTO
    {

        [Key] //data annotation
        public int ID_Camion { get; set; }
        [Required]
        [Display(Name ="Matrícula")]
        public string Matricula { get; set; }

        [Required]
        [Display(Name = "Tipo_Camion")]

        public string Tipo_Camion { get; set; }

        [Required]
        [Display(Name = "Marca")]

        public string Marca { get; set; }

        [Required]
        [Display(Name = "Modelo")]

        public string Modelo { get; set; }

        [Required]
        [Display(Name = "Capacidad")]

        public int Capacidad { get; set; }

        [Required]
        [Display(Name = "Kilometraje")]

        public double Kilometraje { get; set; }

        [DataType(DataType.ImageUrl)]

        public string UrlFoto { get; set; }

        [Required]
        [Display(Name = "Disponibilidad")]
        public bool Disponibilidad { get; set; }
    }
}
