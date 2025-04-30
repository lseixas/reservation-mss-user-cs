using System.Text.Json;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace common;

public class CustomCamelCaseLambdaJsonSerializer
    : DefaultLambdaJsonSerializer
{
    public CustomCamelCaseLambdaJsonSerializer()
        : base(options =>
        {
            // enforce camel-case naming for all properties
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        })
    {
    }
}