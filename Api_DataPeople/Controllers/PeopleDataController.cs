using Api_DataPeople.DTO;
using Api_DataPeople.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nottyn.Dtos.salida;

namespace Api_DataPeople.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class PeopleDataController : ControllerBase
    {
        public readonly IPeopleDataService _peopleDataService;

        public PeopleDataController(IPeopleDataService peopleDataService) { 
            _peopleDataService = peopleDataService;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDataPeople([FromQuery]string dato, CancellationToken ct) {
            try
            {
                var dataPeople = await _peopleDataService.GetDataPeople(dato, ct);
                return Ok(dataPeople);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
    }
}
