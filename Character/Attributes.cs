using System.Text.Json.Serialization;

namespace Saga.Character
{
    public class Attributes
    {
        [JsonIgnore]
        private Player? _player;

        //Primary attributes
        [JsonInclude]
        private int _strength;
        [JsonInclude]
        private int _dexterity;
        [JsonInclude]
        private int _intellect;
        public int Strength { get; private set; }
        public int Dexterity { get; private set; }
        public int Intellect { get; private set; }
        [JsonInclude]
        private int _constitution;
        [JsonInclude]
        private int _willPower;
        [JsonInclude]
        private int _awareness;
        [JsonInclude]
        private int _virtue;


        //Secondary attributes
        public int Constitution { get; private set; }
        public int WillPower { get; private set; }
        public int Awareness { get; private set; }
        public int Virtue { get; private set; }

        public event Action? AttributesChanged;

        public Attributes(Player player, int strength, int dexterity, int intellect) {
            _strength = strength;
            _dexterity = dexterity;
            _intellect = intellect;

            AttachToPlayer(player);
        }

        // Used by Json Deserialization.
        [JsonConstructor]
        private Attributes() { }
        public void AttachToPlayer(Player player) {
            _player = player;
            _player.PlayerChanged += RecalculateAttributes;
            RecalculateAttributes();
        }
        public void RecalculateAttributes() {
            Strength = _strength + _player!.Equipment.BonusStr;
            Dexterity = _dexterity + _player!.Equipment.BonusDex;
            Intellect = _intellect + _player!.Equipment.BonusInt;
            Constitution = CalculateConstiution() + _player!.Equipment.BonusCon;
            WillPower = CalculateWillPower() + _player!.Equipment.BonusWP;
            Awareness = CalculateAwareness() + _player!.Equipment.BonusAwa;
            Virtue = CalculateVirtue() + _player!.Equipment.BonusVirtue;
        }
        int CalculateConstiution() {
            return _constitution + (Strength + Dexterity) / 2;
        }
        int CalculateWillPower() {
            return _willPower + (Strength + Intellect) / 2;
        }
        int CalculateAwareness() {
            return _awareness + (Dexterity + Intellect) / 2;
        }
        int CalculateVirtue() {
            List<int> list = [Strength, Constitution, WillPower];
            return _virtue + list.Min();
        }
        public void AddValues(int strength = 0, int dexterity = 0, int intellect = 0, int constitution = 0, int willPower = 0, int awareness = 0, int virtue = 0) {
            _strength += strength;
            _dexterity += dexterity;
            _intellect += intellect;
            _constitution += constitution;
            _willPower += willPower;
            _awareness += awareness;
            _virtue += virtue;
            AttributesChanged?.Invoke();
        }
    }
}
