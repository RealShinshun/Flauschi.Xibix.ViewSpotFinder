using Flauschi.Xibix.ViewSpotFinder.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Flauschi.Xibix.ViewSpotFinder.Tests
{
    [TestClass]
    public class ValidityTests
    {
        /// <summary>
        /// Ensures that a mesh that does not contain any <see cref="ElementData"/>s in <see cref="MeshData.Elements"/>
        /// does not return any <see cref="ViewSpot"/>s as those would be invalid by default
        /// </summary>
        [TestMethod]
        public void Mesh_WithoutElements_ReturnsNoViewSpots()
        {
            var mesh = new MeshData
            {
                Elements = new ElementData[0],
                Nodes = new NodeData[0],
                Values = new ValueData[0]
            };

            var viewSpots = new ViewSpotFinder()
                .FindAll(Mesh.FromData(mesh));

            Assert.IsNotNull(viewSpots);
            Assert.AreEqual(0, viewSpots.Length);
        }

        /// <summary>
        /// Ensures that a mesh that contains exactly one <see cref="ElementData"/> in <see cref="MeshData.Elements"/>
        /// returns it as a <see cref="ViewSpot"/> as it is one by default
        /// </summary>
        [TestMethod]
        public void Mesh_WithSingleElement_ReturnsItAsViewSpot()
        {
            var mesh = new MeshData
            {
                Elements = new ElementData[]
                {
                    new()
                    {
                        Id = 1,
                        NodeIds= new[] { 1, 2, 3 }
                    }
                },
                Nodes = new NodeData[]
                {
                     new() { Id = 1 },
                     new() { Id = 2 },
                     new() { Id = 3 }
                },
                Values = new ValueData[]
                {
                    new()
                    {
                        ElementId = 1,
                        Value = default
                    }
                }
            };

            var viewSpots = new ViewSpotFinder()
                .FindAll(Mesh.FromData(mesh));

            Assert.IsNotNull(viewSpots);
            Assert.AreEqual(1, viewSpots.Length);
            Assert.AreEqual(1, viewSpots[0].ElementId);
            Assert.AreEqual(default, viewSpots[0].Value);
        }

        /// <summary>
        /// Ensures that a mesh that contains exactly two <see cref="ElementData"/>s in <see cref="MeshData.Elements"/>
        /// that are neighbors (shares one ore more nodes) returns the <see cref="ElementData"/> with greater <see cref="ValueData"/>
        /// as a <see cref="ViewSpot"/>
        /// </summary>
        [TestMethod]
        public void Mesh_WithTwoBorderingElements_IdentifiesCorrectViewSpot()
        {
            var mesh = new MeshData
            {
                Elements = new ElementData[]
                {
                    new()
                    {
                        Id = 1,
                        NodeIds= new[] { 1, 2, 3 }
                    },
                    new()
                    {
                        Id = 2,
                        NodeIds= new[] { 1, 2, 4 }
                    }
                },
                Nodes = new NodeData[]
                {
                     new() { Id = 1 },
                     new() { Id = 2 },
                     new() { Id = 3 },
                     new() { Id = 4 }
                },
                Values = new ValueData[]
                {
                    new()
                    {
                        ElementId = 1,
                        Value = 0.15
                    },
                    new()
                    {
                        ElementId = 2,
                        Value = 0.2
                    }
                }
            };

            var viewSpots = new ViewSpotFinder()
                .FindAll(Mesh.FromData(mesh));

            Assert.IsNotNull(viewSpots);
            Assert.AreEqual(1, viewSpots.Length);
            Assert.AreEqual(2, viewSpots[0].ElementId);
            Assert.AreEqual(0.2, viewSpots[0].Value);
        }

        /// <summary>
        /// Ensures that a mesh that contains three <see cref="ElementData"/>s in <see cref="MeshData.Elements"/>
        /// that are neighbors (shares one ore more nodes) and are arranged in a line (a neighbors b, b neighbors c, a does not neighbor c)
        /// returns accurate <see cref="ViewSpot"/>s depending on provided values
        /// </summary>
        [TestMethod]
        [DataRow(0.1, 0.3, 0.2, false, true, false)]
        [DataRow(0.4, 0.3, 0.2, true, false, false)]
        [DataRow(0.4, 0.3, 0.4, true, false, true)]
        [DataRow(0.1, 0.2, 5, false, false, true)]
        public void Mesh_WithTwoBorderingElements_IdentifiesCorrectViewSpots(
            double valueA,
            double valueB,
            double valueC,
            bool aIsViewSpot,
            bool bIsViewSpot,
            bool cIsViewSpot)
        {
            var mesh = new MeshData
            {
                Elements = new ElementData[]
                {
                    new()
                    {
                        Id = 1,
                        NodeIds= new[] { 1, 2, 3 }
                    },
                    new()
                    {
                        Id = 2,
                        NodeIds= new[] { 3, 4, 5 }
                    },
                    new()
                    {
                        Id = 3,
                        NodeIds= new[] { 5, 6, 7 }
                    }
                },
                Nodes = new NodeData[]
                {
                     new() { Id = 1 },
                     new() { Id = 2 },
                     new() { Id = 3 },
                     new() { Id = 4 },
                     new() { Id = 5 },
                     new() { Id = 6 },
                     new() { Id = 7 }
                },
                Values = new ValueData[]
                {
                    new()
                    {
                        ElementId = 1,
                        Value = valueA
                    },
                    new()
                    {
                        ElementId = 2,
                        Value = valueB
                    },
                    new()
                    {
                        ElementId = 3,
                        Value = valueC
                    }
                }
            };

            var viewSpots = new ViewSpotFinder()
                .FindAll(Mesh.FromData(mesh));

            Assert.IsNotNull(viewSpots);

            var amountOfViewSpots = new[]
            {
                aIsViewSpot,
                bIsViewSpot,
                cIsViewSpot
            }.Count(x => x);

            Assert.AreEqual(amountOfViewSpots, viewSpots.Length);

            if (aIsViewSpot)
            {
                var aAsViewSpot = viewSpots.FirstOrDefault(x => x.ElementId == 1);
                Assert.IsNotNull(aAsViewSpot);
                Assert.AreEqual(1, aAsViewSpot.ElementId);
                Assert.AreEqual(valueA, aAsViewSpot.Value);
            }

            if (bIsViewSpot)
            {
                var bAsViewSpot = viewSpots.FirstOrDefault(x => x.ElementId == 2);
                Assert.IsNotNull(bAsViewSpot);
                Assert.AreEqual(2, bAsViewSpot.ElementId);
                Assert.AreEqual(valueB, bAsViewSpot.Value);
            }

            if (cIsViewSpot)
            {
                var cAsViewSpot = viewSpots.FirstOrDefault(x => x.ElementId == 3);
                Assert.IsNotNull(cAsViewSpot);
                Assert.AreEqual(3, cAsViewSpot.ElementId);
                Assert.AreEqual(valueC, cAsViewSpot.Value);
            }
        }
    }
}