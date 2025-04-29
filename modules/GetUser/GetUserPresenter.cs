using System.Runtime.Loader;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using shared;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetUser;
public class GetUserPresenter
{
    static GetUserPresenter()
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
    
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
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

        return new APIGatewayProxyResponse()
        {
            Body = "Hello from GetUser!",
            StatusCode = 200
        };
    }
}
