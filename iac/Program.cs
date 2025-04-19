using Amazon.CDK;
using iac;
using Environment = System.Environment;

namespace iac

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            
            string? awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");
            string? awsAccountId = Environment.GetEnvironmentVariable("AWS_ACCOUNT_ID");
            string? stackName = Environment.GetEnvironmentVariable("STACK_NAME");
            string? githubRef = Environment.GetEnvironmentVariable("GITHUB_REF");
            
            Console.WriteLine(awsRegion);
            Console.WriteLine(awsAccountId);
            Console.WriteLine(stackName);
            Console.WriteLine(githubRef);

            new LambdaStack(app, new Dictionary<string, string>());
            new IacStack(app, stackName);

            app.Synth();
        }
    }
}