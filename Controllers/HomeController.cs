using Microsoft.AspNetCore.Mvc;

namespace Kt10Validation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    [HttpGet("/Users")]
    public IActionResult Users() => View();

    public IActionResult Error()
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        return View();
    }
}
