using Flauschi.Xibix.ViewSpotFinder.Data;

namespace Flauschi.Xibix.ViewSpotFinder
{
    /// <summary>
    /// Represents a map of a hilly landscape. A mesh is partitioned
    /// in triangles (<see cref="Elements"/>)
    /// </summary>
    public class Mesh
    {
        public Element[] Elements { get; }
        public Node[] Nodes { get; }

        private Mesh(
            IEnumerable<Element> elements,
            IEnumerable<Node> nodes)
        {
            Elements = elements.ToArray();
            Nodes = nodes.ToArray();
        }

        /// <summary>
        /// Creates a <see cref="Mesh"/> from provided <paramref name="data"/>
        /// to more optimally navigate <see cref="Elements"/> and <see cref="Nodes"/>
        /// </summary>
        /// <param name="data">The <see cref="MeshData"/> to create the <see cref="Mesh"/> from</param>
        /// <returns>The created <see cref="Mesh"/></returns>
        /// <exception cref="Exception">
        /// if invalid <paramref name="data"/> is provided
        /// </exception>
        /// <remarks>
        /// Ignores missing <see cref="MeshData.Values"/>
        /// </remarks>
        public static Mesh FromData(
            MeshData data)
        {
            //performance can be optimized if this is left out
            //this is only required if x and y of Node are needed
            var nodeDictionary = data.Nodes
                .ToDictionary(
                    x => x.Id,
                    x => new Node(x));

            //throws if mesh data was not validated and nodes are missing
            var elementDictionary = data.Elements
                .ToDictionary(
                    x => x.Id,
                    x =>
                    {
                        var element = new Element(x);

                        foreach (var nodeId in x.NodeIds)
                            element.AddNode(nodeDictionary[nodeId]);

                        return element;
                    });

            foreach (var valueData in data.Values)
                elementDictionary[valueData.ElementId].Value = valueData.Value;

            return new Mesh(
                elementDictionary.Values,
                nodeDictionary.Values);
        }
    }
}