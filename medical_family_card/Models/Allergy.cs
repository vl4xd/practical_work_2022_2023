using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace medical_family_card.Models;

public partial class Allergy
{
    public int AllergyId { get; set; }

    public int UsrId { get; set; }

    [Required(ErrorMessage = "Название аллергии обязательно")]
    public string AllergyName { get; set; } = null!;

    public string? AllergyComment { get; set; } = null!;

    public virtual Usr? Usr { get; set; } = null!;
}
