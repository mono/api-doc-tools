using Mono.Documentation;
using NUnit.Framework;
using System;

namespace mdoc.Test
{
    [TestFixture]
    public class MDocExceptionTests
    {
        [Test]
        public void Constructor_WithAssemblyNameAndMessage_ShouldSetProperties()
        {
            // Arrange
            var assemblyName = "TestAssembly";
            var message = "Test message";

            // Act
            var exception = new MDocAssemblyException(assemblyName, message);

            // Assert
            Assert.AreEqual(assemblyName, exception.AssemblyName);
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void Constructor_WithAssemblyNameMessageAndInnerException_ShouldSetProperties()
        {
            // Arrange
            var assemblyName = "TestAssembly";
            var message = "Test message";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new MDocAssemblyException(assemblyName, message, innerException);

            // Assert
            Assert.AreEqual(assemblyName, exception.AssemblyName);
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }

        [Test]
        public void TestDefaultConstructor()
        {
            var exception = new MDocException();
            Assert.NotNull(exception);
        }

        [Test]
        public void TestConstructorWithMessage()
        {
            var message = "Test message";
            var exception = new MDocException(message);
            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void TestConstructorWithMessageAndInnerException()
        {
            var message = "Test message";
            var innerException = new Exception("Inner exception");
            var exception = new MDocException(message, innerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
