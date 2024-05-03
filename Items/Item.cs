using Saga.Character;
using System;

namespace Saga.Items
{
    public enum Slot
    {
        SLOT_HEADER,
        SLOT_BODY,
        SLOT_LEGS,
        SLOT_WEAPON,
        SLOT_FEET,
        SLOT_ARMS,
        SLOT_SHOULDERS,
        SLOT_BELT,
        SLOT_CAPE,
        SLOT_GLOVES,
        SLOT_AMULET,
        SLOT_RING1,
        SLOT_RING2,
        SLOT_CREST,
        SLOT_TRINKET,
        SLOT_OFFHAND,
        SLOT_POTION
    }
    [Serializable]
    public abstract class Item
    {
        public string ItemName { get; set; }
        public int ItemLevel { get; set; }
        public Slot ItemSlot { get; set; }

        // Displays the type of item.
        public abstract string ItemDescription();

    }
}
