﻿using Saga.Character;
using System;

namespace Saga.Items
{
    public enum PotionType
    {
        POTION_HEALING,
        POTION_MANA
    }
    [Serializable]
    public class Potion : Item
    {
        public int PotionPotency { get; set; }
        public int PotionQuantity { get; set; }
        public PotionType PotionType { get; set; }
        public override string ItemDescription() {
            return $"Potion of type {PotionType}";
        }
        public Potion() {
            ItemName = "Healing Potion";
            ItemLevel = 1;
            PotionType = PotionType.POTION_HEALING;
            PotionPotency = 5;
            PotionQuantity = 5;
        }
        public override int CalculateItemPrice() {
            ItemPrice = 20+10*Program.CurrentPlayer.Level;
            return ItemPrice;
        }
    }
}
