using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Security.DTOValidators;

using Shared.DTOs.Data.User;


namespace GaucheOuDroiteBackEndTests.Security.DTOValidators
{
    [TestClass]
    public sealed class LevelProgressionDTOValidatorTests
    {
        LevelProgressionDTOValidator _levelProgressionDTOValidator = null!;

        List<Level> _levels = null!;


        // -- Setup -- //

        [TestInitialize]
        public void TestInit()
        {
            // The LevelService isn't used when the Levels are directly provided
            // to the ValidateAsync method.
            _levelProgressionDTOValidator = new LevelProgressionDTOValidator(null!);

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
                    IsInfinite = true,
                    ResponseSequence = "LRRL",
                    Star1MinimumScore = 10,
                    Star2MinimumScore = 20,
                    Star3MinimumScore = 30
                }
            ];
        }


        // -- Tests -- //

        [TestMethod]
        public async Task ValidateAsync_NullDTO_ReturnsInvalid()
        {
            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                null,
                _levels
            );

            Assert.IsFalse(result.IsValid);
        }


        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-100)]
        [TestMethod]
        public async Task ValidateAsync_InvalidId_ReturnsInvalid(int p_id)
        {
            LevelProgressionDTO levelProgressionDTO = new()
            {
                Id = p_id,
                LevelId = 1,
                IsUnlocked = true,
                BestScore = 10
            };

            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                levelProgressionDTO,
                _levels
            );

            Assert.IsFalse(result.IsValid);
        }


        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-100)]
        [TestMethod]
        public async Task ValidateAsync_InvalidLevelId_ReturnsInvalid(int p_levelId)
        {
            LevelProgressionDTO levelProgressionDTO = new()
            {
                Id = 1,
                LevelId = p_levelId,
                IsUnlocked = true,
                BestScore = 10
            };

            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                levelProgressionDTO,
                _levels
            );

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_LevelIdGreaterThanLevelCount_ReturnsInvalid()
        {
            LevelProgressionDTO levelProgressionDTO = new()
            {
                Id = 1,
                LevelId = _levels.Count + 1,
                IsUnlocked = true,
                BestScore = 10
            };

            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                levelProgressionDTO,
                _levels
            );

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_NegativeBestScore_ReturnsInvalid()
        {
            LevelProgressionDTO levelProgressionDTO = new()
            {
                Id = 1,
                LevelId = 1,
                IsUnlocked = true,
                BestScore = -1
            };

            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                levelProgressionDTO,
                _levels
            );

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_FiniteLevelScoreGreaterThanStar3MinimumScore_ReturnsInvalid()
        {
            LevelProgressionDTO levelProgressionDTO = new()
            {
                Id = 1,
                LevelId = 1,
                IsUnlocked = true,
                BestScore = _levels[0].Star3MinimumScore + 1
            };

            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                levelProgressionDTO,
                _levels
            );

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_FiniteLevelScoreEqualToStar3MinimumScore_ReturnsValid()
        {
            LevelProgressionDTO levelProgressionDTO = new()
            {
                Id = 1,
                LevelId = 1,
                IsUnlocked = true,
                BestScore = _levels[0].Star3MinimumScore
            };

            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                levelProgressionDTO,
                _levels
            );

            Assert.IsTrue(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_ValidFiniteLevelProgression_ReturnsValid()
        {
            LevelProgressionDTO levelProgressionDTO = new()
            {
                Id = 1,
                LevelId = 1,
                IsUnlocked = true,
                BestScore = 15
            };

            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                levelProgressionDTO,
                _levels
            );

            Assert.IsTrue(result.IsValid);
        }


        [TestMethod]
        public async Task ValidateAsync_ValidInfiniteLevelProgression_ReturnsValid()
        {
            LevelProgressionDTO levelProgressionDTO = new()
            {
                Id = 2,
                LevelId = 2,
                IsUnlocked = false,
                BestScore = 1000
            };

            DTOValidationResult result = await _levelProgressionDTOValidator.ValidateAsync(
                levelProgressionDTO,
                _levels
            );

            Assert.IsTrue(result.IsValid);
        }
    }
}