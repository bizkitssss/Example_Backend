using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileRepository _repository;

        public ProfilesController(IProfileRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] UserProfile profile)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _repository.AddAsync(profile);
            return Ok(new { id, message = "save data success" });
        }
    }
}
