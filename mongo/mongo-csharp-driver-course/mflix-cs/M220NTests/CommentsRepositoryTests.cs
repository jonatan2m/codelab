using System.Linq;
using System.Threading.Tasks;
using M220N.Repositories;
using MongoDB.Driver;
using NUnit.Framework;


namespace M220NTests
{
    class CommentsRepositoryTests
    {


        private CommentsRepository _commentsRepository;

        [SetUp]
        public void Setup()
        {
            var client = new MongoClient(Constants.MongoDbConnectionUri());
            _commentsRepository = new CommentsRepository(client);
        }

        [Test]
        public async Task TestGetsTopTwenty()
        {
            var result = await _commentsRepository.MostActiveCommentersAsync();
            Assert.AreEqual(20, result.Report.Count);
            Assert.AreEqual(277, result.Report.First().Count);
            Assert.AreEqual(292, result.Report.Last().Count);
        }
    }
}
