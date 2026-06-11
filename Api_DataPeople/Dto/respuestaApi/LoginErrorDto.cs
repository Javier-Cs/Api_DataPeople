namespace Api_DataPeople.Dto.respuestaApi
{
    public class LoginErrorDto
    {
        public string Message { get; set; } = "";
        public int IntentosRestantes { get; set; }
        public bool Bloqueado { get; set; }
    }
}
