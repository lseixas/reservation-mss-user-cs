using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace common;

public static class AssemblyLoader
{
    public static void Initialize()
    {
        AssemblyLoadContext.Default.Resolving += (loadContext, assemblyName) =>
        {
            var path = $"/opt/dotnetcore/store/{assemblyName.Name}.dll";
            if (File.Exists(path))
            {
                return loadContext.LoadFromAssemblyPath(path);
            }

            return null;
        };
    }

}
