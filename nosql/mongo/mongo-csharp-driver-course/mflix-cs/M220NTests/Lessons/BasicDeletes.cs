using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M220N.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NLessons
{
    /// <summary>
    /// This Test Class shows the code used in the Basic Deletes lesson.
    ///
    /// In this lesson, we'll look at 3 ways to delete one or more documents
    /// from MongoDB. The methods are nearly identical to the methods we
    /// explored in the Updates lesson: the driver provides synchronous and
    /// asynchronous versions of the DeleteOne, DeleteMany, and FindOneAndDelete
    /// methods. 
    /// 
    /// </summary>
    public class BasicDeletes
    {
        private IMongoCollection<Theater> _theatersCollection;

        [SetUp]
        public void Setup()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            var client = new MongoClient(Constants.MongoDbConnectionUri());
            _theatersCollection = client.GetDatabase("sample_mflix").GetCollection<Theater>("theaters");

            AddTestTheaters();
        }

        [Test]
        public async Task DeleteOneTheaterAsync()
        {
            var allTheatersCount = _theatersCollection.CountDocuments(
                Builders<Theater>.Filter.Empty);

            /*
             * Let's start with deleting a single document. The process for
             * deleting a MongoDB document with the driver is
             * the same as it is for updating a document: you create a filter
             * to specify the document you want to delete, and then you call
             * either the DeleteOne or FindOneAndDelete method. The choice
             * between the two method depends entirely on whether you want to
             * perforn any further logic on the deleted document. Let's look
             * at both here. Here's our first filter, which finds a specific
             * theater by it's TheaterId:
             */

            var filter = Builders<Theater>.Filter.Eq(t => t.TheaterId, 27017);

            // Now we call DeleteOneAsync, passing in the filter:

            var result = await _theatersCollection.DeleteOneAsync(filter);

            /* We can confirm that the theater was deleted in a few ways:
             * - We can check the DeleteResult.DeletedCount that is returned and
             *   ensure it is "1", or 
             * - We can compare the number of theaters in the collection now to
             *   what the count was before the delete. We don't need to know the
             *   actual value; we just want to make sure it decresed by 1 theater.
            */

            Assert.AreEqual(1, result.DeletedCount);

            Assert.AreEqual(allTheatersCount - 1,
                _theatersCollection
                .CountDocuments(Builders<Theater>.Filter.Empty));

            /* We can also delete a single document with FindOneAndDelete, which
             * does the same thing as above, but the driver returns the deleted
             * document to us.
             */

            filter = Builders<Theater>.Filter.Eq(t => t.TheaterId, 27018);

            var deletedDoc = await _theatersCollection
                .FindOneAndDeleteAsync(filter);

            // And now we can check the deleted document's properties:

            Assert.AreEqual(27018, deletedDoc.TheaterId);
            Assert.AreEqual("Wishywashy", deletedDoc.Location.Address.City);

            // We now expect the total count to be 2 less than when we started:
            Assert.AreEqual(allTheatersCount - 2,
                _theatersCollection
                .CountDocuments(Builders<Theater>.Filter.Empty));

        }

        [Test]
        public async Task DeleteManyTheatersAsync()
        {
            /* By now, you probably have a good guess about how to use the driver
             * to delete multiple documents. If you'd like, stop the video
             * here and give it a try. Otherwise, let's take a look at how
             * we do it. First, we'll create the find filter:
             */

            var filter = Builders<Theater>.Filter.Eq(t => t.Location.Address.City, "Movieville");

            // And then we call the DeleteMany or DeleteManyAsync method:

            var result = await _theatersCollection.DeleteManyAsync(filter);

            Assert.AreEqual(3, result.DeletedCount);

            /* And that's it for Deletes! If you want to see one more example,
             * take look at the Cleanup method below. It removes all of the
             * test documents that were creted at the start of this unit test.
             * It uses the "Gte" ("Greater than or equal to") filter to
             * match the theaters we added, and deletes all of them in a
             * single line of code.
             */
        }

        [TearDown]
        public void Cleanup()
        {
            _theatersCollection.DeleteMany(Builders<Theater>.Filter.Gte(t => t.TheaterId, 20000));

        }

        /// <summary>
        /// Intentionally *not* async becasue we want to make sure the data is
        /// in MongoDB before runnning our tests!
        /// </summary>
        private void AddTestTheaters()
        {
            var theater1 = new Theater(27017, "1 Foo Street", "Dingledongle", "DE", "45678");
            var theater2 = new Theater(27018, "234 Bar Ave.", "Wishywashy", "WY", "87654");
            var theater3 = new Theater(27019, "75th Birthday Road", "Viejo Amigo", "CA", "99887");
            var theater4 = new Theater(27020, "1 Theater Row", "Movieville", "CT", "10101");
            var theater5 = new Theater(27021, "2 Theater Row", "Movieville", "CT", "10101");
            var theater6 = new Theater(27022, "3 Theater Row", "Movieville", "CT", "10101");

            _theatersCollection.InsertMany(
                new List<Theater>()
                {
                    theater1, theater2, theater3, theater4, theater5, theater6
                }
            );
        }
    }
}
