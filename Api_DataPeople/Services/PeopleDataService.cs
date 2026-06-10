using Api_DataPeople.Dto.respuestaApi;
using Api_DataPeople.DTO;
using Api_DataPeople.Model;
using Api_DataPeople.Repository;
using Api_DataPeople.Validacion;
using Nottyn.Dtos.salida;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api_DataPeople.Services
{
    public class PeopleDataService : IPeopleDataService
    {
        private readonly string _urlBase;
        private readonly ContarIntentosRepo _contarIntentosRepo;
        private readonly HttpClient _httpClient;
        private readonly UserAuthRepo _userAuthRepo;
        private readonly JWTService _jwtService;

        public PeopleDataService(HttpClient httpClient, IConfiguration configuracion, UserAuthRepo userAuthRepo, JWTService jWTService, ContarIntentosRepo contarIntentosRepo) {
            _urlBase = configuracion.GetValue<string>("urlApiDatos");
            _httpClient = httpClient;
            _userAuthRepo = userAuthRepo;
            _jwtService = jWTService;
            _contarIntentosRepo = contarIntentosRepo;
        }

        public async Task<DataPeopleResponseDto> GetDataPeople(string datos, CancellationToken ct) {
            var response = await _httpClient.GetAsync($"{_urlBase}/{datos}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DataPeopleResponseDto>(
                    content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            else {
                throw new Exception($"Error al obtener los datos: {response.ReasonPhrase}");
            }
        }



        public async Task<SLUsuarioLoginDto> LoginAsync(LoginDto loginDto,string ip, CancellationToken ct)
        {
            Usuario response = await _userAuthRepo.ObtenerUsuarioEmailAsync(loginDto.Email,null, ct);

            if (string.IsNullOrWhiteSpace(loginDto.Email))
            {
                throw new Exception("Credenciales incorrectas.");
            }

            if (response == null) {
                await _contarIntentosRepo.RegistrarIntentosFallidos(loginDto.Email, ip);
                throw new Exception("credencial erronea.");
            }

            if (response.email == null) {
                throw new Exception("El correo es nulo");
            }

            if (string.IsNullOrEmpty(loginDto.Password)) {
                throw new Exception("Password Vacio");
            }

            var pass = loginDto.Password.Trim();

            if (response.is_deleted) {
                throw new Exception("La cuenta del usuario no puede realizar actividad.");
            }


            // contar intentos  poir correo ----------------
            var intentos = await _contarIntentosRepo.ContarIntentosUltimosMinutos(loginDto.Email);
            // contar intentos por ip
            var intentosPorIp = await _contarIntentosRepo.ContarIntentosPorIp(ip);



            if (intentos >= 5) {
                throw new Exception("Cuenta Bloqueada temporalmente por 30 min, ponte pilas.");
            }

            if (intentosPorIp >= 20) {
                throw new Exception("Demasiados intentos desde esta ip, ponte pilas.");
            }


            if (!BCrypt.Net.BCrypt.Verify(pass, response.passhass))
            {
                // como se obtiene la ip?
                await _contarIntentosRepo.RegistrarIntentosFallidos(loginDto.Email, ip);
                throw new Exception("credencial erronea, ponte pilas.");
            }
            else {
                await _contarIntentosRepo.LimpiarIntentos(loginDto.Email);
            }

            var expiracion = DateTime.UtcNow.AddMinutes(20);
            var token = _jwtService.GenerarToken(response, expiracion);

            return new SLUsuarioLoginDto { 
                Token = token,
                Expiracion = expiracion,
                Estado = response.estado,
                idUsuario = response.id_usuario,
                rol = response.rol,
                Nombre = response.nombre
            };
        }

    
    }
}
