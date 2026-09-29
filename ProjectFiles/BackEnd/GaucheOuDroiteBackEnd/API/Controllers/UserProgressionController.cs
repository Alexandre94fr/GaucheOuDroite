using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Security.DTOValidators;
using GaucheOuDroiteBackEnd.Services;
using GaucheOuDroiteBackEnd.Tools;

using Shared.DTOs;
using Shared.DTOs.Data.User;
using Shared.Tools;


namespace GaucheOuDroiteBackEnd.API.Controllers
{
    [ApiController]
    [Route("api/user-progressions")]
    public class UserProgressionController(UserProgressionDTOValidator p_userProgressionValidator, UserProgressionService p_userProgressionService, DataBaseContext p_dataBaseContext) : ControllerBase
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly UserProgressionDTOValidator _userProgressionValidator = p_userProgressionValidator;
        readonly UserProgressionService _userProgressionService = p_userProgressionService;
        readonly DataBaseContext _dataBaseContext = p_dataBaseContext;


        #region Helping methods

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

            // -- Validating DTO -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to check if the given UserProgressionDTO is valid.");

            DTOValidationResult userProgressionValidationResult = await _userProgressionValidator.ValidateAsync(p_userProgressionDTO);

            if (userProgressionValidationResult.IsValid == false)
            {
                return ReturnBadRequestResult(ref apiResponseDTO, userProgressionValidationResult.ErrorMessage);
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The given UserProgressionDTO is valid.");

            // -- Converting List<LevelProgressionDTO> into List<LevelProgression> -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to convert the list of LevelProgressionDTO into a list of LevelProgression.");

            int userId = UserIdGetter.GetUserId(User);

            // TODO: Rename UserProgression to LevelProgression
            // TODO: Create a real UserProgression (like UserProgressionDTO)
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

            // -- Modifying DataBase's LevelProgression data -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to update the User's LevelProgression data.");

            // In order to be able to revert changes done to the DataBase, we will use a IDbContextTransaction
            IDbContextTransaction transaction = await _dataBaseContext.Database.BeginTransactionAsync();

            // TODO: To do after renaming UserProgression to LevelProgression + creating a real UserProgression.
            //       Verify and change that the variable name or/and strings use the term LevelProgression and UserProgression correctly.
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