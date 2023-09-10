using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Product_API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController] 
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class BaseController : ControllerBase
{
    [HttpGet("hc")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Hc()
    { 
        return Ok(new { Status = "Healthy" });
    }
}