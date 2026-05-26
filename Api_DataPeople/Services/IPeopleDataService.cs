using Api_DataPeople.DTO;

namespace Api_DataPeople.Services
{
    public interface IPeopleDataService
    {
        public Task<DataPeopleResponseDto> GetDataPeople(string datos, CancellationToken ct);
    }
}
