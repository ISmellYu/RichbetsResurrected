﻿using System.Text.Json.Serialization;

namespace RichbetsResurrected.Entities.DatabaseEntities.Shop;

public class ItemType
{
    public bool IsActivatable { get; set; }
    public bool IsEquippable { get; set; }
    public bool IsConsumable { get; set; }
    public bool IsCraftingMaterial { get; set; }
    public bool IsNicknameBanner { get; set; }
    public bool IsNicknameEffect { get; set; }
    public bool IsNicknamePattern { get; set; }
    public bool IsNicknameAnimation { get; set; }
    public bool IsUnique { get; set; }
    
    
    [JsonIgnore]
    public int ItemId { get; set; }
    [JsonIgnore]
    public Item Item { get; set; }
}