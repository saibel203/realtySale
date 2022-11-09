using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RealtySale.Api.Controllers;

[Route("api/{controller}")]
[ApiController]
public class BaseController : ControllerBase
{
    public int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
