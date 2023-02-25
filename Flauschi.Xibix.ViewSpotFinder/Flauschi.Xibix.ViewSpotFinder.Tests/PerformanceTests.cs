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
        [TestMethod]
        public void FindsViewSpots_OfMeshWith10kElements_InLessThan15Seconds()
        {
            const string test10kFilePath = "./Files/mesh_x_sin_cos_10000[82][1][1][1][1][1][1].json";

            var averageTimeTaken = MeasureAverage(
                () =>
                {
                    var mesh = MeshData.FromFile(test10kFilePath);
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 10);

            Assert.IsTrue(averageTimeTaken.TotalSeconds < 15);
        }

        [TestMethod]
        public void FindsViewSpots_OfMeshWith10kElements_InLessThan1Second()
        {
            const string test10kFilePath = "./Files/mesh_x_sin_cos_10000[82][1][1][1][1][1][1].json";

            var averageTimeTaken = MeasureAverage(
                () =>
                {
                    var mesh = MeshData.FromFile(test10kFilePath);
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 10);

            Assert.IsTrue(averageTimeTaken.TotalSeconds < 1);
        }

        [TestMethod]
        public void FindsViewSpots_OfMeshWith20kElements_IsLessThan3TimesHigherThanWith10k()
        {
            const string test10kFilePath = "./Files/mesh_x_sin_cos_10000[82][1][1][1][1][1][1].json";
            const string test20kFilePath = "./Files/mesh_x_sin_cos_20000[1][1][1][1][1][1].json";

            var averageTimeTakenFor10k = MeasureAverage(
                () =>
                {
                    var mesh = MeshData.FromFile(test10kFilePath);
                    _ = new ViewSpotFinder().FindAll(mesh);
                }, 20);

            var averageTimeTakenFor20k = MeasureAverage(
                () =>
                {
                    var mesh = MeshData.FromFile(test20kFilePath);
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