using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _repository;

        public DocumentsController(IDocumentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var docs = await _repository.GetAllAsync();
            return Ok(docs);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id, [FromBody] string reason)
        {
            await _repository.UpdateStatusAsync(id, "อนุมัติ", reason);
            return Ok();
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromBody] string reason)
        {
            await _repository.UpdateStatusAsync(id, "ไม่อนุมัติ", reason);
            return Ok();
        }

        [HttpPost("bulk-approve")]
        public async Task<IActionResult> BulkApprove([FromBody] BulkActionRequest request)
        {
            await _repository.UpdateStatusesAsync(request.Ids, "อนุมัติ", request.Reason);
            return Ok();
        }

        [HttpPost("bulk-reject")]
        public async Task<IActionResult> BulkReject([FromBody] BulkActionRequest request)
        {
            await _repository.UpdateStatusesAsync(request.Ids, "ไม่อนุมัติ", request.Reason);
            return Ok();
        }
    }

    public class BulkActionRequest
    {
        public List<int> Ids { get; set; } = new List<int>();
        public string? Reason { get; set; }
    }
}
