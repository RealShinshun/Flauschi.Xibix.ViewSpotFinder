using System.Text.Json;

namespace Flauschi.Xibix.ViewSpotFinder.Data
{
    /// <summary>
    /// Represents input file format described in assignment.
    /// A mesh is a collection of elements and nodes and is partitioned in
    /// triangles (<see cref="Elements"/>).
    /// </summary>
    public class MeshData
    {
        private static readonly JsonSerializerOptions DefaultSerializerOptions
            = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        public NodeData[] Nodes { get; set; } = default!;
        public ElementData[] Elements { get; set; } = default!;
        public ValueData[] Values { get; set; } = default!;

        public static MeshData FromFile(
            string meshFilePath)
        {
            using var fileStream = File.OpenRead(meshFilePath);
            return JsonSerializer.Deserialize<MeshData>(fileStream, DefaultSerializerOptions)
                ?? throw new NullReferenceException($"Failed parsing mesh file '{meshFilePath}'");
        }

        public static MeshData FromString(
            string meshFileData)
        {
            return JsonSerializer.Deserialize<MeshData>(meshFileData, DefaultSerializerOptions)
                ?? throw new NullReferenceException($"Failed parsing mesh file from content '{meshFileData}'");
        }
    }
}