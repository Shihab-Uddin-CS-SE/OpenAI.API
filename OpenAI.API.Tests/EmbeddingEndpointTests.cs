using OpenAI.API.Embedding;
using OpenAI.API.Models;

namespace OpenAI.API.Tests
{
    public class EmbeddingEndpointTests
    {
        [SetUp]
        public void Setup()
        {
            OpenAI.API.APIAuthentication.Default = new OpenAI.API.APIAuthentication(Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY"));
        }

        [Test]
        public void GetBasicEmbedding()
        {
            var api = new OpenAI.API.OpenAIAPI();

            Assert.IsNotNull(api.Embeddings);

            var results = api.Embeddings.CreateEmbeddingAsync(new EmbeddingRequest(Model.AdaTextEmbedding, "A test text for embedding")).Result;
            Assert.IsNotNull(results);
            if (results.CreatedUnixTime.HasValue)
            {
                Assert.NotZero(results.CreatedUnixTime.Value);
                Assert.NotNull(results.Created);
                Assert.Greater(results.Created.Value, new DateTime(2018, 1, 1));
                Assert.Less(results.Created.Value, DateTime.Now.AddDays(1));
            }
            else
            {
                Assert.Null(results.Created);
            }
            Assert.NotNull(results.Object);
            Assert.NotZero(results.Data.Count);
            Assert.That(results.Data.First().Embedding.Length == 1536);
        }

        [Test]
        public void GetSimpleEmbedding()
        {
            var api = new OpenAI.API.OpenAIAPI();

            Assert.IsNotNull(api.Embeddings);

            var results = api.Embeddings.GetEmbeddingsAsync("A test text for embedding").Result;
            Assert.IsNotNull(results);
            Assert.That(results.Length == 1536);
        }
    }
}
