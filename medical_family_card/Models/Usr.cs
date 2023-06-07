using System;
using System.Collections.Generic;

namespace medical_family_card.Models;

public partial class Usr
{
    public int UsrId { get; set; }

    public string UsrEmail { get; set; } = null!;

    public string UsrPassword { get; set; } = null!;

    public string UsrName { get; set; } = null!;

    public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();

    public virtual ICollection<Ht> Hts { get; set; } = new List<Ht>();

    public virtual UsrInfo? UsrInfo { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public virtual ICollection<Wt> Wts { get; set; } = new List<Wt>();
}
