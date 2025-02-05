using NumberClassificationApi.Models;
using NumberClassificationApi.Services;
using Microsoft.AspNetCore.Mvc;
using NumberClassificationApi.Interface;

namespace NumberClassificationApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class NumberController : ControllerBase
    {
        private readonly INumberService _numberService;

        public NumberController(INumberService numberService)
        {
            _numberService = numberService;
        }

        [HttpGet("classify-number")]
        public async Task<IActionResult> ClassifyNumber([FromQuery] string number)
        {
            if (!int.TryParse(number, out int num) || num < 0)
            {
                return BadRequest(new { number = "alphabet", error = true });
            }

            var response = await _numberService.ClassifyNumber(num);
            return Ok(response);
        }
    }
}
