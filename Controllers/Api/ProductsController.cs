using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("16")]
        public async Task<IActionResult> GetAll16() => Ok(await _repository.GetAll16Async());

        [HttpPost("16")]
        public async Task<IActionResult> Add16([FromBody] string code)
        {
            await _repository.Add16Async(code);
            return Ok();
        }

        [HttpDelete("16/{id}")]
        public async Task<IActionResult> Delete16(int id)
        {
            await _repository.Delete16Async(id);
            return Ok();
        }

        [HttpGet("36")]
        public async Task<IActionResult> GetAll36() => Ok(await _repository.GetAll36Async());

        [HttpPost("36")]
        public async Task<IActionResult> Add36([FromBody] string code)
        {
            await _repository.Add36Async(code);
            return Ok();
        }

        [HttpDelete("36/{id}")]
        public async Task<IActionResult> Delete36(int id)
        {
            await _repository.Delete36Async(id);
            return Ok();
        }
    }
}
