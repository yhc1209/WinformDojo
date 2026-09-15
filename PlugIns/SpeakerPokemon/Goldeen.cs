using System.Security.Cryptography;
using SharedContract.Speaker;

namespace SpeakerPokemon;

public class Goldeen : ISpeaker
{
    public string Name { get; } = "Goldeen";

    private static readonly string[] SPEECHES = {
        "角金魚", "角金魚 角金魚"
    };

    public string Speak()
    {
        int idx = RandomNumberGenerator.GetInt32(SPEECHES.Length);
        return SPEECHES[idx];
    }

    public override string ToString()
    {
        return Name;
    }
}