using System;
using System.Collections.Generic;

namespace medical_family_card.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int UsrId { get; set; }

    public string VisitName { get; set; } = null!;

    public DateTime VisitStartDate { get; set; }

    public DateTime VisitEndDate { get; set; }

    public double VisitCost { get; set; }

    public string VisitComment { get; set; } = null!;

    public virtual ICollection<ImgVisit> ImgVisits { get; set; } = new List<ImgVisit>();

    public virtual Usr? Usr { get; set; } = null!;
}
