using System.Text.Json.Serialization;

namespace Saga.Items
{
    public enum Slot 
    {
        Headgear,
        Torso,
        Legs,        
        Feet,
        Bracers,
        Shoulders,
        Belt,
        Cape,
        Gloves,
        Weapon,
        Amulet,
        Quest,
        SLOT_RING1,
        SLOT_RING2,
        SLOT_CREST,
        SLOT_TRINKET,
        SLOT_OFFHAND,
    }

    [JsonDerivedType(typeof(Weapon), typeDiscriminator: "weapon")]
    [JsonDerivedType(typeof(Armor), typeDiscriminator: "armor")]
    [JsonDerivedType(typeof(Potion), typeDiscriminator: "potion")]
    [JsonDerivedType(typeof(QuestItem), typeDiscriminator: "questItem")]
    public abstract class Item 
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public Slot ItemSlot { get; set; }
        public int ItemPrice { get; set; }
        public abstract int CalculateItemPrice();      
    }
}
