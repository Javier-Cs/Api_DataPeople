using Api_DataPeople.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{dato}")]
        public async Task<IActionResult> GetDataPeople(string dato, CancellationToken ct) {
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
