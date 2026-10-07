namespace Kt10Validation.Models;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PasswordHash { get; set; }
}

public class UserResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
}
