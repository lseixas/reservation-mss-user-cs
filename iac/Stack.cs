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

        private BundlingOptions newBundlingOptions(string moduleName)
        {
            return new BundlingOptions()
            {
                Image = Runtime.DOTNET_8.BundlingImage,
                User = "root",
                OutputType = BundlingOutput.ARCHIVED,
                Command = new String[]
                {
                    "/bin/sh",
                    "-c",
                    $"cd modules/{moduleName} && " +
                    "dotnet tool install -g Amazon.Lambda.Tools" +
                    " && dotnet build" +
                    " && dotnet lambda package --output-package /asset-output/function.zip"
                }
            };
        }
        
        internal StackClass(Construct scope, string id, StackProps? props = null) : base(scope, id, props)
        {

            var sharedLayer = new LayerVersion(this, "ReservationMssUserCsLayer", new LayerVersionProps
            {
                CompatibleRuntimes = new[] { Runtime.DOTNET_8 },
                Code = Code.FromAsset("./layer-package"),
                Description = "Lambda Layer for reservation csharp rebuild project",
                RemovalPolicy = RemovalPolicy.DESTROY,
            });

            var GetUserLambdaFunction = new Function(this, "GetUserFunction", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                MemorySize = 1024,
                LogRetention = RetentionDays.ONE_DAY,
                Handler = "GetUserAssembly::modules.GetUser.GetUserPresenter::FunctionHandler",
                Code = Code.FromAsset(".", new AssetOptions()
                {
                    Bundling = newBundlingOptions(moduleName: "GetUser")
                }),
                Layers = new[] { sharedLayer }
            });
        }
    }
}