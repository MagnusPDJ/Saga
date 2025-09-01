using Saga.Items;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Saga.Character
{
    public class Equipment : IEnumerable<KeyValuePair<string, IEquipable?>>
    {
        public IEquipable? Headgear { get; set; }
        public IEquipable? Torso { get; set; }
        public IEquipable? Legs { get; set; }
        public IEquipable? Feet { get; set; }
        public IEquipable? Bracers { get; set; }
        public IEquipable? Shoulders { get; set; }
        public IEquipable? Belt {  get; set; }
        public IEquipable? Cape {  get; set; }
        public IEquipable? Gloves { get; set; }
        public IEquipable? Right_Hand { get; set; }
        public IEquipable? Left_Hand { get; set; }
        public IEquipable? Amulet { get; set; }
        public IEquipable? Finger_1 { get; set; }
        public IEquipable? Finger_2 { get; set; }
        public IEquipable? Crest { get; set; }
        public IEquipable? Trinket { get; set; }
        public IEquipable? Potion { get; set; }

        public IEnumerator<KeyValuePair<string, IEquipable?>> GetEnumerator() {
            foreach (var prop in typeof(Equipment).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                // Check if property type implements IEquipable
                if (typeof(IEquipable).IsAssignableFrom(prop.PropertyType)) {
                    yield return new KeyValuePair<string, IEquipable?>(
                        prop.Name,
                        (IEquipable?)prop.GetValue(this) // cast to nullable
                    );
                }
            }
        }
        public bool TryGetSlot(Slot slot, [NotNullWhen(true)] out IEquipable? value) {
            var prop = typeof(Equipment).GetProperty(slot.ToString());
            value = (IEquipable?)prop?.GetValue(this);
            return value is not null;
        }
        public void SetSlot(Slot slot, IEquipable? item) {
            var prop = typeof(Equipment).GetProperty(slot.ToString());
            if (prop != null && typeof(IEquipable).IsAssignableFrom(prop.PropertyType)) {
                prop.SetValue(this, item);
            } else {
                throw new ArgumentException($"Invalid slot {slot}");
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
