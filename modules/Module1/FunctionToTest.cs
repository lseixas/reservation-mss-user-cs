using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace modules.Module1;

public class FunctionToTest
{
    public string FunctionHandler(string input, ILambdaContext context)
    {

        return "Hello your input was: " + input.ToUpper() + "; The number from layer is: shit";
        
    }
}
