namespace Nottyn.Dtos.salida
{
    public class SLUsuarioLoginDto
    {
        public string Token { get; set; } = string .Empty;
        public DateTime Expiracion { get; set; }
        public bool Estado { get; set; }
        public int idUsuario { get; set; }
        public string? rol { get; set; }
        public string? Nombre { get; set; }

    }
}
