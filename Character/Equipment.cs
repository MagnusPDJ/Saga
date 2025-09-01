using Saga.Items;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Saga.Character
{  
    public class Equipment
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

        /// <summary>
        /// Enumerates all slots as key/value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEquipable?>> AsEnumerable() {
            yield return new(nameof(Headgear), Headgear);
            yield return new(nameof(Torso), Torso);
            yield return new(nameof(Legs), Legs);
            yield return new(nameof(Feet), Feet);
            yield return new(nameof(Bracers), Bracers);
            yield return new(nameof(Shoulders), Shoulders);
            yield return new(nameof(Belt), Belt);
            yield return new(nameof(Cape), Cape);
            yield return new(nameof(Gloves), Gloves);
            yield return new(nameof(Right_Hand), Right_Hand);
            yield return new(nameof(Left_Hand), Left_Hand);
            yield return new(nameof(Amulet), Amulet);
            yield return new(nameof(Finger_1), Finger_1);
            yield return new(nameof(Finger_2), Finger_2);
            yield return new(nameof(Crest), Crest);
            yield return new(nameof(Trinket), Trinket);
            yield return new(nameof(Potion), Potion);
        }

        /// <summary>
        /// Tries to get an item in a slot by enum.
        /// </summary>
        public bool TryGetSlot(Slot slot, [NotNullWhen(true)] out IEquipable? value) {
            var prop = GetType().GetProperty(slot.ToString());
            value = (IEquipable?)prop?.GetValue(this);
            return value is not null;
        }

        /// <summary>
        /// Sets an item into a slot by enum.
        /// </summary>
        public void SetSlot(Slot slot, IEquipable? item) {
            var prop = GetType().GetProperty(slot.ToString());
            if (prop != null && typeof(IEquipable).IsAssignableFrom(prop.PropertyType)) {
                prop.SetValue(this, item);
            } else {
                throw new ArgumentException($"Invalid slot {slot}");
            }
        }

        /// <summary>
        /// Convenience iterator to use foreach if desired.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEquipable?>> GetSlots() => AsEnumerable();
    }
}
