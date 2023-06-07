using System;
using System.Collections.Generic;

namespace medical_family_card.Models;

public partial class Blood
{
    public int BloodId { get; set; }

    public string BloodName { get; set; } = null!;

    public virtual ICollection<UsrInfo> UsrInfos { get; set; } = new List<UsrInfo>();
}
