using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Flauschi.Xibix.ViewSpotFinder.Data;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Flauschi.Xibix.ViewSpotFinder.AwsServerlessApplication;

public class Functions
{
    /// <summary>
    /// Default constructor that Lambda will invoke.
    /// </summary>
    public Functions()
    {
    }

    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The API Gateway response.</returns>
    public APIGatewayProxyResponse FindViewSpots(
        APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        var specifiesAmountOfViewSpots =
            request.QueryStringParameters != default &&
            request.QueryStringParameters.ContainsKey("amount");

        var meshData = MeshData.FromString(request.Body);

        ViewSpot[] viewSpots;

        if (specifiesAmountOfViewSpots)
            viewSpots = new ViewSpotFinder().Find(meshData, int.Parse(request.QueryStringParameters!["amount"]));
        else
            viewSpots = new ViewSpotFinder().FindAll(meshData);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var bodyContent = JsonSerializer.Serialize(
            viewSpots.OrderByDescending(x => x.Value),
            jsonSerializerOptions);

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = bodyContent,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/plain" }
            }
        };

        return response;
    }
}