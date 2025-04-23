using Constructs;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace iac;

public class Stack
{
    public class StackClass : Amazon.CDK.Stack
    {
        internal StackClass(Construct scope, string id, StackProps? props = null) : base(scope, id, props)
        {
            var buildOption = new BundlingOptions()
            {
                Image = Runtime.DOTNET_8.BundlingImage,
                User = "root",
                OutputType = BundlingOutput.ARCHIVED,
                Command = new String[]
                {
                    "/bin/sh",
                    "-c",
                    "dotnet tool install -g Amazon.Lambda.Tools" +
                    " && dotnet build" +
                    " && dotnet lambda package --output-package /asset-output/function.zip"
                }
            };

            var sharedLayer = new LayerVersion(this, "ReservationMssUserCsLayer", new LayerVersionProps
            {
                CompatibleRuntimes = new[] { Runtime.DOTNET_8 },
                Code = Code.FromAsset("./shared/bin/Release/net8.0/publish"),
                Description = "Lambda Layer for reservation csharp rebuild project",
                RemovalPolicy = RemovalPolicy.DESTROY,
            });
            
            var helloWorldLambdaFunction = new Function(this, "ReservationMssUserCsFunction", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                MemorySize = 1024,
                LogRetention = RetentionDays.ONE_DAY,
                Handler = "modules::FunctionToTest.Function::FunctionHandler",
                Code = Code.FromAsset("./modules/FunctionToTest", new AssetOptions()
                {
                    Bundling = buildOption
                }),
                Layers = new[] { sharedLayer }
            });
        }
    }
}