using System.ComponentModel;
using Constructs;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.AppConfig;
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

            var restApi = new RestApi(this, "ReservationMssUserCsRestApi", new RestApiProps
            {
                RestApiName = "ReservationMssUserCsRestApi",
                Description = "API for ReservationMssUserCs",
                DefaultCorsPreflightOptions = new CorsOptions
                {
                    AllowOrigins = Cors.ALL_ORIGINS,
                    AllowMethods = Cors.ALL_METHODS,
                    AllowHeaders = Cors.DEFAULT_HEADERS,
                }
            });

            var apiGatewayResource = restApi.Root.AddResource("reservation-mss-user-cs", new RestApiProps
            {
                DefaultCorsPreflightOptions = new CorsOptions
                {
                    AllowOrigins = Cors.ALL_ORIGINS,
                    AllowMethods = Cors.ALL_METHODS,
                    AllowHeaders = Cors.DEFAULT_HEADERS,
                }
            });

            var sharedLayer = new LayerVersion(this, "ReservationMssUserCsLayer", new LayerVersionProps
            {
                CompatibleRuntimes = new[] { Runtime.DOTNET_8 },
                Code = Code.FromAsset("./shared", new AssetOptions()
                {
                    Bundling = new BundlingOptions
                    {
                        Image = Runtime.DOTNET_8.BundlingImage,
                        User = "root",
                        OutputType = BundlingOutput.ARCHIVED,
                        Command = new [] {
                            "/bin/sh", "-c",
                            // 1) publica no formato de runtime store em /asset-output
                            "dotnet tool install -g Amazon.Lambda.Tools && " +
                            "dotnet lambda publish-layer --layer-type runtime-package-store " +
                            "--layer-name ReservationMssUserCsLayer --framework net8.0 " +
                            "--region sa-east-1 " +  
                            "--s3-bucket teste-layer-reservation-mss-user-cs " +
                            "--output-package /asset-output/layer.zip"
                        }
                    }
                }),
                Description = "Lambda Layer for reservation csharp rebuild project",
                RemovalPolicy = RemovalPolicy.DESTROY,
            });

            var getUserLambdaFunction = new Function(this, "GetUserFunction", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                MemorySize = 1024,
                LogRetention = RetentionDays.ONE_DAY,
                Handler = "GetUserAssembly::GetUser.GetUserPresenter::FunctionHandler",
                Code = Code.FromAsset(".", new AssetOptions()
                {
                    Bundling = newBundlingOptions(moduleName: "GetUser")
                }),
                Environment = new Dictionary<string, string>
                {
                    {"DOTNET_SHARED_STORE", "/opt/runtime-package-store/"}
                },
                Layers = new[] { sharedLayer }
            });

            getUserLambdaFunction.ApplyRemovalPolicy(RemovalPolicy.DESTROY);
            apiGatewayResource.AddResource("get-user").AddMethod("GET", integration: new LambdaIntegration(getUserLambdaFunction));

            var scanLamdaDirectoryFunction = new Function(this, "ScanLambdaDirectoryFunction", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                MemorySize = 1024,
                LogRetention = RetentionDays.ONE_DAY,
                Handler =
                    "ScanLambdaDirectoryAssembly::ScanLambdaDirectory.ScanLambdaDirectoryPresenter::FunctionHandler",
                Code = Code.FromAsset(".", new AssetOptions()
                {
                    Bundling = newBundlingOptions(moduleName: "ScanLambdaDirectory")
                }),
                Layers = new[] { sharedLayer }
            });
            
            scanLamdaDirectoryFunction.ApplyRemovalPolicy(RemovalPolicy.DESTROY);
            
        }
    }
}