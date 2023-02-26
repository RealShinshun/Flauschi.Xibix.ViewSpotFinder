using Flauschi.Xibix.ViewSpotFinder.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flauschi.Xibix.ViewSpotFinder.Tests
{
    [TestClass]
    public class DataTests
    {
        /// <summary>
        /// Verifies that serializing of the test mesh data creates
        /// a valid mesh and contains expected data
        /// </summary>
        [TestMethod]
        public void ParsingTestMesh_SucceedsAndIsValid()
        {
            const string testFilePath = "./Files/mesh[1][1][1][1][1][1].json";

            var mesh = MeshData.FromFile(testFilePath);

            Assert.IsNotNull(mesh);

            mesh.Validate();

            Assert.AreEqual(200, mesh.Elements.Length);
            Assert.AreEqual(121, mesh.Nodes.Length);
            Assert.AreEqual(200, mesh.Values.Length);
        }
    }
}