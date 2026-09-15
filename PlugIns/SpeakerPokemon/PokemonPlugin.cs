using SharedContract.Speaker;

namespace SpeakerPokemon;

public class PokemonPlugin : ISpeakerPlugin
{
    public string PluginName { get; } = "Pokemon Speaker plugin";

    public IReadOnlyList<ISpeaker> GetExtendedObjects()
    {
        return new List<ISpeaker>()
        {
            new Pikachu(),
            new Goldeen(),
        };
    }
}