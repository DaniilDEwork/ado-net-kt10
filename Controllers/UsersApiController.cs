using Kt10Validation.Models;
using Kt10Validation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kt10Validation.Controllers;

[ApiController]
[Route("api/users")]
public class UsersApiController(UserService users) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(users.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        if (id <= 0) return BadRequest(new { message = "ID должен быть положительным числом" });
        var user = users.GetById(id);
        return user is null ? NotFound(new { message = "Пользователь не найден" }) : Ok(user);
    }

    [HttpPost]
    public IActionResult Create(UserRequest request)
    {
        try
        {
            var user = users.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UserRequest request)
    {
        if (id <= 0) return BadRequest(new { message = "ID должен быть положительным числом" });
        try
        {
            var user = users.Update(id, request);
            return user is null ? NotFound(new { message = "Пользователь не найден" }) : Ok(user);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (id <= 0) return BadRequest(new { message = "ID должен быть положительным числом" });
        return users.Delete(id) ? NoContent() : NotFound(new { message = "Пользователь не найден" });
    }
}
