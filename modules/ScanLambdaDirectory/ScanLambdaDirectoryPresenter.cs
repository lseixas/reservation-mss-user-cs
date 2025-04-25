using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ScanLambdaDirectory;
public class ScanLambdaDirectoryPresenter
{
    public string FunctionHandler(string input, ILambdaContext context)
    {
        
        context.Logger.LogLine("Looking for shared assembly...");

        context.Logger.LogLine("Looking for opt");
        ListDirectoryRecursive("/opt", context);
    
        context.Logger.LogLine("Looking for dotnet");
        ListDirectoryRecursive("/dotnet", context);

        context.Logger.LogLine("Looking for .");
        ListDirectoryRecursive(".", context);

        // Try to read runtimeconfig.json if it exists
        string configPath = "./modules.runtimeconfig.json"; // Adjust filename if different
        if (File.Exists(configPath))
        {
            context.Logger.LogLine($"[FOUND CONFIG] {configPath}");
            string configContent = File.ReadAllText(configPath);
            context.Logger.LogLine("[CONFIG CONTENT]");
            context.Logger.LogLine(configContent);
        }
        else
        {
            context.Logger.LogLine("[CONFIG NOT FOUND] ./modules.runtimeconfig.json");
        }
        
        return "NUNCA CONFIE EM CARECAS";

    }
    void ListDirectoryRecursive(string path, ILambdaContext context)
    {
        if (!Directory.Exists(path))
        {
            context.Logger.LogLine($"Directory does not exist: {path}");
            return;
        }

        // log this directory itself
        context.Logger.LogLine($"[DIR] {path}");

        // 1️⃣  log every file in the current directory
        foreach (var file in Directory.GetFiles(path))
        {
            context.Logger.LogLine($"[FILE] {file}");
        }

        // 2️⃣  recurse into every sub‑directory
        foreach (var dir in Directory.GetDirectories(path))
        {
            ListDirectoryRecursive(dir, context);
        }
    }
}