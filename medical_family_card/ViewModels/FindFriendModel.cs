using System.ComponentModel.DataAnnotations;

namespace medical_family_card.ViewModels
{
    public class FindFriendModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public string UserNameTo { get; set; }
    }
}
