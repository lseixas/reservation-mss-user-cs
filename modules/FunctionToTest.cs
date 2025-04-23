using Amazon.Lambda.Core;
using shared;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace modules;

public class FunctionToTest
{
    public string FunctionHandler(string input, ILambdaContext context)
    {
        LayerClass smtFromLayer = new LayerClass(number: 1234);

        return "Hello your input was: " + input.ToUpper() + "; The number from layer is: " + smtFromLayer.Number;

    }
}
