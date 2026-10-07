using System.ComponentModel.DataAnnotations;

namespace Kt10Validation.Models;

public class UserRequest
{
    [Required(ErrorMessage = "Введите имя пользователя")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Имя должно содержать от 3 до 100 символов")]
    [RegularExpression(@"^\p{L}+$", ErrorMessage = "Имя должно содержать только буквы")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "Введите электронную почту")]
    [StringLength(100, ErrorMessage = "Почта не должна быть длиннее 100 символов")]
    [EmailAddress(ErrorMessage = "Введите корректную электронную почту")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Почта должна содержать @ и точку в домене")]
    public string Email { get; set; } = "";
}

public class RegistrationRequest : UserRequest
{
    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен содержать от 8 до 100 символов")]
    [RegularExpression(@"^(?=[\s\S]*\p{L})(?=[\s\S]*\d)[\s\S]+$", ErrorMessage = "Пароль должен содержать буквы и цифры")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Подтвердите пароль")]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = "";
}
