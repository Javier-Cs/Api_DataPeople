using Api_DataPeople.Data;
using Dapper;

namespace Api_DataPeople.Repository
{
    public class ContarIntentosRepo
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;


        public ContarIntentosRepo(ISqlConnectionFactory sqlConnectionFactory) {
            _sqlConnectionFactory = sqlConnectionFactory;
        }


        //contar intentos de login
        public async Task<int> ContarIntentosUltimosMinutos(string email, int minutos = 30)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM login_attempts
                WHERE email = @email
                    AND fecha >= DATEADD(MINUTE, -@minutos, GETDATE());
            ";

            using var connection = _sqlConnectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                sql,
                new 
                {
                    email,
                    minutos
                }
            );
        }


        //contar intentos por ip
        public async Task<int> ContarIntentosPorIp(string ip, int minutos = 50) {
            const string sql = @"
                SELECT COUNT(*)
                FROM login_attempts
                WHERE ip = @ip
                AND fecha >= DATEADD(MINUTE,-@minutos,GETDATE())
            ";

            using var connection = _sqlConnectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                sql,
                new 
                { 
                    ip,
                    minutos
                }
            );
            
        }





        // guardar intentos fallidos 
        public async Task RegistrarIntentosFallidos(string email, string? ip = null) {
            const string sql = @"
                INSERT INTO  login_attempts
                (
                    email,
                    fecha,
                    ip
                )VALUES(
                    @email,
                    GETDATE(),
                    @ip
                );
                ";
            using var connection = _sqlConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                sql,
                new
                {
                    email,
                    ip
                }
            );
        }


        // limpiar intentos cuando el login es correcto
        public async Task LimpiarIntentos(string email) {
            const string sql = @"
                DELETE FROM login_attempts
                WHERE email = @email;
            ";

            using var connection = _sqlConnectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                sql,
                new {email}
            );
        }

    }
}
