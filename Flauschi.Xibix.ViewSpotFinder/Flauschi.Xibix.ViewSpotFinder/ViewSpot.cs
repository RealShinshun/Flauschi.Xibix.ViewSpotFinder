namespace Flauschi.Xibix.ViewSpotFinder
{
    /// <summary>
    /// Represents output result described in assignment
    /// </summary>
    public class ViewSpot
    {
        public int ElementId { get; }
        public double Value { get; }

        public ViewSpot(Element element)
            : this(element.Id, element.Value)
        { /* empty */ }

        public ViewSpot(int elementId, double value)
        {
            ElementId = elementId;
            Value = value;
        }
    }
}