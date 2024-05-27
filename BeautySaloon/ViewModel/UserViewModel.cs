using System.ComponentModel.DataAnnotations;

namespace BeautySaloon.ViewModel;

public class UserViewModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Имя обязательно для заполнения")]
    [Display(Name = "Имя")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
    [Display(Name = "Фамилия")]
    public string SecondName { get; set; }

    [Display(Name = "Отчество")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Почта обязательна для заполнения")]
    [EmailAddress(ErrorMessage = "Некорректный формат почты")]
    [Display(Name = "Почта")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен для заполнения")]
    [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Роль обязательна для выбора")]
    [Display(Name = "Роль")]
    public Guid RoleId { get; set; }
    public string SelectedRole { get; set; }
    
    public bool RememberMe { get; set; }
    public List<RoleViewModel>? Roles { get; set; }
}