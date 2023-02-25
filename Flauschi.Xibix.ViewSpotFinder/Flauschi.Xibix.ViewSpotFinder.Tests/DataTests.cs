using Flauschi.Xibix.ViewSpotFinder.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flauschi.Xibix.ViewSpotFinder.Tests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void ParsingTestMesh_Succeeds()
        {
            const string testFilePath = "./Files/mesh[1][1][1][1][1][1].json";

            var mesh = MeshData.FromFile(testFilePath);

            Assert.IsNotNull(mesh);

            Assert.IsNotNull(mesh.Elements);
            Assert.AreEqual(200, mesh.Elements.Length);

            Assert.IsNotNull(mesh.Nodes);
            Assert.AreEqual(121, mesh.Nodes.Length);

            Assert.IsNotNull(mesh.Values);
            Assert.AreEqual(200, mesh.Values.Length);
        }
    }
}