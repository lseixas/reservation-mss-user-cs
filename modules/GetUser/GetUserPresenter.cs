using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetUser;
public class GetUserPresenter
{
    public string FunctionHandler(string input, ILambdaContext context)
    {
        try
        {
            shared.LayerClass layerClass = new shared.LayerClass(5);
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
