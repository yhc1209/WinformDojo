using System;
using System.Reflection;
using System.Runtime.Loader;
using SharedContract.Speaker;

namespace WinformDojo.Plugin;

public class SpeakerLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public SpeakerLoadContext(string dllpath) : base(isCollectible: true)
    {
        _resolver = new AssemblyDependencyResolver(dllpath);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
    }
}