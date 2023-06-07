using System;
using System.Collections.Generic;

namespace medical_family_card.Models;

public partial class Friend
{
    public int FriendId { get; set; }
    public int FromUsrId { get; set; }

    public int ToUsrId { get; set; }

    public int FriendTypeId { get; set; }

    public virtual FriendType? FriendType { get; set; } = null!;

    public virtual Usr? FromUsr { get; set; } = null!;

    public virtual Usr? ToUsr { get; set; } = null!;
}
