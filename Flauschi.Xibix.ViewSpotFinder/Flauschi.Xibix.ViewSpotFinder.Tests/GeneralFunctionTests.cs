using Flauschi.Xibix.ViewSpotFinder.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flauschi.Xibix.ViewSpotFinder.Tests
{
    [TestClass]
    public class GeneralFunctionTests
    {
        [TestMethod]
        public void FindAllViewSpots_ReturnsViewSpots()
        {
            const string testFilePath = "./Files/mesh[1][1][1][1][1][1].json";

            var mesh = MeshData.FromFile(testFilePath);

            var foundViewSpots = new ViewSpotFinder()
                .FindAll(mesh);

            Assert.IsNotNull(foundViewSpots);
        }

        [TestMethod]
        public void FindingViewSpots_HonorsAmountParameter()
        {
            const string testFilePath = "./Files/mesh[1][1][1][1][1][1].json";
            const int amountOfViewSpotsToFind = 2;

            var mesh = MeshData.FromFile(testFilePath);

            var foundViewSpots = new ViewSpotFinder()
                .Find(mesh, amountOfViewSpotsToFind);

            Assert.AreEqual(amountOfViewSpotsToFind, foundViewSpots.Length);
        }
    }
}