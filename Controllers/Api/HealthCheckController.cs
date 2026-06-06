using Backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly DbContext _context;

        public HealthCheckController(DbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "APIHealthCheck")]
        public object APIHealthCheck()
        {
            try
            {
                return Ok(new { status = 200, success = true, message = $"Connect {AppSetting.AssemblyName} Success!" });
            }
            catch (Exception ex)
            {
                return NotFound(new { status = 500, success = false, message = $"{ex.Message} | Inner: {ex.InnerException?.Message}" });
            }
        }

        [HttpGet("ConnectionCheck", Name = "ConnectionCheck")]
        public object ConnectionCheck()
        {
            try
            {
                using (var con = _context.CreateConnection())
                {
                    try
                    {
                        con.Open();
                        return Ok(new { status = 200, success = true, message = $"Connect to DB Success!" });
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally { con.Close(); }
                }  
            }
            catch (Exception ex)
            {
                return NotFound(new { status = 500, success = false, message = $"{ex.Message} | Inner: {ex.InnerException?.Message}" });
            }
        }
    }
}