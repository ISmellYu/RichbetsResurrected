﻿using RichbetsResurrected.Entities.DatabaseEntities.Shop;

namespace RichbetsResurrected.Entities.DatabaseEntities.BaseRichbet;

public class RichbetUser
{
    public int Id { get; set; }
    public int Points { get; set; }
    public float Multiplier { get; set; }
    public bool DailyRedeemed { get; set; }
    
    public ICollection<ActiveItem> ActiveItems { get; set; }
    public ICollection<UserItem> UserItems { get; set; }
}