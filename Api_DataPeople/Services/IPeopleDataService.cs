using Api_DataPeople.Dto.respuestaApi;
using Api_DataPeople.DTO;
using Nottyn.Dtos.salida;

namespace Api_DataPeople.Services
{
    public interface IPeopleDataService
    {
        public Task<DataPeopleResponseDto> GetDataPeople(string datos, CancellationToken ct);
        public Task<SLUsuarioLoginDto> LoginAsync(LoginDto loginDto, string ip, CancellationToken ct);
    }
}
