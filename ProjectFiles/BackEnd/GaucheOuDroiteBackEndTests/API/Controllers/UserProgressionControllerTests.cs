using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.API.Controllers;
using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Security.DTOValidators;
using GaucheOuDroiteBackEnd.Services;

using GaucheOuDroiteBackEndTests.Tools;

using Shared.DTOs;
using Shared.DTOs.Data.User;


namespace GaucheOuDroiteBackEndTests.API.Controllers
{
    [TestClass]
    public sealed class UserProgressionControllerTests
    {
        TestDataBase _dataBase = null!;

        UserProgressionController _userProgressionController = null!;

        UserProgressionService _userProgressionService = null!;

        List<Level> _levels = null!;


        const int CURRENT_USER_ID = 1;
        const int OTHER_USER_ID = 2;


        // -- Setup -- //

        [TestInitialize]
        public async Task TestInit()
        {
            _dataBase = new TestDataBase();

            _levels =
            [
                new Level
                {
                    Id = 1,
                    Name = "Level 1",
                    Difficulty = 1,
                    IsInfinite = false,
                    ResponseSequence = "LRLR",
                    Star1MinimumScore = 10,
                    Star2MinimumScore = 20,
                    Star3MinimumScore = 30
                },

                new Level
                {
                    Id = 2,
                    Name = "Level 2",
                    Difficulty = 2,
                    IsInfinite = false,
                    ResponseSequence = "RLRL",
                    Star1MinimumScore = 20,
                    Star2MinimumScore = 30,
                    Star3MinimumScore = 40
                }
            ];

            _dataBase.Context.Levels.AddRange(_levels);

            _dataBase.Context.Users.AddRange(
                new User
                {
                    Id = CURRENT_USER_ID,
                    Username = "CurrentUser",
                    PasswordHash = "Hash"
                },

                new User
                {
                    Id = OTHER_USER_ID,
                    Username = "OtherUser",
                    PasswordHash = "Hash"
                }
            );

            await _dataBase.Context.SaveChangesAsync();


            LevelService levelService = new(_dataBase.Context);

            LevelProgressionDTOValidator levelProgressionDTOValidator = new(levelService);

            UserProgressionDTOValidator userProgressionDTOValidator = new(
                levelService,
                levelProgressionDTOValidator
            );

            UserService userService = new(_dataBase.Context);

            _userProgressionService = new(
                _dataBase.Context,
                userService
            );

            _userProgressionController = new(
                userProgressionDTOValidator,
                _userProgressionService,
                _dataBase.Context
            );

            SetCurrentUser(CURRENT_USER_ID);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            _dataBase.Dispose();
        }


        void SetCurrentUser(int p_userId)
        {
            ClaimsIdentity identity = new(
                [
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        p_userId.ToString()
                    )
                ],
                "TestAuthentication"
            );

