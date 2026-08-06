using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Services;
using GaucheOuDroiteBackEnd.Tools;

using Shared.DTOs;
using Shared.DTOs.Data.User;
using Shared.Tools;


namespace GaucheOuDroiteBackEnd.API.Controllers
{
    [ApiController]
    [Route("api/user-progressions")]
    public class UserProgressionController(LevelService p_levelService, UserProgressionService p_userProgressionService, DataBaseContext p_dataBaseContext) : ControllerBase
    {
        const bool IS_DEBUG_MODE_ON = true;

        // The DataBase stores the BestScore using an 'int', we don't want a User to break the DataBase by passing a value bigger than an 'int'.
        const int INFINITE_LEVEL_MAXIMAL_SCORE = int.MaxValue;

        readonly LevelService _levelService = p_levelService;
        readonly UserProgressionService _userProgressionService = p_userProgressionService;
        readonly DataBaseContext _dataBaseContext = p_dataBaseContext;


        #region Helping methods

        bool IsPreviousLevelUnlocked(in List<LevelProgressionDTO> p_levelProgressions, int p_i)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try checking if the Level (LevelId: {p_levelProgressions[p_i].LevelId}) has the previous Level unlocked.");

            // We can pass this check if the Level we are checking is the first Level, because it was no previous Level.
            if (p_levelProgressions[p_i].LevelId == 1)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Tried to check the first Level of the game. It has no previous Level. Returning true.");

                return true;
            }

            LevelProgressionDTO currentLevelProgression = p_levelProgressions[p_i];
            LevelProgressionDTO previousLevelProgression = p_levelProgressions.First(levelProgression => levelProgression.LevelId == currentLevelProgression.LevelId - 1);

