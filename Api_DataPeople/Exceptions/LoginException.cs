namespace Api_DataPeople.Exceptions
{
    public class LoginException : Exception
    {
        public int IntentosRestantes { get; }
        public bool Bloqueado { get; }

        public LoginException(
            string mensaje,
            int intentosRestantes,
            bool bloqueado = false
        ) : base(mensaje) 
        {
            IntentosRestantes = intentosRestantes;
            Bloqueado = bloqueado;
        }
    }
}
