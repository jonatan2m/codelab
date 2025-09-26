using System;
using System.Linq;
using System.Threading.Tasks;
using M220N;
using M220N.Controllers;
using M220N.Models;
using M220N.Models.Responses;
using M220N.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Exception = System.Exception;
namespace M220NTests
{
    internal class CommentControllerTests
    {
        private CommentsRepository _commentRepository;
        private UsersRepository _userRepository;
        private MoviesRepository _movieRepository;
        private UserController _userController;
        private CommentController _commentController;
        private User _opinionatedUser;
        private User _anotherUser;
        private string _movieId;
        private ObjectId _commentId;
        [SetUp]
        public void Setup()
        {
            var client = new MongoClient(Constants.MongoDbConnectionUri());
            _userRepository = new UsersRepository(client);
            _commentRepository = new CommentsRepository(client);
            _movieRepository = new MoviesRepository(client);
            _opinionatedUser = new User
            {
                Name = "Inigo Montoya",
                Email = "youkiltmyfodder@mongodb.com",
                Password = "hdfn123?"
            };
            _anotherUser = new User
            {
                Name = "Vizzini",
                Email = "thesicilian@sicily.it",
                Password = "iocanepower"
            };
            _movieId = "573a1398f29313caabcea974";
            var jwt = new JwtAuthentication
            {
                SecurityKey = "ouNtF8Xds1jE55/d+iVZ99u0f2U6lQ+AHdiPFwjVW3o=",
                ValidAudience = "https://localhost:5000/",
                ValidIssuer = "https://localhost:5000/"
            };
            var appSettingsOptions = Options.Create(jwt);
            _userController = new UserController(_userRepository, _commentRepository, appSettingsOptions);
            _commentController = new CommentController(_commentRepository, _userRepository, appSettingsOptions);
        }
        [Test]
        public async Task TestAddsComment()
        {
            var existing = await _movieRepository.GetMovieAsync(_movieId);
            var numberOfComments = existing.Comments.Count;
            var commentText = "I do not think this movie means what you think it means.";
            try
            {
                var result = await AddCommentToMovieAsync(commentText);
                var okResult = (OkObjectResult)result;
                Assert.AreEqual(200, okResult.StatusCode);
                var comments = (CommentResponse)okResult.Value;
                Assert.AreEqual(numberOfComments + 1, comments.Comments.Count);
                this._commentId = comments.Comments.First().Id;
                await GetMovieAndVerifyChanges(commentText, numberOfComments);
            }
            catch (Exception ex)
            {
                await _commentRepository.DeleteCommentAsync(new ObjectId(_movieId),
                    _commentId, _opinionatedUser);
                Assert.Fail(ex.Message);
            }
            finally
            {
                await Cleanup();
            }
        }
        [Test]
        public async Task TestUpdatesComment()
        {
            var existing = await _movieRepository.GetMovieAsync(_movieId);
            var numberOfComments = existing.Comments.Count;
            var oldCommentText = "I do not think this movie means what you think it means.";
            var newCommentText = "He's right on top of us! I wonder if he is using the same wind we are using...";
            try
            {
                await AddCommentToMovieAsync(oldCommentText);
                var movie = await _movieRepository.GetMovieAsync(_movieId);
                var firstComment = movie.Comments.FirstOrDefault();
                _commentId = firstComment.Id;
                Assert.IsNotNull(firstComment);
                Assert.AreEqual(oldCommentText, firstComment.Text);
                var controllerWithAuth = await GetControllerWithAuthAsync();
                var result = await controllerWithAuth.UpdateCommentAsync(
                    new MovieCommentInput()
                    {
                        CommentId = _commentId.ToString(),
                        UpdatedComment = newCommentText,
                        MovieId = _movieId
                    });
                var okResult = (OkObjectResult)result;
                Assert.IsNotNull(okResult);
                Assert.AreEqual(200, okResult.StatusCode);
                await GetMovieAndVerifyChanges(newCommentText, numberOfComments);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                await Cleanup();
            }
        }
        [Test]
        public async Task DeleteComment()
        {
            var existing = await _movieRepository.GetMovieAsync(_movieId);
            var numberOfComments = existing.Comments.Count;
            var commentText = "I do not think this movie means what you think it means.";
            try
            {
                var result = await AddCommentToMovieAsync(commentText);
                var okResult = (OkObjectResult)result;
                var comments = (CommentResponse)okResult.Value;
                this._commentId = comments.Comments.First().Id;
                await GetMovieAndVerifyChanges(commentText, numberOfComments);
                var movie = await _commentRepository.DeleteCommentAsync(new ObjectId(_movieId),
                                _commentId, _anotherUser);
                Assert.IsNotNull(movie);
                Assert.AreEqual(numberOfComments + 1, movie.Comments.Count);
                movie = await _commentRepository.DeleteCommentAsync(new ObjectId(_movieId),
                    _commentId, _opinionatedUser);
                Assert.IsNotNull(movie);
                Assert.AreEqual(numberOfComments, movie.Comments.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                await Cleanup();
            }
        }
        [TearDown]
        public async Task Cleanup()
        {
            await _commentRepository.DeleteCommentAsync(new ObjectId(_movieId),
                _commentId, _opinionatedUser);
        }
        private async Task GetMovieAndVerifyChanges(string commentText, int expected)
        {
            var movie = await _movieRepository.GetMovieAsync(_movieId);
            Assert.IsNotNull(movie);
            Assert.AreEqual(expected + 1, movie.Comments.Count);
            var comment = movie.Comments.First();
            Assert.AreEqual(commentText, comment.Text);
            Assert.AreEqual(_opinionatedUser.Name, comment.Name);
            Assert.AreEqual(_opinionatedUser.Email, comment.Email);
            Assert.AreEqual(new ObjectId(_movieId), comment.MovieId);
            Assert.AreEqual(DateTime.UtcNow.Date, comment.Date.Date);
        }
        private async Task<ActionResult> AddCommentToMovieAsync(string comment)
        {
            var controllerWithAuth = await GetControllerWithAuthAsync();
            return await controllerWithAuth.AddComment(new MovieCommentInput()
            {
                Comment = comment,
                MovieId = _movieId
            });
        }
        private async Task<CommentController> GetControllerWithAuthAsync()
        {
            await _userController.AddUser(_opinionatedUser);
            var loginResult = (OkObjectResult)await _userController.Login(_opinionatedUser);
            var newResultValue = (UserResponse)loginResult.Value;
            var newCommentController = _commentController;
            newCommentController.ControllerContext =
                new ControllerContext { HttpContext = new DefaultHttpContext() };
            newCommentController.ControllerContext.HttpContext.Request.Headers["Authorization"] =
                "Bearer:" + newResultValue.AuthToken;
            return newCommentController;
        }
    }
}
