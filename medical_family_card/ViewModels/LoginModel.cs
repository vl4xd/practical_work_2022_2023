using System.ComponentModel.DataAnnotations;

namespace medical_family_card.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указана Электронная почта")]
        public string UsrEmail { get; set; }

        [Required(ErrorMessage = "Не указан Пароль")]
        [DataType(DataType.Password)]
        public string UsrPassword { get; set; }
    }
}
