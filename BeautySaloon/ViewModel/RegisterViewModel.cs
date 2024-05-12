using System.ComponentModel.DataAnnotations;

namespace BeautySaloon.ViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Поле 'Имя' обязательно.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Поле 'Фамилия' обязательно.")]
    public string SecondName { get; set; }

    public string? LastName { get; set; }

    [Required(ErrorMessage = "Поле 'Email' обязательно.")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Поле 'Пароль' обязательно.")]
    public string Password { get; set; }
    [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword { get; set; }
    public string? Phone { get; set; }
}