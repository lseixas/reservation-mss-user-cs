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
        try 
        {
            // This will help us see what's happening
            string[] files = System.IO.Directory.GetFiles("/opt/dotnet/shared/");
            foreach (string file in files)
            {
                context.Logger.LogLine($"Found file: {file}");
            }
        
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error: {ex.Message}");
            context.Logger.LogLine($"Stack: {ex.StackTrace}");
            return $"Error: {ex.Message}";
        }
        
    }
}
