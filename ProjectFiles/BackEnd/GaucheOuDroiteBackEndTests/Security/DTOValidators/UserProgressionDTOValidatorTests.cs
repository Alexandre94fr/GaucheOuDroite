using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Security.DTOValidators;
using GaucheOuDroiteBackEnd.Services;

using GaucheOuDroiteBackEndTests.Tools;

using Shared.DTOs.Data.User;


namespace GaucheOuDroiteBackEndTests.Security.DTOValidators
{
    [TestClass]
    public sealed class UserProgressionDTOValidatorTests
    {
        TestDataBase _dataBase = null!;

        UserProgressionDTOValidator _userProgressionDOTValidator = null!;

        List<Level> _levels = null!;


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

            await _dataBase.Context.SaveChangesAsync();

            LevelService levelService = new(_dataBase.Context);

            LevelProgressionDTOValidator levelProgressionDTOValidator = new(levelService);

            _userProgressionDOTValidator = new UserProgressionDTOValidator(
                levelService,
                levelProgressionDTOValidator
            );
        }


        [TestCleanup]
        public void TestCleanup()
        {
            _dataBase.Dispose();
        }


        static LevelProgressionDTO CreateProgressionDTO(int p_id, int p_levelId, bool p_isUnlocked = false, int p_bestScore = 0)
        {
            return new()
            {
                Id = p_id,
                LevelId = p_levelId,
                IsUnlocked = p_isUnlocked,
                BestScore = p_bestScore
            };
        }


        static UserProgressionDTO CreateValidUserProgressionDTO()
        {
            return new()
            {
                LevelProgressions =
                [
                    CreateProgressionDTO(
                        p_id: 1,
                        p_levelId: 1,
                        p_isUnlocked: true,
                        p_bestScore: 10
                    ),

                    CreateProgressionDTO(
                        p_id: 2,
                        p_levelId: 2,
                        p_isUnlocked: false,
                        p_bestScore: 20
                    )
                ]
            };
        }


        // -- Tests -- //

        [TestMethod]
        public async Task ValidateAsync_NullDTO_ReturnsInvalid()
        {
            DTOValidationResult result = await _userProgressionDOTValidator.ValidateAsync(null);

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_NullLevelProgressions_ReturnsInvalid()
        {
            UserProgressionDTO userProgressionDTO = new()
            {
                LevelProgressions = null!
            };

            DTOValidationResult result = await _userProgressionDOTValidator.ValidateAsync(userProgressionDTO);

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_EmptyLevelProgressions_ReturnsInvalid()
        {
            UserProgressionDTO userProgressionDTO = new()
            {
                LevelProgressions = []
            };

            DTOValidationResult result = await _userProgressionDOTValidator.ValidateAsync(userProgressionDTO);

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_WrongNumberOfProgressions_ReturnsInvalid()
        {
            UserProgressionDTO userProgressionDTO = new()
            {
                LevelProgressions =
                [
                    CreateProgressionDTO(
                        p_id: 1,
                        p_levelId: 1,
                        p_isUnlocked: true
                    )
                ]
            };

            DTOValidationResult result = await _userProgressionDOTValidator.ValidateAsync(userProgressionDTO);

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_DuplicateLevelId_ReturnsInvalid()
        {
            UserProgressionDTO userProgressionDTO = new()
            {
                LevelProgressions =
                [
                    CreateProgressionDTO(
                        p_id: 1,
                        p_levelId: 1,
                        p_isUnlocked: true
                    ),

                    CreateProgressionDTO(
                        p_id: 2,
                        p_levelId: 1,
                        p_isUnlocked: false
                    )
                ]
            };

            DTOValidationResult result = await _userProgressionDOTValidator.ValidateAsync(userProgressionDTO);

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_FirstLevelUnlocked_ReturnsValid()
        {
            UserProgressionDTO userProgressionDTO = CreateValidUserProgressionDTO();

            DTOValidationResult result = await _userProgressionDOTValidator.ValidateAsync(userProgressionDTO);

            Assert.IsTrue(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_UnlockedLevelWithLockedPreviousLevel_ReturnsInvalid()
        {
            UserProgressionDTO userProgressionDTO = new()
            {
                LevelProgressions =
                [
                    CreateProgressionDTO(
                        p_id: 1,
                        p_levelId: 1,
                        p_isUnlocked: false
                    ),

                    CreateProgressionDTO(
                        p_id: 2,
                        p_levelId: 2,
                        p_isUnlocked: true
                    )
                ]
            };

            DTOValidationResult result = await _userProgressionDOTValidator.ValidateAsync(userProgressionDTO);

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_UnlockedLevelWithUnlockedPreviousLevel_ReturnsValid()
        {
            UserProgressionDTO userProgressionDTO = CreateValidUserProgressionDTO();

            userProgressionDTO.LevelProgressions[1].IsUnlocked = true;

            DTOValidationResult result = await _userProgressionDOTValidator.ValidateAsync(userProgressionDTO);

            Assert.IsTrue(result.IsValid);
        }
    }
}