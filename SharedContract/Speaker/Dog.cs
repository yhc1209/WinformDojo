using System.Security.Cryptography;

namespace SharedContract.Speaker;

public class Dog : ISpeaker
{
    public string Name { get; } = "Dog";

    private static readonly string[] SPEECHES = {
        "Want.", "Want~", "Want, want.", "Want...", "WANT!",
        "Want!", "Want! want!", "Want! want! want!",
        "Want! Want and want!", "Want! Want and want! ...want?!",
        "Want! Want! Want! Want and want! ...want?!",
        "Want! Want! Want! Want! Want! Want!", "WAAAAAAAAAAANNT!!!!"
    };

    public string Speak()
    {
        int idx = RandomNumberGenerator.GetInt32(SPEECHES.Length);
        return SPEECHES[idx];
    }

    public override string ToString()
    {
        return "Dog";
    }
}

