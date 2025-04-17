using Amazon.CDK.AWS.APIGateway;

namespace iac;

using Amazon.CDK;
using Constructs;

public class IacStack : Stack
{
    public IacStack(Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
    {
        // The code that defines your stack goes here
        // For example, you can create an S3 bucket:
        // new Bucket(this, "MyFirstBucket", new BucketProps
        // {
        //     Versioned = true
        // });

        var restApi = new RestApi(this, "Test_Stack_Dev", new RestApiProps
        {
            RestApiName = "Test_Stack_Dev",
            Description = "This is a test stack for dev environment",
            DefaultCorsPreflightOptions = new CorsOptions
            {
                AllowOrigins = Cors.ALL_ORIGINS,
                AllowMethods = new[]
                {
                    "GET"
                },
                AllowHeaders = new[] { "*" }
            }
        });

        var apiGatewayResource = restApi.Root.AddResource("reservation-api", new ResourceOptions
        {
            DefaultCorsPreflightOptions = new CorsOptions
            {
                AllowOrigins = Cors.ALL_ORIGINS,
                AllowMethods = new[] { "GET" },
                AllowHeaders = Cors.DEFAULT_HEADERS
            }
        });
    }
}