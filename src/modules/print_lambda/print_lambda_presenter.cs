using Amazon.Lambda.Core;

namespace src.modules.print_lambda;

public class PrintLambdaHandler
{
    public string FunctionHandler(string message, ILambdaContext context)
    {
        return "Hello from Lambda! " + message;
    }
}