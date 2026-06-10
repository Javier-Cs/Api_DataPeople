namespace Api_DataPeople.Dto.respuestaApi
{
    public class DataPeopleResponseDto
    {
        public string? Cedula { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Conyuge { get; set; }
        public DateTime? FechaCedulacion { get; set; }
        public string? LugarDomicilio { get; set; }
        public string? CalleDomicilio { get; set; }
        public string? NumeracionDomicilio { get; set; }
        public string? NombreMadre { get; set; }
        public string? NombrePadre { get; set; }
        public string? LugarNacimiento { get; set; }
        public DateTime? FechaConsulta { get; set; }
        public DateTime? FechaExpira { get; set; }
        public string? Genero { get; set; }
        public string? Nacionalidad { get; set; }
        public string? Instruccion { get; set; }
        public string? Profesion { get; set; }
        public string? Condicion { get; set; }
        public List<LicenciasDeConducirDto>? licenciasDeConducir { get; set; }
    }
}
