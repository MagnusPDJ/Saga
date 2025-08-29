using System.Text.Json.Serialization;

namespace Saga.Items
{
    [JsonDerivedType(typeof(QuestItemBase), typeDiscriminator: "questItemBase")]
    [JsonDerivedType(typeof(ArmorBase), typeDiscriminator: "armorBase")]
    [JsonDerivedType(typeof(OneHandedSword), typeDiscriminator: "oneHandedSword")]
    [JsonDerivedType(typeof(TwoHandedSword), typeDiscriminator: "twoHandedSword")]
    [JsonDerivedType(typeof(OneHandedAxe), typeDiscriminator: "oneHandedAxe")]
    [JsonDerivedType(typeof(OneHandedMace), typeDiscriminator: "oneHandedMace")]    
    [JsonDerivedType(typeof(Wand), typeDiscriminator: "wand")]
    [JsonDerivedType(typeof(Staff), typeDiscriminator: "staff")]
    [JsonDerivedType(typeof(Tome), typeDiscriminator: "tome")]
    [JsonDerivedType(typeof(Bow), typeDiscriminator: "bow")]
    [JsonDerivedType(typeof(Dagger), typeDiscriminator: "dagger")]
    [JsonDerivedType(typeof(Crossbow), typeDiscriminator: "crossbow")]
    [JsonDerivedType(typeof(HealingPotion), typeDiscriminator: "healingPotion")]
    public abstract class ItemBase : IItem
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        private int _itemPrice;
        public int ItemPrice => _itemPrice;
        public string ItemDescription { get; init; }
        
        public void SetItemPrice() => _itemPrice = CalculateItemPrice();
        public abstract int CalculateItemPrice();
    }
}
