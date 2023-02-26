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

        /// <summary>
        /// Ensures all data that assumingly was set by serializers are valid
        /// </summary>
        /// <remarks>
        /// Verifies that all properties have been initialized
        /// Verifies that all <see cref="Elements"/> reference existing <see cref="Nodes"/>
        /// Verifies that all <see cref="Elements"/> have <see cref="Values"/> that are referencing them
        /// </remarks>
        public void Validate()
        {
            EnsureAllCollectionsHaveValueAssigned();

            EnsureAllElementsReferenceExistingNodes();

            EnsureAllElementsHaveValueReferences();
        }

        private void EnsureAllCollectionsHaveValueAssigned()
        {
            if (Nodes == default)
                throw new NullReferenceException($"{nameof(Nodes)} must have a value");

            if (Elements == default)
                throw new NullReferenceException($"{nameof(Nodes)} must have a value");

            if (Values == default)
                throw new NullReferenceException($"{nameof(Nodes)} must have a value");
        }

        private void EnsureAllElementsReferenceExistingNodes()
        {
            var nodeIds = new HashSet<int>(
                Nodes.Select(x => x.Id));

            foreach (var element in Elements)
                foreach (var referencedNodeId in element.NodeIds)
                    if (!nodeIds.Contains(referencedNodeId))
                        throw new InvalidOperationException(
                            $"Element '{element.Id}' references node '{referencedNodeId}' but it does not exist");
        }

        private void EnsureAllElementsHaveValueReferences()
        {
            var valueReferencedElementIds = new HashSet<int>(
                Values.Select(x => x.ElementId));

            foreach (var element in Elements)
                if (!valueReferencedElementIds.Contains(element.Id))
                    throw new InvalidOperationException(
                        $"Element '{element.Id}' has no Value");
        }
    }
}