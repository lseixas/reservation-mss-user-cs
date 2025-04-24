using Amazon.Lambda.Core;
using opt.dotnet.shared;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetUser;
public class GetUserPresenter
{
    public string FunctionHandler(string input, ILambdaContext context)
    {
        try
        {
            LayerClass layerClass = new LayerClass(5);
            context.Logger.LogLine("Successfully created LayerClass");
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error creating LayerClass: {ex.Message}");
            context.Logger.LogLine($"Stack Trace: {ex.StackTrace}");
        }

        return "TS NOT WORK";
    }
}
