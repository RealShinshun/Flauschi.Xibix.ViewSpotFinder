using Flauschi.Xibix.ViewSpotFinder.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Flauschi.Xibix.ViewSpotFinder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ParseCommandLineArguments(
                    args,
                    out var meshFilePath,
                    out var amountOfViewSpotsToBeFound);

                var mesh = MeshData.FromFile(meshFilePath);

                var foundViewSpots = new ViewSpotFinder()
                    .Find(mesh, amountOfViewSpotsToBeFound)
                    .OrderByDescending(x => x.Value);

                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                Console.Out.Write(
                    JsonSerializer.Serialize(foundViewSpots, jsonSerializerOptions));
            }
            catch (Exception ex)
            {
                ExitWithError(ex.Message);
            }
        }

        static void ParseCommandLineArguments(
            string[] args,
            out string meshFilePath,
            out int numberOfViewSpotsToBeFound)
        {
            if (args.Length != 2)
                throw new InvalidOperationException("Invalid number of arguments");

            meshFilePath = args[0];

            if (!int.TryParse(args[1], out numberOfViewSpotsToBeFound))
                throw new InvalidOperationException("Failed parsing numberOfViewSpots arg");
        }

        [DoesNotReturn]
        static void ExitWithError(string errorDescription)
        {
            Console.Error.Write(errorDescription);
            Environment.Exit(-1);
        }
    }
}