using Flauschi.Xibix.ViewSpotFinder.Data;

namespace Flauschi.Xibix.ViewSpotFinder
{
    public class Mesh
    {
        public Element[] Elements { get; set; } = default!;

        public Mesh(IEnumerable<Element> elements)
        {
            Elements = elements.ToArray();
        }

        public static Mesh FromMeshData(
            MeshData meshData)
        {
            var nodeDictionary = new Dictionary<int, Node>();
            var elementDictionary = new Dictionary<int, Element>();

            //performance can be optimized if this is left out
            //this is only required if x and y of Node are needed
            foreach (var nodeData in meshData.Nodes)
                nodeDictionary.Add(
                    nodeData.Id,
                    new Node(nodeData));

            foreach (var elementData in meshData.Elements)
            {
                var element = new Element(elementData);

                foreach (var nodeId in elementData.NodeIds)
                    //TODO: exception handling if nodeId is not found
                    //-> bad input file
                    element.AddNode(nodeDictionary[nodeId]);

                elementDictionary.Add(elementData.Id, element);
            }

            foreach (var valueData in meshData.Values)
                elementDictionary[valueData.ElementId].Value = valueData.Value;

            return new Mesh(elementDictionary.Values);
        }
    }
}