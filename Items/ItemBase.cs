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
        Right_Hand,
        Left_Hand,
        Amulet,
        Quest,
        SLOT_RING1,
        SLOT_RING2,
        SLOT_CREST,
        SLOT_TRINKET,
    }

    [JsonDerivedType(typeof(WeaponBase), typeDiscriminator: "weapon")]
    [JsonDerivedType(typeof(ArmorBase), typeDiscriminator: "armor")]
    [JsonDerivedType(typeof(Potion), typeDiscriminator: "potion")]
    [JsonDerivedType(typeof(QuestItem), typeDiscriminator: "questItem")]
    public abstract class ItemBase 
    {
        public abstract string ItemName { get; protected set; }
        public int ItemLevel { get; set; }
        public Slot ItemSlot { get; set; }
        public int ItemPrice { get; set; }
        public string ItemDescription { get; set; } = "";
        public abstract int CalculateItemPrice();
        
        protected ItemBase()
        {
            ItemName = GenerateRandomName();
        }

        protected abstract string GenerateRandomName();
    }
}
