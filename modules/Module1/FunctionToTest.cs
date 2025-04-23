using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace modules.Module1;

public class FunctionToTest
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

        shared.LayerClass layerClass = new shared.LayerClass(5);
        
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
