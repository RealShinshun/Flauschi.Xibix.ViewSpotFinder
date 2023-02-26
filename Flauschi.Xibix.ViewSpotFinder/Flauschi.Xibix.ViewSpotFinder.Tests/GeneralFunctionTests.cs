using Flauschi.Xibix.ViewSpotFinder.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flauschi.Xibix.ViewSpotFinder.Tests
{
    [TestClass]
    public class GeneralFunctionTests
    {
        /// <summary>
        /// Verifies that the view spot finding code processes an input mesh
        /// without exceptions and returns any view spots
        /// </summary>
        [TestMethod]
        public void FindAllViewSpots_ReturnsViewSpots()
        {
            const string testFilePath = "./Files/mesh[1][1][1][1][1][1].json";

            var mesh = Mesh.FromData(
                MeshData.FromFile(testFilePath));

            var foundViewSpots = new ViewSpotFinder()
                .FindAll(mesh);

            Assert.IsNotNull(foundViewSpots);
        }

        /// <summary>
        /// Verifies that the view spot finding code limits the results
        /// to the desired amount of viewspots to find
        /// </summary>
        [TestMethod]
        public void FindingViewSpots_HonorsAmountParameter()
        {
            const string testFilePath = "./Files/mesh[1][1][1][1][1][1].json";
            const int amountOfViewSpotsToFind = 2;

            var mesh = Mesh.FromData(
                MeshData.FromFile(testFilePath));

            var allViewSpots = new ViewSpotFinder()
                .FindAll(mesh);

            var limitedViewSpots = new ViewSpotFinder()
                .Find(mesh, amountOfViewSpotsToFind);

            Assert.IsTrue(
                allViewSpots.Length > amountOfViewSpotsToFind,
                "The mesh used for testing must return more viewspots than the limit");

            Assert.AreEqual(amountOfViewSpotsToFind, limitedViewSpots.Length);
        }
    }
}