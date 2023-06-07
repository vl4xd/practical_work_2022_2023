using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace medical_family_card.Models;

public partial class Wt
{
    public int WtId { get; set; }

    public int UsrId { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    public double WtValue { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    public DateTime WtDate { get; set; }

    public virtual Usr? Usr { get; set; } = null!;
}
