using Amazon.CDK;

namespace Iac
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            app.Synth();
        }
    }
}