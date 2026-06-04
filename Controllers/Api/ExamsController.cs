using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamRepository _repository;

        public ExamsController(IExamRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Exam exam)
        {
            var id = await _repository.AddAsync(exam);
            return Ok(new { id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] ExamResult result)
        {
            var id = await _repository.AddResultAsync(result);
            return Ok(new { id });
        }

        [HttpGet("results")]
        public async Task<IActionResult> GetResults() => Ok(await _repository.GetResultsAsync());
    }
}
