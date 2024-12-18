using CsvTest.Application.Model;
using CsvTest.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsvTest.Controllers
{
    [Route("CsvData")]
    public class CsvDataController : Controller
    {
        private readonly CsvDataService _service;

        public CsvDataController(CsvDataService service)
        {
            _service = service;
        }

        [HttpPost("WriteCsv")]
        public async Task<ActionResult<ResponseModel>> WriteCsv(IFormFileCollection files)
        {
            try
            {
                return Ok(await _service.WriteCsv(files));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("HighestTipAmount")]
        public async Task<ActionResult<ResponseModel<int>>> GetHighestTipAmount()
        {
            try
            {
                return Ok(await _service.GetHighestTipAmount());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("LongestTripsDistance")]
        public async Task<ActionResult<ResponseModel<List<CsvRecord>>>> GetLongestTripDistance()
        {
            try
            {
                return Ok(await _service.GetLongestTripsDistance());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("LongestTripsByTime")]
        public async Task<ActionResult<ResponseModel<List<CsvRecord>>>> GetLongestTripsByTime()
        {
            try
            {
                return Ok(await _service.GetLongestTripsByTime());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TripsList/{puLocationId:int}")]
        public async Task<ActionResult<List<int>>> GetList(int puLocationId)
        {
            try
            {
                return Ok(await _service.GetList(puLocationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
