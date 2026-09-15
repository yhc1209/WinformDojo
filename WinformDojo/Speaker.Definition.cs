using System.Collections.Generic;

namespace WinformDojo;

/// <summary>帶有<c>Speak</c>方法的物件介面。</summary>
/// <remarks>用來練習plugin使用的。</remarks>
public interface ISpeaker
{
    public string Name { get; }
    public string Speak();
}

public interface ISpeakerPlugin
{
    public string Name { get; }
    public IReadOnlyList<ISpeaker> GetExtendedObjects();
}