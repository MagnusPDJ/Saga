using System;

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
    [Serializable]
    public abstract class Item 
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public Slot ItemSlot { get; set; }
        public int ItemPrice { get; set; }
        public abstract int CalculateItemPrice();      
    }
}
