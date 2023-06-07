using System.ComponentModel.DataAnnotations;

namespace medical_family_card.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указано Имя пользователя")]
        public string UsrName { get; set; }

        [Required(ErrorMessage = "Не указана Электронная почта")]
        public string UsrEmail { get; set; }

        [Required(ErrorMessage = "Не указан Пароль")]
        [DataType(DataType.Password)]
        public string UsrPassword { get; set; }

        [Required(ErrorMessage = "Подтверждение Пароля введено неверно")]
        [DataType(DataType.Password)]
        public string ConfirmUsrPassword { get; set; }
    }
}
