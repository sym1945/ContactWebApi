using ContactWebApi.Domain.Exceptions;
using ContactWebApi.Domain.Models;

namespace ContactWebApi.UnitTests.Domain.Common
{
    public class ExceptionMessageTest
    {
        [Test]
        public void InvalidModelExceptionMessage()
        {
            var msg = "Invalid model exception";
            var modelErrors = new List<ModelError>
            {
                new ModelError("1", "1"),
                new ModelError("1", "2"),
                new ModelError("2", "1"),
                new ModelError("3", "1"),
                new ModelError("3", "2"),
            };

            var ex = new InvalidModelException(msg, modelErrors.ToArray());

            Assert.That(ex.Message, Is.EqualTo(msg));
            Assert.That(ex.ModelErrors, Is.EqualTo(modelErrors));
        }


        [Test]
        [TestCase("asdf", new string[] { }, ExpectedResult = $"'asdf' is an unsupported ContentType.")]
        [TestCase("asdf", new string[] { "apple" }, ExpectedResult = $"'asdf' is an unsupported ContentType. supported: [ 'apple' ]")]
        [TestCase("asdf", new string[] { "apple", "banana" }, ExpectedResult = $"'asdf' is an unsupported ContentType. supported: [ 'apple', 'banana' ]")]
        [TestCase("asdf", new string[] { "apple", "banana", "mango" }, ExpectedResult = $"'asdf' is an unsupported ContentType. supported: [ 'apple', 'banana', 'mango' ]")]
        public string UnsupportedImportContentTypeExceptionMessage(string reqContentType, string[] supportedContentTypes)
        {
            var ex = new UnsupportedImportContentTypeException(reqContentType, supportedContentTypes);

            return ex.Message;
        }


    }
}
