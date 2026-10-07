using Kt10Validation.Models;
using Kt10Validation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kt10Validation.Controllers;

[Route("Registration")]
public class RegistrationController(UserService users) : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View(new RegistrationRequest());

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public IActionResult Index(RegistrationRequest model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            var user = users.Create(model, model.Password);
            return RedirectToAction(nameof(Success), new { id = user.Id });
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError("", exception.Message);
            return View(model);
        }
    }

    [HttpGet("Success/{id:int}")]
    public IActionResult Success(int id)
    {
        var user = users.GetById(id);
        return user is null ? NotFound() : View(user);
    }
}
