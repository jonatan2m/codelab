using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M220N;
using M220N.Controllers;
using M220N.Models;
using M220N.Models.Projections;
using M220N.Models.Responses;
using M220N.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NUnit.Framework;

namespace M220NTests
{
    internal class UserControllerTests
    {
        private UserController _userController;
        private UsersRepository _userRepository;
        private CommentsRepository _commentsRepository;

        [SetUp]
        public void Setup()
        {
            var client = new MongoClient(Constants.MongoDbConnectionUri());
            _userRepository = new UsersRepository(client);
            _commentsRepository = new CommentsRepository(client);
            var jwt = new JwtAuthentication
            {
                SecurityKey = "ouNtF8Xds1jE55/d+iVZ99u0f2U6lQ+AHdiPFwjVW3o=",
                ValidAudience = "https://localhost:5000/",
                ValidIssuer = "https://localhost:5000/"
            };

            var appSettingsOptions = Options.Create(jwt);
            _userController = new UserController(_userRepository, _commentsRepository, appSettingsOptions);
        }

        [Test]
        public async Task TestAddNewUser()
        {
            var user = new User
            {
                Name = "Test User Foo", Email = "testuserfoo@mongodb.com", Password = "hdfn123?"
            };

            try
            {
                var result = await _userController.AddUser(user);
                Assert.IsNotNull(result);
                Assert.AreEqual(typeof(OkObjectResult), result.GetType());
                var okresult = (OkObjectResult) result;
                var returnedUser = (User) okresult.Value;
                Assert.IsNotNull(returnedUser);
                Assert.AreEqual("Test User Foo", returnedUser.Name);
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
        public async Task TestLogsInAndOut()
        {
            var user = new User {Name = "Test User 1000", Email = "testuser10000@mongodb.com", Password = "hdfn123?"};
            var user2 = new User {Name = "Test User Bar", Email = "testuserbar@mongodb.com", Password = "hdfn123?"};

            try
            {
                var result = (OkObjectResult) await _userController.Login(user);
                Assert.IsNotNull(result);

                var resultValue = (UserResponse) result.Value;
                Assert.AreEqual("No user found. Please check the email address.", resultValue.ErrorMessage);

                await _userController.AddUser(user2);
                var loginResult = (OkObjectResult) await _userController.Login(user2);
                Assert.IsNotNull(loginResult);

                var newResultValue = (UserResponse) loginResult.Value;
                Assert.IsNotNull(newResultValue);
                Assert.IsNotNull(newResultValue.AuthToken);

                _userController.ControllerContext =
                    new ControllerContext {HttpContext = new DefaultHttpContext()};
                _userController.ControllerContext.HttpContext.Request.Headers["Authorization"] =
                    "Bearer:" + newResultValue.AuthToken;
                var logoutResult = (OkObjectResult) await _userController.Logout();
                Assert.AreEqual(typeof(UserResponse), logoutResult.Value.GetType());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            finally
            {
                await _userRepository.DeleteUserAsync(user2.Email);
            }
        }

        [Test]
        public async Task TestSetUserPreferences()
        {
            var user = new User
            {
                Name = "UnkeyMonkey",
                Email = "unkeymonkey@mongodb.com",
                Password = "hdfn123?"
            };

            try
            {
                await _userController.AddUser(user);
                var loginResult = (OkObjectResult) await _userController.Login(user);
                Assert.IsNotNull(loginResult);
                var resultValue = (UserResponse) loginResult.Value;

                var preferences = new Dictionary<string, string>()
                {
                    {"pie", "apple"},
                    {"drink", "mocha"}
                };

                _userController.ControllerContext =
                    new ControllerContext {HttpContext = new DefaultHttpContext()};
                _userController.ControllerContext.HttpContext.Request.Headers["Authorization"] =
                    "Bearer:" + resultValue.AuthToken;

                var updateResult = (OkObjectResult)await _userController.UpdateUserPreferences(
                    new PreferencesObject(){Preferences = preferences});

                Assert.AreEqual(200, updateResult.StatusCode);
                Assert.AreEqual(preferences, ((UserResponse)updateResult.Value).User.Preferences);
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
        public async Task TestsUserCommentReport()
        {
            _userController.ControllerContext =
                new ControllerContext { HttpContext = new DefaultHttpContext() };
            _userController.ControllerContext.HttpContext.Request.Headers["Authorization"] =
                "Bearer:thisisnotavalidtoken,yo";

            var result = await _userController.GenerateCommentReport();
            var ok = (OkObjectResult) result;
            var commentResponse = (TopCommentsProjection) ok.Value;

            Assert.AreEqual(20, commentResponse.Report.Count);
            Assert.AreEqual(277, commentResponse.Report.First().Count);
            Assert.AreEqual("roger_ashton-griffiths@gameofthron.es", commentResponse.Report.First().Id);
        }
    }
}