            if (previousLevelProgression == null)
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] Failed to found the previous Level (LevelProgression) that has the LevelId equal to {currentLevelProgression.LevelId - 1}. Returning false.");
                return false;
            }

            if (previousLevelProgression.IsUnlocked == false)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The previous Level (LevelId: {previousLevelProgression.LevelId}) is not unlocked. Returning false.");

                return false;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The previous Level (LevelId: {previousLevelProgression.LevelId}) is unlocked. Returning true.");

            return true;
        }

        BadRequestObjectResult ReturnBadRequestResult(ref ApiResponseDTO p_apiResponseDTO, string p_errorMessage)
        {
            p_apiResponseDTO.ErrorMessage = $"{p_errorMessage} Returning:\n{ObjectToStringFormatter.ObjectToString(p_apiResponseDTO)}";

            Console.WriteLine($"WARNING: [{GetType().Name}] {p_apiResponseDTO.ErrorMessage}");

            return BadRequest(p_apiResponseDTO);
        }

        #endregion


        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            // Theses variable's values will be changed during the method flow.
            GetUserProgressionResponseDTO getUserProgressionResponseDTO = new()
            {
                HasSucceeded = false,
                ErrorMessage = "",

                LevelProgressions = [],
            };

            int userId = UserIdGetter.GetUserId(User);

            List<UserProgression>? userProgressions = await _userProgressionService.GetAllUserProgressionsAsync(userId);

            if (userProgressions == null)
            {
                getUserProgressionResponseDTO.ErrorMessage = $"Failed to find any UserProgression with a UserId equal to {userId} inside the DataBase. The GetUserProgression request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(getUserProgressionResponseDTO)}";

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] {getUserProgressionResponseDTO.ErrorMessage}");

                return NotFound(getUserProgressionResponseDTO);
            }


            // Converting List<UserProgression> into List<LevelProgressionDTO>
            getUserProgressionResponseDTO.LevelProgressions = new(userProgressions.Count);

            foreach (UserProgression userProgression in userProgressions)
            {
                getUserProgressionResponseDTO.LevelProgressions.Add(new()
                {
                    Id = userProgression.Id,
                    LevelId = userProgression.LevelId,

                    IsUnlocked = userProgression.IsUnlocked,
                    BestScore = userProgression.BestScore,
                });
            }

            getUserProgressionResponseDTO.HasSucceeded = true;


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The GetUser request has succeeded. Returning:\n{ObjectToStringFormatter.ObjectToString(getUserProgressionResponseDTO)}");

            return Ok(getUserProgressionResponseDTO);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> Put(UserProgressionDTO p_userProgressionDTO)
        {
            // Theses variable's values will be changed during the method flow.
            ApiResponseDTO apiResponseDTO = new()
            {
                HasSucceeded = false,
                ErrorMessage = "",
            };


            #region -- Security checks --

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to check if the given UserProgressionDTO is valid.");

            // Getting all Levels' properties.
            // Will be used to verify received LevelProgression data.
            List<Level> levels = await _levelService.GetAllLevelsAsync();


            if (p_userProgressionDTO == null)
            {
                return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO argument is null.");
            }


            if (p_userProgressionDTO.LevelProgressions == null)
            {
                return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions argument is null.");
            }


            if (p_userProgressionDTO.LevelProgressions.Count <= 0)
            {
                return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions argument is an empty list.");
            }

            if (p_userProgressionDTO.LevelProgressions.Count != levels.Count)
            {
                return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions argument is a list that doesn't have the same count as the list of Levels in the game.");
            }


            for (int i = 0; i < p_userProgressionDTO.LevelProgressions.Count; i++)
            {
                if (p_userProgressionDTO.LevelProgressions[i].Id < 0)
                {
                    return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions[{i}].Id argument is under 0.");
                }


                if (p_userProgressionDTO.LevelProgressions[i].LevelId < 0)
                {
                    return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions[{i}].LevelId argument is under 0.");
                }

                if (p_userProgressionDTO.LevelProgressions[i].LevelId > levels.Count)
                {
                    return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions[{i}].LevelId argument is superior to the number of Levels in the game.");
                }


                if (p_userProgressionDTO.LevelProgressions[i].IsUnlocked && !IsPreviousLevelUnlocked(p_userProgressionDTO.LevelProgressions, i))
                {
                    return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions[{i}].IsUnlocked (LevelId: {p_userProgressionDTO.LevelProgressions[i].LevelId}) argument is at true, but the previous Level (LevelId: {p_userProgressionDTO.LevelProgressions[i].LevelId - 1}) is not unlocked.");
                }

                
                if (p_userProgressionDTO.LevelProgressions[i].BestScore < 0)
                {
                    return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions[{i}].BestScore argument is under 0.");
                }

                // Note: If the Level is not infinite you can't have more score than the 'Star3MinimumScore'.
                if (levels[i].IsInfinite == false && p_userProgressionDTO.LevelProgressions[i].BestScore > levels[i].Star3MinimumScore)
                {
                    return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions[{i}].BestScore argument is superior to the maximal score for a finite Level.");
                }

                // Note: The DataBase stores the BestScore using an 'int', we don't want a User to break the DataBase by passing a value bigger than an 'int'.
                if (levels[i].IsInfinite == true && p_userProgressionDTO.LevelProgressions[i].BestScore > INFINITE_LEVEL_MAXIMAL_SCORE)
                {
                    return ReturnBadRequestResult(ref apiResponseDTO, $"The given UserProgressionDTO.LevelProgressions[{i}].BestScore argument is superior to the maximal score for an infinite Level.");
                }
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The given UserProgressionDTO is valid.");

            #endregion


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to update the User's progression data.");

            int userId = UserIdGetter.GetUserId(User);

            // Converting List<LevelProgressionDTO> into List<UserProgression>.
            List<UserProgression> levelsProgressions = new(p_userProgressionDTO.LevelProgressions.Count);

            foreach (LevelProgressionDTO levelProgressionDTO in p_userProgressionDTO.LevelProgressions)
            {
                levelsProgressions.Add(new()
                {
                    Id = levelProgressionDTO.Id,
                    UserId = userId,
                    LevelId = levelProgressionDTO.LevelId,

                    IsUnlocked = levelProgressionDTO.IsUnlocked,
                    BestScore = levelProgressionDTO.BestScore,
                });
            }

            // Updating the LevelProgressions
            // In order to be able to revert changes done to the DataBase, we will use a IDbContextTransaction
            IDbContextTransaction transaction = await _dataBaseContext.Database.BeginTransactionAsync();

            try
            {
                foreach (UserProgression levelProgressions in levelsProgressions)
                {
                    if (!await _userProgressionService.UpdateUserProgressionAsync(levelProgressions))
                    {
                        apiResponseDTO.ErrorMessage = $"Failed to put the UserProgression (Id: {levelProgressions.Id}, UserId: {levelProgressions.UserId}, LevelId: {levelProgressions.LevelId}) from the DataBase. The PutUser request has failed. Reverting changes. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                        if (IS_DEBUG_MODE_ON)
                            Console.WriteLine($"DEBUG: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                        // We trigger manually the 'catch' by throwing an exception
                        throw new Exception($"The '{nameof(_userProgressionService.UpdateUserProgressionAsync)}' method, failed to put the UserProgression (Id: {levelProgressions.Id}, UserId: {levelProgressions.UserId}, LevelId: {levelProgressions.LevelId}) from the DataBase.");
                    }
                }
                
                await transaction.CommitAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"ERROR: [{GetType().Name}] An error occurred while putting a UserProgression. Rollbacking the changes and returning.\nError: {exception.Message}");

                await transaction.RollbackAsync();

                return BadRequest(apiResponseDTO);
            }

            apiResponseDTO.HasSucceeded = true;


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The PutUserProgression request has succeeded. The User's progression data has been successfully updated. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}");

            return Ok(apiResponseDTO);
        }
    }
}