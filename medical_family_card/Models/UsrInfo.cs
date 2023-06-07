using System;
using System.Collections.Generic;

namespace medical_family_card.Models;

public partial class UsrInfo
{
    public int UsrId { get; set; }

    public string UsrInfoFirstName { get; set; } = null!;

    public string UsrInfoLastName { get; set; } = null!;

    public string UsrInfoMiddleName { get; set; } = null!;

    public DateTime UsrInfoBirthday { get; set; }

    public int GenderId { get; set; }

    public int BloodId { get; set; }

    public int RhesusFactorId { get; set; }

    public string? ImgName { get; set; } = null!;

    public byte[]? ImgData { get; set; } = null!;

    public virtual Blood? Blood { get; set; } = null!;

    public virtual Gender? Gender { get; set; } = null!;

    public virtual RhesusFactor? RhesusFactor { get; set; } = null!;

    public virtual Usr? Usr { get; set; } = null!;
}
