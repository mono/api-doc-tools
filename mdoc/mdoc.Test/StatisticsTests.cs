using System.Collections.Generic;
using NUnit.Framework;

using Mono.Documentation.Updater.Statistics;

namespace mdoc.Test
{
    [TestFixture()]
    public class StatisticsTests
    {
        const string FRAMEWORK_NAME = "frameworkName";
        
        [Test()]
        public void StatisticsStorage_Increment()
        {
            var statisticsStorage = new StatisticsStorage();

            statisticsStorage.AddMetric(StatisticsItem.Members, StatisticsMetrics.Added);

            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Members][StatisticsMetrics.Added], 1);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Members][StatisticsMetrics.Removed], 0);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Namespaces][StatisticsMetrics.Added], 0);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Namespaces][StatisticsMetrics.Removed], 0);
        }

        [Test()]
        public void StatisticsStorage_ManyIncrements()
        {
            var statisticsStorage = new StatisticsStorage();

            statisticsStorage.AddMetric(StatisticsItem.Members, StatisticsMetrics.Added);
            statisticsStorage.AddMetric(StatisticsItem.Members, StatisticsMetrics.Added);
            statisticsStorage.AddMetric(StatisticsItem.Members, StatisticsMetrics.Added);

            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Members][StatisticsMetrics.Added], 3);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Members][StatisticsMetrics.Removed], 0);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Namespaces][StatisticsMetrics.Added], 0);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Namespaces][StatisticsMetrics.Removed], 0);
        }

        [Test()]
        public void StatisticsStorage_IncrementDelta()
        {
            var statisticsStorage = new StatisticsStorage();

            statisticsStorage.AddMetric(StatisticsItem.Members, StatisticsMetrics.Added, 5);

            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Members][StatisticsMetrics.Added], 5);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Members][StatisticsMetrics.Removed], 0);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Namespaces][StatisticsMetrics.Added], 0);
            Assert.AreEqual(statisticsStorage.Values[StatisticsItem.Namespaces][StatisticsMetrics.Removed], 0);
        }

        [Test()]
        public void StatisticsCollector_Increment()
        {
            var statisticsCollector = new StatisticsCollector();

            statisticsCollector.AddMetric(FRAMEWORK_NAME, StatisticsItem.Members, StatisticsMetrics.Added);

            Assert.AreEqual(
                statisticsCollector.Storages[FRAMEWORK_NAME].Values[StatisticsItem.Members][StatisticsMetrics.Added], 1);
            Assert.AreEqual(
                statisticsCollector.Storages[FRAMEWORK_NAME].Values[StatisticsItem.Members][StatisticsMetrics.Removed], 0);
            Assert.AreEqual(
                statisticsCollector.Storages[FRAMEWORK_NAME].Values[StatisticsItem.Namespaces][StatisticsMetrics.Added], 0);
            Assert.AreEqual(
                statisticsCollector.Storages[FRAMEWORK_NAME].Values[StatisticsItem.Namespaces][StatisticsMetrics.Removed], 0);
        }

        [Test()]
        public void StatisticsCollector_IncrementDelta()
        {
            var statisticsCollector = new StatisticsCollector();

            statisticsCollector.AddMetric(FRAMEWORK_NAME, StatisticsItem.Members, StatisticsMetrics.Added, 7);

            Assert.AreEqual(
                statisticsCollector.Storages[FRAMEWORK_NAME].Values[StatisticsItem.Members][StatisticsMetrics.Added], 7);
            Assert.AreEqual(
                statisticsCollector.Storages[FRAMEWORK_NAME].Values[StatisticsItem.Members][StatisticsMetrics.Removed], 0);
            Assert.AreEqual(
                statisticsCollector.Storages[FRAMEWORK_NAME].Values[StatisticsItem.Namespaces][StatisticsMetrics.Added], 0);
            Assert.AreEqual(
                statisticsCollector.Storages[FRAMEWORK_NAME].Values[StatisticsItem.Namespaces][StatisticsMetrics.Removed], 0);
        }
    }
}