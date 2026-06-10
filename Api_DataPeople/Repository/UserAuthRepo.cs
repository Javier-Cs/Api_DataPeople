using Api_DataPeople.Data;
using Api_DataPeople.Model;
using Dapper;
using System.Data;

namespace Api_DataPeople.Repository
{
    public class UserAuthRepo
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UserAuthRepo(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        // obtener email
        public async Task<Usuario?> ObtenerUsuarioEmailAsync(
            string email,
            IDbTransaction tx = null,
            CancellationToken ct = default
        ) {
            const string sql = @"
                SELECT 
                    id_usuario,
                    nombre_user AS nombre,
                    email_user AS email,
                    passHass AS passhass,
                    rol_user AS rol,
                    estado_user AS estado,
                    telefono AS telefono,
                    is_deleted AS is_deleted
                FROM usuario_tbl
                WHERE email_user = @email
                    AND is_deleted = 0
                    AND estado_user = 1;
            ";

            using var connection = _sqlConnectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                new CommandDefinition(
                    sql,
                    new {email},
                    transaction: tx,
                    cancellationToken: ct
                )
            );
        }

        // crear usuario
        public async Task<Usuario?> CrearUsuario(Usuario user, IDbTransaction? tx = null, CancellationToken ct = default) {
            const string sql = @"
                INSERT INTO usuario_tbl(
                    nombre_user,
                    email_user,
                    passHass,
                    telefono,
                    fecha_creacion
                )VALUES(
                    @nombre,
                    @email,
                    @passhass,
                    @telefono,
                    @fecha_creacion
                );";

            using var connection = _sqlConnectionFactory.CreateConnection();
            var respuesta = await connection.QueryFirstOrDefaultAsync<Usuario>(
                new CommandDefinition(
                    sql,
                    new { 
                        nombre_user = user.nombre,
                        email = user.email,
                        passhass = user.passhass,
                        telefono = user.telefono,
                        fecha_creacion = user.fecha_creacion
                    },
                    transaction: tx,
                    cancellationToken: ct
                )
            );
            return respuesta;
        }

        
    }
}
