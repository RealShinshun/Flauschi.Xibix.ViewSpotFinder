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
    { /* empty */ }

    /// <summary>
    /// A Lambda function to respond to HTTP Post methods from API Gateway to
    /// find <see cref="ViewSpot"/>s from <see cref="MeshData"/> provided in
    /// <see cref="APIGatewayProxyRequest.Body"/> limiting the amount of found
    /// <see cref="ViewSpot"/> by the query parameter 'amount'
    /// </summary>
    /// <param name="request">The request to handle</param>
    /// <returns>
    /// The API Gateway response containing all found <see cref="ViewSpot"/>s
    /// optionally limited by 'amount' or a response indicating failure
    /// </returns>
    public APIGatewayProxyResponse FindViewSpots(
        APIGatewayProxyRequest request,
        ILambdaContext context)
    {
        const string amountQueryParameterName = "amount";

        var specifiesAmountOfViewSpots =
            request.QueryStringParameters != default &&
            request.QueryStringParameters.ContainsKey(amountQueryParameterName);

        var meshData = MeshData.FromString(request.Body);

        try
        {
            meshData.Validate();
        }
        catch (Exception ex)
        {
            context.Logger.LogInformation(
                $"Failed validation: {ex.Message}");

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = "\"Failed validating mesh\"",
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "text/plain" }
                }
            };
        }

        var mesh = Mesh.FromData(meshData);

        ViewSpot[] viewSpots;

        if (specifiesAmountOfViewSpots)
        {
            var amount = int.Parse(
                request.QueryStringParameters![amountQueryParameterName]);

            viewSpots = new ViewSpotFinder().Find(mesh, amount);
        }
        else
            viewSpots = new ViewSpotFinder().FindAll(mesh);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var bodyContent = JsonSerializer.Serialize(
            viewSpots,
            jsonSerializerOptions);

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = bodyContent,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            }
        };
    }
}