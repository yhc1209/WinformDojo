using System.Security.Cryptography;

namespace SharedContract.Speaker;

public class Cat : ISpeaker
{
    public string Name { get; } = "Cat";

    private static readonly string[] SPEECHES = {
        "Meow~", "Meow.", "Meeeoow...", "Meow, meow..."
    };

    public string Speak()
    {
        int idx = RandomNumberGenerator.GetInt32(SPEECHES.Length);
        return SPEECHES[idx];
    }

    public override string ToString()
    {
        return "Cat";
    }
}