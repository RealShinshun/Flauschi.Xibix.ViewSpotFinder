using Flauschi.Xibix.ViewSpotFinder.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Flauschi.Xibix.ViewSpotFinder.Tests
{
    [TestClass]
    public class PerformanceTests
    {
        /// <summary>
        /// Verifies the view spot finding code's performance is better than
        /// the set, worst performing limit for equivalent python code
        /// </summary>
        [TestMethod]
        public void FindsViewSpots_OfMeshWith10kElements_InLessThan15Seconds()
        {
            const string test10kFilePath = "./Files/mesh_x_sin_cos_10000[82][1][1][1][1][1][1].json";

            var averageTimeTaken = MeasureAverage(
                () =>
                {
                    var mesh = Mesh.FromData(
                        MeshData.FromFile(test10kFilePath));
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 10);

            Assert.IsTrue(averageTimeTaken.TotalSeconds < 15);
        }

        /// <summary>
        /// Verifies the view spot finding code's performance is better than
        /// the set, worst performing limit for equivalent python code taking
        /// into account that C# code should be at least twice as fast
        /// </summary>
        [TestMethod]
        public void FindsViewSpots_OfMeshWith10kElements_InLessThan7500Milliseconds()
        {
            const string test10kFilePath = "./Files/mesh_x_sin_cos_10000[82][1][1][1][1][1][1].json";

            var averageTimeTaken = MeasureAverage(
                () =>
                {
                    var mesh = Mesh.FromData(
                        MeshData.FromFile(test10kFilePath));
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 10);

            Assert.IsTrue(averageTimeTaken.TotalMilliseconds < 7500);
        }

        /// <summary>
        /// Verifies the view spot finding code's performance is better than
        /// the set, optimally performing limit for equivalent python code
        /// </summary>
        [TestMethod]
        public void FindsViewSpots_OfMeshWith10kElements_InLessThan1Second()
        {
            const string test10kFilePath = "./Files/mesh_x_sin_cos_10000[82][1][1][1][1][1][1].json";

            var averageTimeTaken = MeasureAverage(
                () =>
                {
                    var mesh = Mesh.FromData(
                        MeshData.FromFile(test10kFilePath));
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 10);

            Assert.IsTrue(averageTimeTaken.TotalSeconds < 1);
        }

        /// <summary>
        /// Verifies the view spot finding code's performance is better than
        /// the set, optimally performing limit for equivalent python code taking
        /// into account that C# code should be at least twice as fast
        /// </summary>
        [TestMethod]
        public void FindsViewSpots_OfMeshWith10kElements_InLessThan500Milliseconds()
        {
            const string test10kFilePath = "./Files/mesh_x_sin_cos_10000[82][1][1][1][1][1][1].json";

            var averageTimeTaken = MeasureAverage(
                () =>
                {
                    var mesh = Mesh.FromData(
                        MeshData.FromFile(test10kFilePath));
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 10);

            Assert.IsTrue(averageTimeTaken.TotalMilliseconds < 500);
        }

        /// <summary>
        /// Verifies the view spot finding code's performance for an input mesh of
        /// 20.000 elements is not less than 3 times slower than a mesh of 10.000 elements
        /// </summary>
        [TestMethod]
        public void FindsViewSpots_OfMeshWith20kElements_IsLessThan3TimesHigherThanWith10k()
        {
            const string test10kFilePath = "./Files/mesh_x_sin_cos_10000[82][1][1][1][1][1][1].json";
            const string test20kFilePath = "./Files/mesh_x_sin_cos_20000[1][1][1][1][1][1].json";

            var averageTimeTakenFor10k = MeasureAverage(
                () =>
                {
                    var mesh = Mesh.FromData(
                        MeshData.FromFile(test10kFilePath));
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 20);

            var averageTimeTakenFor20k = MeasureAverage(
                () =>
                {
                    var mesh = Mesh.FromData(
                        MeshData.FromFile(test20kFilePath));
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 20);

            Assert.IsTrue(
                averageTimeTakenFor20k < averageTimeTakenFor10k * 3,
                $"10k: {averageTimeTakenFor10k.TotalMilliseconds}, 20k: {averageTimeTakenFor20k.TotalMilliseconds}");
        }

        private static TimeSpan MeasureAverage(
            Action action,
            int executions)
        {
            var stopwatch = new Stopwatch();
            var measuredTimes = new List<TimeSpan>();

            for (var i = 0; i < executions; i++)
            {
                stopwatch.Start();

                action();

                stopwatch.Stop();
                measuredTimes.Add(stopwatch.Elapsed);

                stopwatch.Reset();
            }

            return TimeSpan.FromMilliseconds(
                measuredTimes.Average(x => x.TotalMilliseconds));
        }
    }
}