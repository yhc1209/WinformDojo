using System.Security.Cryptography;
using SharedContract.Speaker;

namespace WinformDojo.PlugIns;

public class Pikachu : ISpeaker
{
    public string Name { get; } = "Pikachu";

    private static readonly string[] SPEECHES = {
        "Pika", "Pika, Pika", "Pikachu~", "PikaPi~", "PIKA PIKA!", "Pi~Ka~Chuuuuuuuuuu~~~!"
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