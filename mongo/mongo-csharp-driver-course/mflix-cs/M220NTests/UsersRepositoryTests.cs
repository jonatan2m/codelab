using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using M220N;
using M220N.Controllers;
using M220N.Models;
using M220N.Repositories;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NTests
{
    internal class UsersRepositoryTests
    {
        private UsersRepository _userRepository;
        private IMongoCollection<Session> _sessionsCollection;

        [SetUp]
        public void Setup()
        {
            var client = new MongoClient(Constants.MongoDbConnectionUri());
            _userRepository = new UsersRepository(client);
            _sessionsCollection = client.GetDatabase("sample_mflix").GetCollection<Session>("sessions");
        }

        [Test]
        public async Task TestReturnsUser()
        {
            var user = await _userRepository.GetUserAsync("sean_bean@gameofthron.es");
            Assert.AreEqual("Ned Stark", user.Name);
        }

        [Test]
        public async Task TestCreatesNewUser()
        {
            var user = new User { Name = "Test User 1", Email = "testuser1@mongodb.com", Password = "hdfn123?" };

            try
            {
                var result = await _userRepository.AddUserAsync(user.Name, user.Email, user.Password);

                Assert.IsNotNull(result.User);

                var getUser = await _userRepository.GetUserAsync(user.Email);
                Assert.AreEqual(result.User.Name, getUser.Name);
                Assert.AreEqual(result.User.Email, getUser.Email);
                Assert.IsNull(getUser.Password);
                Assert.IsNotNull(result.User.HashedPassword);

                result = await _userRepository.AddUserAsync(user.Name, user.Email, user.Password);

                Assert.IsNotNull(result.ErrorMessage);
                Assert.IsTrue(result.ErrorMessage.Contains("E11000 duplicate key error"), "Duplicate key error is expected.");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                await _userRepository.DeleteUserAsync(user.Email);
            }
        }

        [Test]
        public async Task TestLogsInUser()
        {
            
            var jwt = new JwtAuthentication
            {
                SecurityKey = "ouNtF8Xds1jE55/d+iVZ99u0f2U6lQ+AHdiPFwjVW3o=",
                ValidAudience = "https://localhost:5000/",
                ValidIssuer = "https://localhost:5000/"
            };

            var user = new User { Name = "Test User 2",
                Email = "testuser2@mongodb.com",
                Password = "hdfn123?",
                AuthToken = jwt.SecurityKey };

            try
            {
                var result = await _userRepository.LoginUserAsync(user);
                Assert.IsNotNull(result.ErrorMessage);
                Assert.AreEqual("No user found. Please check the email address.",
                    result.ErrorMessage);

                var addUserResult = await _userRepository.AddUserAsync(user.Name,
                    user.Email, user.Password);
                Assert.IsNotNull(addUserResult.User);

                result = await _userRepository.LoginUserAsync(user);
                Assert.IsNotNull(result.User);
                Assert.AreEqual("Test User 2", result.User.Name);
                Assert.AreEqual("ouNtF8Xds1jE55/d+iVZ99u0f2U6lQ+AHdiPFwjVW3o=",
                    result.User.AuthToken);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                //cleanup
                await _userRepository.LogoutUserAsync(user.Email);
                await _userRepository.DeleteUserAsync(user.Email);
            }
        }

        [Test]
        public async Task TestUpdateUserPreferences()
        {
            var user = new User { Name = "Test User P",
                Email = "testuserp@mongodb.com",
                Password = "hdfn123?" };

            try
            {
                await _userRepository.AddUserAsync(user.Name, user.Email, user.Password);
                var preferences = new Dictionary<string, string>()
                {
                    {"pie", "pumpkin"},
                    {"drink", "latte"}
                };

                var result = await _userRepository.SetUserPreferencesAsync(user.Email, preferences);

                Assert.IsNull(result.ErrorMessage);
                Assert.AreEqual("True", result.SuccessMessage);

                var fetchedUser = await _userRepository.GetUserAsync(user.Email);

                Assert.AreEqual(preferences, fetchedUser.Preferences);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                await _userRepository.LogoutUserAsync(user.Email);
            }
        }

        [Test]
        public async Task TestLogsOutUser()
        {
            var user = new User {Name = "Test User 2",
                Email = "testuser2@mongodb.com",
                Password = "hdfn123?"};
            var result = await _userRepository.LogoutUserAsync(user.Email);
            Assert.AreEqual("User logged out.", result.SuccessMessage);

            var loggedOutUser = _sessionsCollection.Find<Session>(s => s.UserId == "testuser2@mongodb.com")
                .FirstOrDefault();

            Assert.IsNull(loggedOutUser);
        }

        [Test]
        public async Task TestsMakeAdmin()
        {
            var user = new User { Name = "Admin User 1",
                Email = "adminuser1@mongodb.com",
                Password = "hdfn123?" };

            try
            {
                var newAdmin = await _userRepository.MakeAdmin(user);
                Assert.AreEqual(user.Email, newAdmin.Email);
                Assert.IsTrue(newAdmin.IsAdmin);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                await _userRepository.DeleteUserAsync(user.Email);
            }
        }

    }
}
