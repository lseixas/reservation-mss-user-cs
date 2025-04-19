using Constructs;

namespace iac;

using Amazon.CDK.AWS.Lambda;
using Amazon.CDK;

public class LambdaStack : Construct
{

    private readonly ILayerVersion lambdaLayer;
    private readonly ILayerVersion lambdaPowerTools;
    private readonly string lambdaRegion;

    public LambdaStack(Construct scope, Dictionary<string, string> environmentVariables) : base(scope, "ReservationMssUserCs")
    {
        var lambdaLayer = new LayerVersion(this, "ReservationMssUserCs_Layer", new LayerVersionProps
        {
            Code = Code.FromAsset("../src/shared"),
            CompatibleRuntimes = new[] { Runtime.DOTNET_8 },
            Description = "ReservationMssUserCs_Layer .NET 8"
        });
        
        lambdaRegion = environmentVariables.GetValueOrDefault("REGION", "sa-east-1");
        
        var printLambdaFunction = CreateLambdaFunction(
            "PrintLambdaHandler", 
            "print_lambda", 
            "get"
            );
            
    }

    private Function CreateLambdaFunction(string handlerName, string moduleName, string methodName)
    {
        return new Function(this, moduleName, new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            Code = Code.FromAsset("../src/modules/print_lambda"),
            Handler = "app." + moduleName + "::" + handlerName + "::FunctionHandler",
            FunctionName = methodName.ToUpper(),
            MemorySize = 256,
            Timeout = Duration.Seconds(30)
        });
    }
}