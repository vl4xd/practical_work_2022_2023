using System;
using System.Collections.Generic;

namespace medical_family_card.Models;

public partial class Gender
{
    public int GenderId { get; set; }

    public string GenderName { get; set; } = null!;

    public virtual ICollection<UsrInfo> UsrInfos { get; set; } = new List<UsrInfo>();
}
