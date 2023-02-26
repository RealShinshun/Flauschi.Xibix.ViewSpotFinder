using Flauschi.Xibix.ViewSpotFinder.Data;
using System.Collections.ObjectModel;

namespace Flauschi.Xibix.ViewSpotFinder
{
    public class Node
    {
        public int Id { get; }
        public double X { get; }
        public double Y { get; }

        public ICollection<Element> Elements { get; }

        public Node(NodeData nodeData)
            : this(
                  nodeData.Id,
                  nodeData.X,
                  nodeData.Y)
        { /* empty */ }

        public Node(
            int id,
            double x,
            double y)
        {
            Id = id;
            X = x;
            Y = y;

            Elements = new Collection<Element>();
        }
    }
}