using Saga.Character;
using System.Text.Json.Serialization;

namespace Saga.Assets
{
    [JsonDerivedType(typeof(Go), typeDiscriminator: "go")]
    [JsonDerivedType(typeof(Examine), typeDiscriminator: "examine")]
    [JsonDerivedType(typeof(Equip), typeDiscriminator: "equip")]
    [JsonDerivedType(typeof(Use), typeDiscriminator: "use")]
    public abstract class InputAction(string keyWord, string abrKeyWord = null)
    {
        public string KeyWord { get; set; } = keyWord;
        public string AbrKeyWord { get; set; } = abrKeyWord;

        public abstract void RespondToInput(Player player, string[] separatedInputWords = null);
    }

    public class Go(string keyWord) : InputAction(keyWord)
    {
        public override void RespondToInput(Player player, string[] separatedInputWords) {
            //not implemented
        }
    }
    public class Examine(string keyWord) : InputAction(keyWord)
    {
        public override void RespondToInput(Player player, string[] separatedInputWords) {
            //not implemented
        }
    }
    public class Equip(string keyWord) : InputAction(keyWord)
    {
        public override void RespondToInput(Player player, string[] separatedInputWords) {
            //not implemented
        }
    }
    public class Use(string keyWord) : InputAction(keyWord)
    {
        public override void RespondToInput(Player player, string[] separatedInputWords) {
            //not implemented
        }
    }
}

