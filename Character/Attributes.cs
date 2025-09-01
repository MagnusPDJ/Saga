using System.Text.Json.Serialization;

namespace Saga.Character
{
    public class Attributes
    {
        [JsonIgnore]
        private Player? _player;

        //Primary attributes
        [JsonInclude]
        public int Strength { get; private set; }
        [JsonInclude]
        public int Dexterity { get; private set; }
        [JsonInclude]
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
        [JsonInclude]
        public int Constitution { get; private set; }
        [JsonInclude]
        public int WillPower { get; private set; }
        [JsonInclude]
        public int Awareness { get; private set; }
        [JsonInclude]
        public int Virtue { get; private set; }

        public event Action? AttributesChanged;

        public Attributes(Player player, int strength, int dexterity, int intellect) {
            Strength = strength;
            Dexterity = dexterity;
            Intellect = intellect;

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
            Constitution = CalculateConstiution();
            WillPower = CalculateWillPower();
            Awareness = CalculateAwareness();
            Virtue = CalculateVirtue();
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
        public void AddValues(int strenght = 0, int dexterity = 0, int intellect = 0, int constitution = 0, int willPower = 0, int awareness = 0, int virtue = 0) {
            Strength += strenght;
            Dexterity += dexterity;
            Intellect += intellect;
            _constitution += constitution;
            _willPower += willPower;
            _awareness += awareness;
            _virtue += virtue;
            AttributesChanged?.Invoke();
        }
    }
}
