using Saga.Character.DmgLogic;

namespace Saga.Character.Buffs
{
    public interface IBuff
    {
        string Name { get; }
        int Duration { get; }
        int RemainingTurns { get; set; }
        
        int Strength { get; }
        int Dexterity { get; }
        int Intellect { get;  }
        int Constitution { get; }
        int Willpower { get; }
        int Awareness { get; }
        int Virtue { get; }
        Dictionary<PhysicalType, int> PhysicalRes { get; }   
        Dictionary<ElementalType, int> ElementalRes { get; } 
        Dictionary<MagicalType, int> MagicalRes { get; } 
        int Initiative { get; }
        int Health { get; }
        int Mana { get; }
        int ManaRegenRate { get; }
        int ActionPoints { get; }
        int AttackSpeed { get; }
        int CastingSpeed { get; }
        int ArmorRating { get; }

        void OnApply(Player player);
        void OnRemove(Player player);
        public IEnumerable<KeyValuePair<string, int>> AsEnumerable();
    }
}
