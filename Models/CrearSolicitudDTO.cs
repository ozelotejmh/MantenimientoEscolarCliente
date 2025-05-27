using System;
using System.ComponentModel.DataAnnotations;

namespace MantenimientoEscolarCliente.Models
{
    public class CrearSolicitudDTO
    {
        [Required]
        public int usuarioId { get; set; }

        [Required]
        public int categoriaId { get; set; }

        [Required]
        public string descripcion { get; set; }

        [Required]
        public string ubicacion { get; set; }

        [Required]
        public DateTime fecha { get; set; }

        [Required]
        public string estado { get; set; }
    }
}