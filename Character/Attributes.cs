
namespace Saga.Character
{
    public class Attributes
    {
        private readonly Player _player;

        //Primary attributes
        public int Strength { get; private set; }
        public int Dexterity { get; private set; }
        public int Intellect { get; private set; }

        private int _constitution;
        private int _willPower;
        private int _awareness;
        private int _virtue;

        //Secondary attributes
        public int Constitution { get; private set; }
        public int WillPower { get; private set; }
        public int Awareness { get; private set; }
        public int Virtue { get; private set; }

        public event Action? AttributesChanged;

        public Attributes(Player player) {
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