            _userProgressionController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };
        }


        static LevelProgressionDTO CreateLevelProgressionDTO(int p_id, int p_levelId, bool p_isUnlocked = false, int p_bestScore = 0)
        {
            return new()
            {
                Id = p_id,
                LevelId = p_levelId,
                IsUnlocked = p_isUnlocked,
                BestScore = p_bestScore
            };
        }


        static UserProgressionDTO CreateValidUserProgressionDTO(int p_firstProgressionId, int p_secondProgressionId)
        {
            return new()
            {
                LevelProgressions =
                [
                    CreateLevelProgressionDTO(
                        p_id: p_firstProgressionId,
                        p_levelId: 1,
                        p_isUnlocked: true,
                        p_bestScore: 10
                    ),

                    CreateLevelProgressionDTO(
                        p_id: p_secondProgressionId,
                        p_levelId: 2,
                        p_isUnlocked: false,
                        p_bestScore: 20
                    )
                ]
            };
        }


        // -- Tests -- //

        [TestMethod]
        public async Task Get_UserHasProgressions_ReturnsOkWithProgressions()
        {
            _dataBase.Context.UserProgressions.AddRange(
                new UserProgression
                {
                    Id = 1,
                    UserId = CURRENT_USER_ID,
                    LevelId = 1,
                    IsUnlocked = true,
                    BestScore = 15
                },

                new UserProgression
                {
                    Id = 2,
                    UserId = CURRENT_USER_ID,
                    LevelId = 2,
                    IsUnlocked = false,
                    BestScore = 20
                }
            );

            await _dataBase.Context.SaveChangesAsync();

            _dataBase.Context.ChangeTracker.Clear();


            IActionResult actionResult = await _userProgressionController.Get();

            OkObjectResult okResult = actionResult as OkObjectResult ?? throw new AssertFailedException(
                "The controller should return OkObjectResult."
            );

            Assert.AreEqual(
                StatusCodes.Status200OK,
                okResult.StatusCode
            );

            GetUserProgressionResponseDTO getUserProgressionResponseDTO = okResult.Value as GetUserProgressionResponseDTO ?? throw new AssertFailedException(
                "The ApiResponseDTO should be a GetUserProgressionResponseDTO."
            );

            Assert.IsTrue(getUserProgressionResponseDTO.HasSucceeded);
            Assert.HasCount(2, getUserProgressionResponseDTO.LevelProgressions);

            Assert.AreEqual(1, getUserProgressionResponseDTO.LevelProgressions[0].Id);
            Assert.AreEqual(1, getUserProgressionResponseDTO.LevelProgressions[0].LevelId);
            Assert.IsTrue(getUserProgressionResponseDTO.LevelProgressions[0].IsUnlocked);
            Assert.AreEqual(15, getUserProgressionResponseDTO.LevelProgressions[0].BestScore);

            Assert.AreEqual(2, getUserProgressionResponseDTO.LevelProgressions[1].Id);
            Assert.AreEqual(2, getUserProgressionResponseDTO.LevelProgressions[1].LevelId);
            Assert.IsFalse(getUserProgressionResponseDTO.LevelProgressions[1].IsUnlocked);
            Assert.AreEqual(20, getUserProgressionResponseDTO.LevelProgressions[1].BestScore);
        }


        [TestMethod]
        public async Task Get_UserHasNoProgressions_ReturnsOkWithEmptyList()
        {
            IActionResult actionResult = await _userProgressionController.Get();

            OkObjectResult okResult = actionResult as OkObjectResult ?? throw new AssertFailedException(
                "The controller should return OkObjectResult."
            );

            Assert.AreEqual(
                StatusCodes.Status200OK,
                okResult.StatusCode
            );

            GetUserProgressionResponseDTO getUserProgressionResponseDTO = okResult.Value as GetUserProgressionResponseDTO ?? throw new AssertFailedException(
                "The apiResponseDTO should be a GetUserProgressionResponseDTO."
            );

            Assert.IsTrue(getUserProgressionResponseDTO.HasSucceeded);
            Assert.IsEmpty(getUserProgressionResponseDTO.LevelProgressions);
        }


        [TestMethod]
        public async Task Put_ValidProgressions_UpdatesDatabaseAndReturnsOk()
        {
            _dataBase.Context.UserProgressions.AddRange(
                new UserProgression
                {
                    Id = 1,
                    UserId = CURRENT_USER_ID,
                    LevelId = 1,
                    IsUnlocked = true,
                    BestScore = 5
                },

                new UserProgression
                {
                    Id = 2,
                    UserId = CURRENT_USER_ID,
                    LevelId = 2,
                    IsUnlocked = false,
                    BestScore = 10
                }
            );

            await _dataBase.Context.SaveChangesAsync();

            _dataBase.Context.ChangeTracker.Clear();


            UserProgressionDTO userProgressionDTO = CreateValidUserProgressionDTO(
                p_firstProgressionId: 1,
                p_secondProgressionId: 2
            );

            userProgressionDTO.LevelProgressions[0].BestScore = 25;
            userProgressionDTO.LevelProgressions[1].IsUnlocked = true;
            userProgressionDTO.LevelProgressions[1].BestScore = 35;


            IActionResult actionResult = await _userProgressionController.Put(userProgressionDTO);

            OkObjectResult okResult = actionResult as OkObjectResult ?? throw new AssertFailedException(
                "The controller should return OkObjectResult."
            );

            Assert.AreEqual(
                StatusCodes.Status200OK,
                okResult.StatusCode
            );

            ApiResponseDTO apiResponseDTO = okResult.Value as ApiResponseDTO ?? throw new AssertFailedException(
                "The apiResponseDTO should be an apiResponseDTO."
            );

            Assert.IsTrue(apiResponseDTO.HasSucceeded);


            UserProgression? firstUserProgression = await _dataBase.Context.UserProgressions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == 1);

            UserProgression? secondUserProgression = await _dataBase.Context.UserProgressions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == 2);

            Assert.IsNotNull(firstUserProgression);
            Assert.IsNotNull(secondUserProgression);

            Assert.AreEqual(25, firstUserProgression.BestScore);
            Assert.IsTrue(secondUserProgression.IsUnlocked);
            Assert.AreEqual(35, secondUserProgression.BestScore);
        }


        [TestMethod]
        public async Task Put_ProgressionOwnedByAnotherUser_ReturnsBadRequest()
        {
            _dataBase.Context.UserProgressions.AddRange(
                new UserProgression
                {
                    Id = 1,
                    UserId = OTHER_USER_ID,
                    LevelId = 1,
                    IsUnlocked = true,
                    BestScore = 10
                },

                new UserProgression
                {
                    Id = 2,
                    UserId = CURRENT_USER_ID,
                    LevelId = 2,
                    IsUnlocked = false,
                    BestScore = 20
                }
            );

            await _dataBase.Context.SaveChangesAsync();

            _dataBase.Context.ChangeTracker.Clear();


            UserProgressionDTO userProgressionDTO = CreateValidUserProgressionDTO(
                p_firstProgressionId: 1,
                p_secondProgressionId: 2
            );


            IActionResult actionResult = await _userProgressionController.Put(userProgressionDTO);

            BadRequestObjectResult badRequestResult = actionResult as BadRequestObjectResult ?? throw new AssertFailedException(
                "The controller should return BadRequestObjectResult."
            );

            Assert.AreEqual(
                StatusCodes.Status400BadRequest,
                badRequestResult.StatusCode
            );
        }


        [TestMethod]
        public async Task Put_SecondUpdateFails_RollsBackFirstUpdate()
        {
            _dataBase.Context.UserProgressions.AddRange(
                new UserProgression
                {
                    Id = 1,
                    UserId = CURRENT_USER_ID,
                    LevelId = 1,
                    IsUnlocked = true,
                    BestScore = 10
                },

                new UserProgression
                {
                    Id = 2,
                    UserId = OTHER_USER_ID,
                    LevelId = 2,
                    IsUnlocked = false,
                    BestScore = 20
                }
            );

            await _dataBase.Context.SaveChangesAsync();

            _dataBase.Context.ChangeTracker.Clear();


            UserProgressionDTO userProgressionDTO = CreateValidUserProgressionDTO(
                p_firstProgressionId: 1,
                p_secondProgressionId: 2
            );

            userProgressionDTO.LevelProgressions[0].BestScore = 25;


            IActionResult actionResult = await _userProgressionController.Put(userProgressionDTO);

            BadRequestObjectResult badRequestResult = actionResult as BadRequestObjectResult ?? throw new AssertFailedException(
                "The controller should return BadRequestObjectResult."
            );

            Assert.AreEqual(
                StatusCodes.Status400BadRequest,
                badRequestResult.StatusCode
            );


            UserProgression? firstProgression = await _dataBase.Context.UserProgressions
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == 1);

            Assert.IsNotNull(firstProgression);

            // The first modification must also have been rolled back.
            Assert.AreEqual(10, firstProgression.BestScore);
        }


        [TestMethod]
        public async Task Put_InvalidDTO_ReturnsBadRequestWithoutModifyingDatabase()
        {
            _dataBase.Context.UserProgressions.Add(
                new UserProgression
                {
                    Id = 1,
                    UserId = CURRENT_USER_ID,
                    LevelId = 1,
                    IsUnlocked = true,
                    BestScore = 10
                }
            );

            await _dataBase.Context.SaveChangesAsync();

            _dataBase.Context.ChangeTracker.Clear();


            UserProgressionDTO userProgressionDTO = new()
            {
                LevelProgressions =
                [
                    new LevelProgressionDTO
                    {
                        Id = 1,
                        LevelId = 1,
                        IsUnlocked = true,
                        BestScore = -1
                    },

                    new LevelProgressionDTO
                    {
                        Id = 2,
                        LevelId = 2,
                        IsUnlocked = false,
                        BestScore = 10
                    }
                ]
            };


            IActionResult actionResult = await _userProgressionController.Put(userProgressionDTO);

            BadRequestObjectResult badRequestResult = actionResult as BadRequestObjectResult ?? throw new AssertFailedException(
                "The controller should return BadRequestObjectResult."
            );

            Assert.AreEqual(
                StatusCodes.Status400BadRequest,
                badRequestResult.StatusCode
            );


            UserProgression? userProgression = await _dataBase.Context.UserProgressions
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == 1);

            Assert.IsNotNull(userProgression);
            Assert.AreEqual(10, userProgression.BestScore);
        }
    }
}