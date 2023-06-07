using System;
using System.Collections.Generic;

namespace medical_family_card.Models;

public partial class ImgVisit
{
    public int ImgId { get; set; }

    public int VisitId { get; set; }

    public string ImgName { get; set; } = null!;

    public byte[] ImgData { get; set; } = null!;

    public virtual Visit? Visit { get; set; } = null!;
}
