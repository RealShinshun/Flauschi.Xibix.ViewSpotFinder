using Flauschi.Xibix.ViewSpotFinder.Data;
using System.Collections.ObjectModel;

namespace Flauschi.Xibix.ViewSpotFinder
{
    public class Element
    {
        public int Id { get; }

        public double Value { get; set; }
        public ICollection<Node> Nodes { get; }

        public Element(
            ElementData elementData)
            : this(elementData.Id)
        { /* empty */ }

        public Element(int id)
        {
            Id = id;
            Value = 0;
            Nodes = new Collection<Node>();
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
            node.Elements.Add(this);
        }
    }
}