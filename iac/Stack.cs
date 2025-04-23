using Constructs;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;

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
        }
    }
}