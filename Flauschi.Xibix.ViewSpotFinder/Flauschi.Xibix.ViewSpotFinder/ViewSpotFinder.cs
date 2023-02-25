using Flauschi.Xibix.ViewSpotFinder.Data;

namespace Flauschi.Xibix.ViewSpotFinder
{
    public class ViewSpotFinder
    {
        public ViewSpotFinder()
        { /* empty */ }

        public ViewSpot[] FindAll(MeshData meshData)
        {
            var mesh = Mesh.FromMeshData(meshData);

            return FindViewSpots(mesh.Elements.OrderByDescending(x => x.Value))
                .ToArray();
        }

        public ViewSpot[] Find(MeshData meshData, int amount)
        {
            var mesh = Mesh.FromMeshData(meshData);

            return FindViewSpots(mesh.Elements)
                .Take(amount)
                .ToArray();
        }

        private IEnumerable<ViewSpot> FindViewSpots(
            IEnumerable<Element> elements)
        {
            var excludedElements = new HashSet<Element>();

            foreach (var element in elements)
            {
                if (excludedElements.Contains(element))
                    continue;

                var neighboringElements = element.Nodes
                    .SelectMany(x => x.Elements)
                    .ToList();

                var isLocalMaxima = neighboringElements
                    .All(x => x.Value <= element.Value);

                if (isLocalMaxima)
                {
                    yield return new ViewSpot
                    {
                        ElementId = element.Id,
                        Value = element.Value
                    };

                    excludedElements.UnionWith(
                        neighboringElements);
                }

                excludedElements.Add(element);
            }
        }
    }
}