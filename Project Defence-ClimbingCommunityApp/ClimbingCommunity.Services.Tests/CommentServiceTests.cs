namespace ClimbingCommunity.Services.Tests
{
    using ClimbingCommunity.Data.Models;
    using ClimbingCommunity.Services.Tests.ComparerViewModels;
    using ClimbingCommunity.Services.Tests.Mocking;
    using ClimbingCommunity.Web.ViewModels.Comment;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using WebShopDemo.Core.Data.Common;

    [TestFixture]
    public class CommentServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private CommentService _commentService;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>();
            _commentService = new CommentService(_repositoryMock.Object);
        }
        [Test]
        public async Task Test_AddCommentAsync_ActivityTypeClimbingTrip()
        {
            // Arrange
            var activityId = "06fffbf1-bab3-4729-9b64-8b765c0dc8c9";
            var activityType = "ClimbingTrip";
            var newCommentText = "This is a new comment";
            var userId = "279e0abd-6fcf-425f-8f73-84567d67a9b2";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                           .ReturnsAsync(new ApplicationUser());

            // Act
            await _commentService.AddCommentAsync(activityId, activityType, newCommentText, userId);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync<Comment>(It.IsAny<Comment>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_AddCommentAsync_ActivityTypeTraining()
        {
            // Arrange
            var activityId = "06fffbf1-bab3-4729-9b64-8b765c0dc8c9";
            var activityType = "Training";
            var newCommentText = "This is a new comment";
            var userId = "279e0abd-6fcf-425f-8f73-84567d67a9b2";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                           .ReturnsAsync(new ApplicationUser());

            // Act
            await _commentService.AddCommentAsync(activityId, activityType, newCommentText, userId);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync<Comment>(It.IsAny<Comment>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Test_AddCommentAsync_InvalidActivityType()
        {
            // Arrange
            var activityId = "06fffbf1-bab3-4729-9b64-8b765c0dc8c9";
            var activityType = "Invalid type";
            var newCommentText = "This is a new comment";
            var userId = "279e0abd-6fcf-425f-8f73-84567d67a9b2";

            // Act & Assert

            await _commentService.AddCommentAsync(activityId, activityType, newCommentText, userId);


            _repositoryMock.Verify(repo => repo.AddAsync<Comment>(It.IsAny<Comment>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }

        [Test]
        public async Task Test_AddCommentAsync_UserNotFoundInvalidGuidIdDoNothing()
        {
            // Arrange
            var activityId = "activityId";
            var activityType = "ClimbingTrip";
            var newCommentText = "This is a new comment";
            var userId = "nonExistentUserId";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                           .ReturnsAsync((ApplicationUser)null!);

            // Act & Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _commentService.AddCommentAsync(activityId, activityType, newCommentText, userId);
            });

            _repositoryMock.Verify(repo => repo.AddAsync<Comment>(It.IsAny<Comment>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_AddCommentAsync_ActivityNotFoundInvalidGuidIdDoNothing()
        {

            var activityId = "invalid-id";
            var activityType = "ClimbingTrip";
            var newCommentText = "This is a new comment";
            var userId = "279e0abd-6fcf-425f-8f73-84567d67a9b2";

            // Act & Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _commentService.AddCommentAsync(activityId, activityType, newCommentText, userId);
            });

            _repositoryMock.Verify(repo => repo.AddAsync<Comment>(It.IsAny<Comment>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_AddCommentAsync_InvalidNewCommentTextDoNothing()
        {
            // Arrange
            var activityId = "06fffbf1-bab3-4729-9b64-8b765c0dc8c9";
            var activityType = "ClimbingTrip";
            string newCommentText = null; // Invalid comment text
            var userId = "279e0abd-6fcf-425f-8f73-84567d67a9b2";

            // Act & Assert

            await _commentService.AddCommentAsync(activityId, activityType, newCommentText, userId);


            _repositoryMock.Verify(repo => repo.AddAsync<Comment>(It.IsAny<Comment>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            await Task.CompletedTask;
        }


        [Test]
        public async Task Test_GetActivityForCommentById_ValidClimbingTripActivity()
        {
            // Arrange
            var activityId = "279e0abd-6fcf-425f-8f73-84567d67a9b2";
            var activityType = "ClimbingTrip";
            var climbingTrip = new ClimbingTrip
            {
                Id = Guid.Parse(activityId),
                Title = "Climbing Trip Title",
                PhotoUrl = "photo.jpg"
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(Guid.Parse(activityId)))
                           .ReturnsAsync(climbingTrip);

            // Act
            var result = await _commentService.GetActivityForCommentById(activityId, activityType);

            // Assert
            Assert.That(result.Id, Is.EqualTo(activityId));
            Assert.That(result.Title, Is.EqualTo(climbingTrip.Title));
            Assert.That(result.PhotoUrl, Is.EqualTo(climbingTrip.PhotoUrl));
            Assert.That(result.ActivityType, Is.EqualTo(activityType));
        }
        [Test]
        public async Task Test_GetActivityForCommentById_ValidTrainingActivityId()
        {
            // Arrange
            var activityId = "279e0abd-6fcf-425f-8f73-84567d67a9b2";
            var activityType = "Training";
            var training = new Training
            {
                Id = Guid.Parse(activityId),
                Title = "Training Title",
                PhotoUrl = "training.jpg"
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(Guid.Parse(activityId)))
                           .ReturnsAsync(training);

            // Act
            var result = await _commentService.GetActivityForCommentById(activityId, activityType);

            // Assert
            Assert.That(result.Id, Is.EqualTo(activityId));
            Assert.That(result.Title, Is.EqualTo(training.Title));
            Assert.That(result.PhotoUrl, Is.EqualTo(training.PhotoUrl));
            Assert.That(result.ActivityType, Is.EqualTo(activityType));
        }
        [Test]
        public async Task Test_GetActivityForCommentById_InValidGuidIdTrainingActivity()
        {
            // Arrange
            var activityId = "invalid-id";
            var activityType = "Training";


            // Act and Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _commentService.GetActivityForCommentById(activityId, activityType);
            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_GetActivityForCommentById_InValidGuidIdClimbingTripActivity()
        {
            // Arrange
            var activityId = "invalid-id";
            var activityType = "ClimbingTrip";


            // Act and Assert
            Assert.ThrowsAsync<FormatException>(async () =>
            {
                await _commentService.GetActivityForCommentById(activityId, activityType);
            }, "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");

            await Task.CompletedTask;
        }
        [Test]
        public async Task Test_GetActivityForCommentById_NullActivityType()
        {
            // Arrange
            var activityId = "validClimbingTripId";
            string activityType = null;

            // Act
            var result = await _commentService.GetActivityForCommentById(activityId, activityType);

            // Assert
            Assert.That(result.Id, Is.EqualTo(activityId));
            Assert.IsNull(result.Title);
            Assert.IsNull(result.PhotoUrl);
            Assert.That(result.ActivityType, Is.EqualTo(activityType));
        }
        [Test]
        public void Test_IsActivityTypeExist_ValidActivityType_ClimbingTrip()
        {
            // Arrange
            var activityType = "ClimbingTrip";

            // Act
            var result = _commentService.IsActivityTypeExist(activityType);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_IsActivityTypeExist_ValidActivityType_Training()
        {
            // Arrange
            var activityType = "Training";

            // Act
            var result = _commentService.IsActivityTypeExist(activityType);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_IsActivityTypeExist_InvalidActivityType()
        {
            // Arrange
            var activityType = "InvalidType";

            // Act
            var result = _commentService.IsActivityTypeExist(activityType);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task Test_IsActivityExistsByIdAndTypeAsync_ValidClimbingTripId()
        {
            // Arrange
            ClimbingTrip validClimbingTrip = new ClimbingTrip()
            {
                Id = Guid.NewGuid()
            };
            var activityId = validClimbingTrip.Id;
            var activityType = "ClimbingTrip";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<ClimbingTrip>(activityId))
               .ReturnsAsync(validClimbingTrip);

            // Act
            var result = await _commentService.IsActivityExistsByIdAndTypeAsync(activityId.ToString(), activityType);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsActivityExistsByIdAndTypeAsync_ValidTrainingId()
        {
            // Arrange
            Training validTraining = new Training()
            {
                Id = Guid.NewGuid()
            };
            var activityId = validTraining.Id;
            var activityType = "Training";

            _repositoryMock.Setup(repo => repo.GetByIdAsync<Training>(activityId))
               .ReturnsAsync(validTraining);

            // Act
            var result = await _commentService.IsActivityExistsByIdAndTypeAsync(activityId.ToString(), activityType);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_IsActivityExistsByIdAndTypeAsync_InvalidTrainingIdWithValidType()
        {
            Training validTraining = new Training()
            {
                Id = Guid.NewGuid()
            };
            // Arrange
            var invalidActivityId = "3ea55161-e145-44f0-95dc-97331642ba47";
            var activityType = "Training";

            // Act
            var result = await _commentService.IsActivityExistsByIdAndTypeAsync(invalidActivityId, activityType);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task Test_IsActivityExistsByIdAndTypeAsync_InvalidClimbingTripIdWithValidType()
        {
            // Arrange
            Training validTraining = new Training()
            {
                Id = Guid.NewGuid()
            };
            var invalidActivityId = "3ea55161-e145-44f0-95dc-97331642ba47";
            var activityType = "ClimbingTrip";

            // Act
            var result = await _commentService.IsActivityExistsByIdAndTypeAsync(invalidActivityId, activityType);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Test_IsActivityExistsByIdAndTypeAsync_InvalidTypeWithValidId()
        {
            Training validTraining = new Training()
            {
                Id = Guid.NewGuid()
            };
            // Arrange
            var activityId = validTraining.Id;
            var activityType = "InvalidType";

            // Act
            var result = await _commentService.IsActivityExistsByIdAndTypeAsync(activityId.ToString(), activityType);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task Test_GetAllCommentsByActivityIdAndTypeAsync_ClimbingTrip()
        {
            // Arrange
            var activityId = Guid.Parse("479334bf-b9e3-4661-b45a-14c0917f0411");
            var activityType = "ClimbingTrip";
            var commentsData = GetCommentsForTesting();

            var mockQueryable = new MockAsyncEnumerable<Comment>(commentsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Comment>(It.IsAny<Expression<Func<Comment, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(c => c.CreatedOn)
                           .Where(ct=>ct.ClimbingTripId==activityId));

            var expectedViewModels = commentsData
                .Where(c => c.ClimbingTripId == activityId)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    AuthorId = c.AuthorId,
                    Author = c.Author,
                    CreatedOn = c.CreatedOn,
                });

            // Act
            var result = await _commentService.GetAllCommentsByActivityIdAndTypeAsync(activityId.ToString(), activityType);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new CommentViewModelComparer());
        }

        [Test]
        public async Task Test_GetAllCommentsByActivityIdAndTypeAsync_Training()
        {
            // Arrange
            var activityId = Guid.Parse("25bdebf0-2eee-4650-a7c2-67c5ba2c3b9f");
            var activityType = "Training";
            var commentsData = GetCommentsForTesting();

            var mockQueryable = new MockAsyncEnumerable<Comment>(commentsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Comment>(It.IsAny<Expression<Func<Comment, bool>>>()))
                           .Returns(mockQueryable
                           .OrderByDescending(c => c.CreatedOn)
                           .Where(t => t.TrainingId == activityId));

            var expectedViewModels = commentsData
                .OrderByDescending(c=>c.CreatedOn)
                .Where(c => c.TrainingId == activityId)
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    AuthorId = c.AuthorId,
                    Author = c.Author
                });

            // Act
            var result = await _commentService.GetAllCommentsByActivityIdAndTypeAsync(activityId.ToString(), activityType);

            // Assert
            CollectionAssert.AreEqual(expectedViewModels, result, new CommentViewModelComparer());
        }


        [Test]
        public async Task Test_GetAllCommentsByActivityIdAndTypeAsync_InvalidClimbingTripId_ValidActivityType()
        {
            // Arrange
            var activityId = Guid.Parse("e5129569-0f07-475e-bbf8-9903d699768f");
            var activityType = "ClimbingTrip";
            var commentsData = GetCommentsForTesting();
            var mockQueryable = new MockAsyncEnumerable<Comment>(commentsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Comment>(It.IsAny<Expression<Func<Comment, bool>>>()))
                           .Returns(mockQueryable
                           .Where(c=>c.ClimbingTripId==activityId));

            // Act
            var result = await _commentService.GetAllCommentsByActivityIdAndTypeAsync(activityId.ToString(), activityType);

            // Assert
            Assert.IsEmpty(result);
        }
        [Test]
        public async Task Test_GetAllCommentsByActivityIdAndTypeAsync_InvalidTrainingId_ValidActivityType()
        {
            // Arrange
            var activityId = Guid.Parse("e5129569-0f07-475e-bbf8-9903d699768f");
            var activityType = "Training";
            var commentsData = GetCommentsForTesting();
            var mockQueryable = new MockAsyncEnumerable<Comment>(commentsData);

            _repositoryMock.Setup(repo => repo.AllReadonly<Comment>(It.IsAny<Expression<Func<Comment, bool>>>()))
                           .Returns(mockQueryable
                           .Where(c => c.ClimbingTripId == activityId));

            // Act
            var result = await _commentService.GetAllCommentsByActivityIdAndTypeAsync(activityId.ToString(), activityType);

            // Assert
            Assert.IsEmpty(result);
        }
        private List<Comment> GetCommentsForTesting()
        {
            var comments = new List<Comment>
        {
            new Comment
            {
                Id = 1,
                Text = "Comment 1 for ClimbingTrip",
                AuthorId = "4f3b31e8-df14-4931-9c43-51aae37e6d4d",
                Author = new ApplicationUser { Id = "4f3b31e8-df14-4931-9c43-51aae37e6d4d", UserName = "User1" },
                CreatedOn = DateTime.UtcNow,
                ClimbingTripId = Guid.Parse("479334bf-b9e3-4661-b45a-14c0917f0411"),
                TrainingId = null
            },
            new Comment
            {
                Id = 2,
                Text = "Comment 2 for ClimbingTrip",
                AuthorId = "6da22dfd-995f-4641-9be8-900af665c2e5",
                Author = new ApplicationUser { Id = "6da22dfd-995f-4641-9be8-900af665c2e5", UserName = "User2" },
                CreatedOn = DateTime.UtcNow,
                ClimbingTripId = Guid.Parse("479334bf-b9e3-4661-b45a-14c0917f0411"),
                TrainingId = null
            },
            new Comment
            {
                Id = 3,
                Text = "Comment 1 for Training",
                AuthorId = "8fc6f2a7-67a8-4262-a157-44d274db04ff",
                Author = new ApplicationUser { Id = "8fc6f2a7-67a8-4262-a157-44d274db04ff", UserName = "User3" },
                CreatedOn = DateTime.UtcNow,
                ClimbingTripId = null,
                TrainingId = Guid.Parse("25bdebf0-2eee-4650-a7c2-67c5ba2c3b9f")
            }
        };

            return comments;
        }

    }
}
