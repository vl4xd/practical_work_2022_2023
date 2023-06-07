using System;
using System.Collections.Generic;

namespace medical_family_card.Models;

public partial class RhesusFactor
{
    public int RhesusFactorId { get; set; }

    public string RhesusFactorName { get; set; } = null!;

    public virtual ICollection<UsrInfo> UsrInfos { get; set; } = new List<UsrInfo>();
}
