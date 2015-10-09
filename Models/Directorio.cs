using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace DirectorioSahuro.Models
{
    public class Directorio
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "El Nombre es requerido")]
        public string NOMBRE { get; set; }

        [Required(ErrorMessage = "Fecha de Nacimiento requerida")]
        public DateTime FECHA_NACIMIENTO { get; set; }

        public string FOTOGRAFIA { get; set; }

        [Required(ErrorMessage = "Telefono(s) requerido")]
        public string TELEFONOS { get; set; }
    }

    public class DirectorioDBContext : DbContext
    {
        public DbSet<Directorio> Directorio { get; set; }
    }
}