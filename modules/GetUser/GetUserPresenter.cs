using Amazon.Lambda.Core;
namespace GetUser;

public class GetUserPresenter
{
    public string FunctionHandler(ILambdaContext context, string input)
    {

        var instance = Common.AssemblyLoader.CreateInstance("LayerAssembly");
        
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

        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            context.Logger.LogLine($"[FOUND FILE] {file}");
        }
    }
}
