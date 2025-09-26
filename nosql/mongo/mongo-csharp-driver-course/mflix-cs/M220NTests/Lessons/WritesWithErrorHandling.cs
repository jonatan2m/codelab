using System;
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
    /// 
    /// In this lesson, we'll look at how we handle the common exceptions
    /// that might arrise when using the driver to write to MongoDB. 
    /// We'll look at a 2 different types errors and how to handle them
    /// gracefully in your code. This way, we can ensure your applicaiton
    /// is resilient to issues that occur in concurrent and distributed
    /// systems.
    /// 
    /// </summary>
    public class WritesWithErrorHandling
    {
        private IMongoClient _client;
        private IMongoCollection<Theater> _theatersCollection;

        [SetUp]
        public void Setup()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _client = new MongoClient(Constants.MongoDbConnectionUri());
            _theatersCollection = _client.GetDatabase("sample_mflix").GetCollection<Theater>("theaters");
        }

        [Test]
        public async Task InsertWithDuplicateKey()
        {

            /* In a concurrent system, your app might try to save a document that
             * has a duplicate value on a field that is marked unique. When this
             * happens, MongoDB returns a Duplicate Key error.
             *
             * To show this, let's add a Theater document to a collection, and
             * then add a new document with the same Id value as the first
             * Theater. We're using the Id field because it maps to the _id field 
             * in Mongo, and _id is a field that is always marked as unique. 
             */

            var testTheater = new Theater(10101010, "234 Bar Ave.", "Wishywashy", "WY", "87654");
            await _theatersCollection.InsertOneAsync(testTheater);

            /* You may recall from a previous lesson that in our Theater class,
             * we don't assign a value to the Id field; we let MongoDB do that
             * for us. When the call to InsertOne or InsertOneAsync returns,
             * our new Theater object has that field populated for us!
             *
             * So let's now use that existing Id value to create and insert a
             * new Theater object:
             */

            var myNewTheater = new Theater() { Id = testTheater.Id };

            /* This call should fail with an exception:
             */

            Assert.Throws<MongoWriteException>(() =>
                _theatersCollection.InsertOne(myNewTheater));

            /* Now that we've confirmed this, we can wrap our writes in
             * try-catch blocks to handle exceptions accordingly. While a Duplicate
             * Key error is unlikely to happen, especially if you let MongoDB
             * handle assigning a value to the _id field for you, it's always a
             * best practice to use try-catch blocks. And if you are managing
             * a field of unique values in your code, you'll definitely want
             * to be handling this exception!
             */

            try
            {
                await _theatersCollection.InsertOneAsync(myNewTheater);
            }

            catch (MongoWriteException ex)
            {
                /* There are, of course, other potential write errors, so
                 * let's make sure we really caught a dupliate key error:
                 */
                Assert.IsTrue(ex.Message.Contains("E11000 duplicate key error"));

                /* If you have a custom process for generating unique key values,
                 * this would be a good place to change the Id value and re-try
                 * the Insert.
                 */
            }

        }

        [Test]
        public void TimeoutException()
        {
            /* The second type of exception we'll look at is a timeout error,
             * which occurs if your app is experiencing network latency. When
             * working with Atlas, it is very hard to actually make this error
             * happen with a valid URI, so we'll use a bad URI value to force
             * the error. Unfortunately, the timeout takes 30 seconds by default,
             * so we'll use a bit of video magic to skip ahead.
             * 
             */


            var badClient = new MongoClient(Constants.BadMongoDbConnectionUri);

            var fastTheaterCollection = badClient.GetDatabase("sample_mflix")
                .GetCollection<Theater>("theaters");

            try
            {
                // NOTE: uncomment only for this lesson. Otherwise, your unit
                // tests will stall for 30 seconds every time!

                // fastTheaterCollection.InsertOne(new Theater() { TheaterId = 27055 });
            }
            catch (TimeoutException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("A timeout occured after 30000ms"));
            }
        }

        [TearDown]
        public void Cleanup()
        {
            _theatersCollection.DeleteMany(Builders<Theater>.Filter.Gte(t => t.TheaterId, 27017));
        }

            private async Task<List<Theater>> AddTestTheatersAsync()
        {
            var theater1 = new Theater(27017, "1 Foo Street", "Dingledongle", "DE", "45678");
            var theater2 = new Theater(27018, "234 Bar Ave.", "Wishywashy", "WY", "87654");
            var theater3 = new Theater(27019, "75 Birthday Road", "Viejo Amigo", "CA", "99887");
            var theater4 = new Theater(27020, "1 Theater Row", "Movieville", "CT", "10101");
            var theater5 = new Theater(27021, "2 Theater Row", "Movieville", "CT", "10101");
            var theater6 = new Theater(27022, "3 Theater Row", "Movieville", "CT", "10101");

            await _theatersCollection.InsertManyAsync(
                new List<Theater>()
                {
                    theater1, theater2, theater3, theater4, theater5, theater6
                }
            );

            return await _theatersCollection
                .Find(Builders<Theater>.Filter.Gte(t => t.TheaterId, 27017))
                .ToListAsync();
        }
    }
}
