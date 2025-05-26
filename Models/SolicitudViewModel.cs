namespace MantenimientoEscolarCliente.Models
{
    public class SolicitudViewModel
    {
        public int idSolicitud { get; set; }
        public int usuarioId { get; set; }
        public string nombreUsuario { get; set; }
        public string correo { get; set; }
        public string tipoUsuario { get; set; }
        public int categoriaId { get; set; }
        public string nombreCategoria { get; set; }
        public string descripcion { get; set; }
        public string ubicacion { get; set; }
        public DateTime fecha { get; set; }
        public string estado { get; set; }
    }
}
