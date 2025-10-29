using Mono.Documentation;
using NUnit.Framework;
using System.IO;

namespace mdoc.Test
{
    [TestFixture]
    public class MdocFileTests
    {
        private string testFilePath;
        private string tempFilePath;

        [SetUp]
        public void SetUp()
        {
            testFilePath = Path.GetTempFileName();
            tempFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        [Test]
        public void UpdateFile_FileDoesNotExist_CreatesFile()
        {
            File.Delete(testFilePath);

            MdocFile.UpdateFile(testFilePath, path => File.WriteAllText(path, "test content"));

            Assert.IsTrue(File.Exists(testFilePath));
            Assert.AreEqual("test content", File.ReadAllText(testFilePath));
        }

        [Test]
        public void UpdateFile_FileExistsAndIsDifferent_UpdatesFile()
        {
            File.WriteAllText(testFilePath, "old content");

            MdocFile.UpdateFile(testFilePath, path => File.WriteAllText(path, "new content"));

            Assert.AreEqual("new content", File.ReadAllText(testFilePath));
        }

        [Test]
        public void UpdateFile_FileExistsAndIsIdentical_DoesNotUpdateFile()
        {
            File.WriteAllText(testFilePath, "identical content");

            MdocFile.UpdateFile(testFilePath, path => File.WriteAllText(path, "identical content"));

            Assert.AreEqual("identical content", File.ReadAllText(testFilePath));
        }

        [Test]
        public void DeleteFile_FileExists_DeletesFile()
        {
            File.WriteAllText(testFilePath, "content");

            MdocFile.DeleteFile(testFilePath);

            Assert.IsFalse(File.Exists(testFilePath));
        }

        [Test]
        public void DeleteFile_FileDoesNotExist_DoesNotThrow()
        {
            File.Delete(testFilePath);

            Assert.DoesNotThrow(() => MdocFile.DeleteFile(testFilePath));
        }
    }
}
