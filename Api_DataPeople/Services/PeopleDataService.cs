using Api_DataPeople.DTO;
using System.Text.Json;

namespace Api_DataPeople.Services
{
    public class PeopleDataService : IPeopleDataService
    {
        private readonly string _urlBase;
        private readonly HttpClient _httpClient;

        public PeopleDataService(HttpClient httpClient, IConfiguration configuracion) {
            _urlBase = configuracion.GetValue<string>("urlApiDatos");
            _httpClient = httpClient;
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

    }
}
