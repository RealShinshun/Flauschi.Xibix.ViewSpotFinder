namespace Flauschi.Xibix.ViewSpotFinder
{
    public class ViewSpotFinder
    {
        public ViewSpotFinder()
        { /* empty */ }

        /// <summary>
        /// Finds all <see cref="ViewSpot"/>s of a <paramref name="mesh"/> and returns them
        /// ordered by <see cref="ViewSpot.Value"/>
        /// </summary>
        /// <param name="mesh">The mesh to find <see cref="ViewSpot"/>s from</param>
        /// <returns>All found <see cref="ViewSpot"/>s ordered by <see cref="ViewSpot.Value"/></returns>
        public ViewSpot[] FindAll(Mesh mesh)
            => FindViewSpots(mesh.Elements)
            .OrderByDescending(x => x.Value)
            .ToArray();

        /// <summary>
        /// Finds a specific amount of <see cref="ViewSpot"/>s of a <paramref name="mesh"/>
        /// starting with the heighest <see cref="ViewSpot.Value"/> to lowest <see cref="ViewSpot.Value"/>
        /// </summary>
        /// <param name="mesh">The mesh to find <see cref="ViewSpot"/>s from</param>
        /// <returns>
        /// All found <see cref="ViewSpot"/>s starting with the heighest <see cref="ViewSpot.Value"/>
        /// and ending with the lowest <see cref="ViewSpot.Value"/> or limiting by <paramref name="amount"/>
        /// </returns>
        public ViewSpot[] Find(Mesh mesh, int amount)
            => FindViewSpots(mesh.Elements.OrderByDescending(x => x.Value))
            .Take(amount)
            .ToArray();

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

                //neighboringElements might contain duplicate elements and also the current element
                //as the assignments states that elements with same height should only find the first instance
                //it should not matter and improve performance if not filtering them out
                var isLocalMaxima = neighboringElements
                    .All(x => x.Value <= element.Value);

                if (isLocalMaxima)
                {
                    yield return new ViewSpot(element);

                    excludedElements.UnionWith(
                        neighboringElements);
                }

                excludedElements.Add(element);
            }
        }
    }
}