namespace Api_DataPeople.Model
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string? nombre { get; set; }
        public string? rol {  get; set; }
        public string? email { get; set; }
        public string? passhass { get; set; }
        public bool estado {  get; set; }
        public bool is_deleted { get; set; }
        public string? telefono { get; set; }
        public DateTime fecha_creacion { get; set; }
    }
}
