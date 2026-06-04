using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueuesController : ControllerBase
    {
        private readonly IQueueRepository _repository;

        public QueuesController(IQueueRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("next")]
        public async Task<IActionResult> GetNext()
        {
            var queueNumber = await _repository.GetNextQueueAsync();
            return Ok(new { queueNumber });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset()
        {
            await _repository.ResetQueueAsync();
            return Ok(new { message = "Queue reset successfully" });
        }
    }
}
