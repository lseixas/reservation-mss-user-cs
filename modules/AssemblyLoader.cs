using System.Reflection;

namespace modules;

public static class AssemblyLoader
{
    private static readonly Assembly? sharedAssembly;

    static AssemblyLoader()
    {
        string dllPath = "/opt/shared.dll"; // Adjust path as needed
        if (System.IO.File.Exists(dllPath))
        {
            sharedAssembly = Assembly.LoadFrom(dllPath);
            Console.WriteLine($"Loaded assembly: {sharedAssembly.FullName}");
        }
        else
        {
            Console.WriteLine("shared.dll not found at /opt/");
        }
    }

    public static Type? GetType(string typeName)
    {
        return sharedAssembly?.GetType(typeName);
    }

    public static object? CreateInstance(string typeName)
    {
        var type = GetType(typeName);
        return type != null ? Activator.CreateInstance(type) : null;
    }
}