﻿
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
        Finger_1,
        Finger_2,
        Crest,
        Trinket,
    }
    internal interface IEquipable
    {
        Slot ItemSlot { get; set; }
        void Equip();
        void UnEquip();
    }
}
