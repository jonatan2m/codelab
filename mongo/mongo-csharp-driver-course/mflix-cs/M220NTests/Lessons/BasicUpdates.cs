using System;
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
    /// This Test Class shows the code used in the Basic Updates lesson.
    ///
    /// In this lesson, we'll look at a few different ways we can update
    /// existing data in MongoDB by using the driver.
    /// 
    /// </summary>
    ///

    public class BasicUpdates
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
        public async Task UpdateOneTheaterAsync()
        {
            /*
             * Let's begin by looking at the methods available to us
             * if we want to update one or more records in MongoDB. We have
             * synchronous and asynchronous methods for UpdateOne, UpdateMany,
             * and FindOneAndUpdate. 
             * The name of each method should provide a good hint as to its
             * purpose. UpdateOne updates a single document, while UpdateMany
             * updates all of the documents that match the filter. Both
             * methods return an UpdateResult object, while the FindOneAndUpdate
             * method updates a single document and returns the updated document. 
             *
             * Let's suppose that one of the movie theaters in our database has
             * moved, so we need to change the address we have on record. This
             * is a obvious case for using UpdateOne or FindOneAndUpdate, so let's
             * look at both of those. First, we create the filter to find the
             * specific theater we need to update:
             */

            var filter = Builders<Theater>.Filter.Eq(t => t.TheaterId, 8);

            /*
             * Now let's call Find just to make sure we're finding the correct
             * document:
             */

            var theater = await _theatersCollection.Find<Theater>(filter).FirstOrDefaultAsync();

            Assert.AreEqual(theater.TheaterId, 8);
            Assert.AreEqual("14141 Aldrich Ave S", theater.Location.Address.Street1);

            /* We don't need to call Find before doing an update; this is just
             * to ensure that we've built our filter correctly. Instead, 
             * we can call UpdateOne or FindOneAndUpdate to change the address
             * of the theater. The UpdateDefinition uses the $set command to
             * change just the "location.address.street1" field in the document. 
             */

            var updateResult = _theatersCollection.UpdateOne(filter,
              new BsonDocument("$set",
                    new BsonDocument("location.address.street1", "123 Main St."))
              );

            Assert.AreEqual(1, updateResult.MatchedCount);
            Assert.AreEqual(1, updateResult.ModifiedCount);

            /* Note: this can also be written using the Builders class, which
            * helps ensure we are using the correct the field name:
            */

            _theatersCollection.UpdateOne(filter,
             Builders<Theater>.Update.Set(t => t.Location.Address.Street1,
                 "123 Main St.")
             );


            /* Finally, if you want to do something with the updated document, you
             * can use the FindOneAndUpdate method:
             */

            var updatedDoc = await _theatersCollection.FindOneAndUpdateAsync<Theater>(
                filter,
                Builders<Theater>.Update.Set(
                    t => t.Location.Address.Street1,
                    "123 Main St.")
                );

            Assert.AreEqual("123 Main St.", updatedDoc.Location.Address.Street1);
        }


        /* Now, what if we want to update many documents? As an example, the
         * state of Minnesota has updated one of its ZIP code locations from
         * Minneapolis to the neighboring city of Bloomington. So we want an
         * operation that will find all of the theaters in the 55111 ZIP code
         * and set the "city" field to "Bloomington". 
         */

        [Test]
        public async Task UpdateManyTheatersAsync()
        {
            // So let's first create the filter:
            var filter = Builders<Theater>
                .Filter.Eq(t => t.Location.Address.Zipcode, "55111");

            // And create the update we want to make:
            var update = Builders<Theater>
                .Update.Set(t => t.Location.Address.City, "Bloomington");

            // And then we run UpdateManyAsync:
            var result = await _theatersCollection.UpdateManyAsync(
                filter,
                update
               );

            Assert.IsTrue(result.IsAcknowledged);
            Assert.AreEqual(6, result.ModifiedCount);
        }




        /* This TearDown method just resets the data to its original state so
         * future tests don't fail.
         */
        [TearDown]
        public void Cleanup()
        {
            _theatersCollection.UpdateOne(Builders<Theater>.Filter.Eq(t => t.TheaterId, 8),
                  new BsonDocument("$set",
                        new BsonDocument("location.address.street1", "14141 Aldrich Ave S"))
                  );

            var result = _theatersCollection.UpdateMany(
                Builders<Theater>.Filter.Eq(t => t.Location.Address.Zipcode, "55111"),
                Builders<Theater>
                .Update.Set(t => t.Location.Address.City, "Minneaplois")
               );
        }
    }
}
