using Saga.Character;
using System;

namespace Saga.Items
{
    //Lists all available armor types
    public enum ArmorType
    {
        ARMOR_CLOTH,
        ARMOR_LEATHER,
        ARMOR_MAIL,
        ARMOR_PLATE
    }
    [Serializable]
    public class Armor : Item
    {
        public ArmorType ArmorType { get; set; }
        public PrimaryAttributes Attributes { get; set; }

        /// <inheritdoc/>
        public override string ItemDescription() {
            return $"Armor of type {ArmorType}";
        }
    }
}
