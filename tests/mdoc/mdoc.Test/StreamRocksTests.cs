using Mono.Rocks;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace mdoc.Test
{
    [TestFixture]
    public class StreamRocksTests
    {
        [Test]
        public void WriteTo_CopiesContentCorrectly()
        {
            // Arrange
            byte[] sourceData = Encoding.UTF8.GetBytes("Hello, World!");
            byte[] destinationData = new byte[sourceData.Length];

            using (MemoryStream sourceStream = new MemoryStream(sourceData))
            using (MemoryStream destinationStream = new MemoryStream(destinationData))
            {
                // Act
                StreamRocks.WriteTo(sourceStream, destinationStream);

                // Assert
                Assert.AreEqual(sourceData, destinationStream.ToArray());
            }
        }
    }

}
