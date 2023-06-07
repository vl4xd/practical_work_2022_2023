using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace medical_family_card.Models;

public partial class Ht
{
    public int HtId { get; set; }

    public int UsrId { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    public int HtValue { get; set; }

    [Required(ErrorMessage = "Обязательное поле")]
    public DateTime HtDate { get; set; }

    public virtual Usr? Usr { get; set; }
}
