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

            if (p_userProgressionDTO == null)
            {
                apiResponseDTO.ErrorMessage = $"The given UserProgressionDTO argument is null. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                Console.WriteLine($"WARNING: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                return BadRequest(apiResponseDTO);
            }

            if (p_userProgressionDTO.LevelProgressions == null)
            {
                apiResponseDTO.ErrorMessage = $"The given UserProgressionDTO.LevelProgressions argument is null. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                Console.WriteLine($"WARNING: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                return BadRequest(apiResponseDTO);
            }

            if (p_userProgressionDTO.LevelProgressions.Count <= 0)
            {
                apiResponseDTO.ErrorMessage = $"The given UserProgressionDTO.LevelProgressions argument is an empty list. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                Console.WriteLine($"WARNING: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                return BadRequest(apiResponseDTO);
            }


            // Getting all Levels' properties.
            // Will be used to verify received LevelProgression data.
            List<Level> levels = await _levelService.GetAllLevelsAsync();
            
            for (int i = 0; i < p_userProgressionDTO.LevelProgressions.Count; i++)
            {
                if (p_userProgressionDTO.LevelProgressions[i].Id < 0)
                {
                    apiResponseDTO.ErrorMessage = $"The given UserProgressionDTO.LevelProgressions[{i}].Id argument is under 0. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                    Console.WriteLine($"WARNING: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                    return BadRequest(apiResponseDTO);
                }


                if (p_userProgressionDTO.LevelProgressions[i].LevelId < 0)
                {
                    apiResponseDTO.ErrorMessage = $"The given UserProgressionDTO.LevelProgressions[{i}].LevelId argument is under 0. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                    Console.WriteLine($"WARNING: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                    return BadRequest(apiResponseDTO);
                }


                // TODO: Add a check that checks if the previous Level is unlocked
                // If the, for example, Level3 is unlocked but not the Level2 and Level1 (recursion) then throw error


                if (p_userProgressionDTO.LevelProgressions[i].BestScore < 0)
                {
                    apiResponseDTO.ErrorMessage = $"The given UserProgressionDTO.LevelProgressions[{i}].BestScore argument is under 0. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                    Console.WriteLine($"WARNING: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                    return BadRequest(apiResponseDTO);
                }

                // Note: If the Level is not infinite you can't have more score than the 'Star3MinimumScore'.
                if (levels[i].IsInfinite == false && p_userProgressionDTO.LevelProgressions[i].BestScore > levels[i].Star3MinimumScore)
                {
                    apiResponseDTO.ErrorMessage = $"The given UserProgressionDTO.LevelProgressions[{i}].BestScore argument is superior to the maximal score for a finite Level. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                    Console.WriteLine($"WARNING: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                    return BadRequest(apiResponseDTO);
                }

                // Note: The DataBase stores the BestScore using an 'int', we don't want a User to break the DataBase by passing a value bigger than an 'int'.
                if (levels[i].IsInfinite == true && p_userProgressionDTO.LevelProgressions[i].BestScore > INFINITE_LEVEL_MAXIMAL_SCORE)
                {
                    apiResponseDTO.ErrorMessage = $"The given UserProgressionDTO.LevelProgressions[{i}].BestScore argument is superior to the maximal score for an infinite Level. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                    Console.WriteLine($"WARNING: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                    return BadRequest(apiResponseDTO);
                }
            }

            #endregion


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
                Console.WriteLine($"DEBUG: [{GetType().Name}] The PutUserProgression request has succeeded. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}");

            return Ok(apiResponseDTO);
        }
    }
}