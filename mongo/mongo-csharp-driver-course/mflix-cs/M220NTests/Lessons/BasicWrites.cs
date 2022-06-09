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
    /// This Test Class shows the code used in the Basic Writes lesson.
    ///
    /// Welcome back. In this lesson, we'll explore writing new
    /// documents to a MongoDB collection.
    /// 
    /// </summary>
    public class BasicWrites

    {
        private IMongoCollection<Theater> _theatersCollection;

        [SetUp]
        public void Setup()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            var client = new MongoClient(Constants.MongoDbConnectionUri());
            _theatersCollection = client.GetDatabase("sample_mflix").GetCollection<Theater>("theaters");
        }

        [Test]
        public async Task CreateMovieAsync()
        {
            /* There are two methods for writing documents with the MongoDB
             * driver: InsertOne and InsertMany, as well as the Async versions
             * of those two methods. In this lesson, I'll show you how to use
             * the Async methods in a couple of different ways.
             *
             * Let's start with a simple example. We want to add a new movie
             * theater to the "theaters" collection. Let's first take a look at
             * the Theater mapping class. It has Id, TheaterId, and Location
             * properties. The Location property is an object that contains an
             * Address object and Geo information, which we won't worry about
             * in this lesson.
             *
             * With that bit of information, let's create a new instance of the
             * Theater class, using the constructor that takes in a TheaterId and
             * various Address components:
             *
             */

            var newTheater = new Theater(27777,
                "4 Privet Drive",
                "Little Whinging",
                "LA",
                "343434");

            // And now we call InsertOneAsync() to add it to the collection:
            await _theatersCollection.InsertOneAsync(newTheater);

            /* Here's something you should be aware of. In the constructor for
             * the Theater object, we don't set the Id property. Instead,
             * we let MongoDB create one and assign it for us as part of the
             * Insert call. We can check that property as soon as the InsertOne
             * returns:
             */

            Assert.IsNotEmpty(newTheater.Id.ToString());


            /* Adding multiple theaters to the collection is just as easy.
             * Let's add a group of 3 new theaters that have just been built.
             * First, we create the Theater objects:
             */

            var theater1 = new Theater(27017, "1 Foo Street", "Dingledongle", "DE", "45678");
            var theater2 = new Theater(27018, "234 Bar Ave.", "Wishywashy", "WY", "87654");
            var theater3 = new Theater(27019, "75 Birthday Road", "Viejo Amigo", "CA", "99887");

            /* And then we call InsertManyAsync, passing in a new List of
             * those Theater objects:
             */

            await _theatersCollection.InsertManyAsync(
                new List<Theater>()
                {
                    theater1, theater2, theater3
                }
            );

            /* Let's make sure everything worked. */

            var newTheaters = await _theatersCollection.Find<Theater>(
                    Builders<Theater>.Filter.Gte(t => t.TheaterId, 27017)
                    & Builders<Theater>.Filter.Lte(t => t.TheaterId, 27019))
                .ToListAsync();

            Assert.AreEqual(3, newTheaters.Count);
            Assert.AreEqual("1 Foo Street",
                newTheaters.First().Location.Address.Street1);
            Assert.AreEqual("Viejo Amigo",
                newTheaters.Last().Location.Address.City);


            /* So that's all there is to writing new documents to a MongoDB
             * collection. In the next lesson, we'll look at updating
             * existing documents.
             */
             
        }

        [TearDown]
        public void Cleanup()
        {
            _theatersCollection.DeleteMany(t => t.TheaterId >= 27017);
        }
    }
}
