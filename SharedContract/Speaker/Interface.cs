using System.Collections.Generic;

namespace SharedContract.Speaker;

/// <summary>帶有<c>Speak</c>方法的物件介面。</summary>
/// <remarks>用來練習plugin使用的。</remarks>
public interface ISpeaker
{
    public string Name { get; }
    public string Speak();
}

public interface ISpeakerPlugin
{
    public string PluginName { get; }
    public IReadOnlyList<ISpeaker> GetExtendedObjects();
}