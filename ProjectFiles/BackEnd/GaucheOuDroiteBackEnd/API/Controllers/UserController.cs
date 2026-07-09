using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/users")]
    public class UserController(DataBaseContext p_dataBaseContext, UserService p_userService, UserProgressionService p_userProgressionService) : ControllerBase
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly DataBaseContext _dataBaseContext = p_dataBaseContext;
        readonly UserService _userService = p_userService;
        readonly UserProgressionService _userProgressionService = p_userProgressionService;


        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            // Theses variable's values will be changed during the method flow.
            GetUserResponseDTO getUserResponseDTO = new()
            {
                HasSucceeded = false,
                ErrorMessage = "",

                Id = -1,
                Username = "",
            };

            int userId = UserIdGetter.GetUserId(User);

            User? user = await _userService.GetUserAsync(userId);

            if (user == null)
            {
                getUserResponseDTO.ErrorMessage = $"Failed to get the User (Id: {userId}) from the DataBase. The GetUser request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(getUserResponseDTO)}";

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] {getUserResponseDTO.ErrorMessage}");

                return NotFound(getUserResponseDTO);
            }


            getUserResponseDTO.Id = user.Id;
            getUserResponseDTO.Username = user.Username;
            getUserResponseDTO.HasSucceeded = true;


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The GetUser request has succeeded. Returning:\n{ObjectToStringFormatter.ObjectToString(getUserResponseDTO)}");

            return Ok(getUserResponseDTO);
        }

        // For now, there is no need to have PutUser method, but if one day we need it,
        // you can un-comment this code and add a PutUserRequestDTO.
        /*
        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> Put(PutUserRequestDTO p_userDTO)
        {
            // Theses variable's values will be changed during the method flow.
            ApiResponseDTO apiResponseDTO = new()
            {
                HasSucceeded = false,
                ErrorMessage = "",
            };

            if (!await _userService.UpdateUserAsync(p_userDTO.Id))
            {
                apiResponseDTO.ErrorMessage = $"Failed to put the User (Id: {p_userDTO.Id}) from the DataBase. The PutUser request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                return BadRequest(apiResponseDTO);
            }


            apiResponseDTO.HasSucceeded = true;


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The PutUser request has succeeded. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}");

            return Ok(apiResponseDTO);
        }
        */

        [Authorize]
        [HttpDelete()]
        public async Task<IActionResult> Delete()
        {
            // Theses variable's values will be changed during the method flow.
            ApiResponseDTO apiResponseDTO = new()
            {
                HasSucceeded = false,
                ErrorMessage = "",
            };

            int userId = UserIdGetter.GetUserId(User);

            // Deleting the UserProgressions and the User data

            // In order to be able to revert changes done to the DataBase, we will use a IDbContextTransaction
            IDbContextTransaction transaction = await _dataBaseContext.Database.BeginTransactionAsync();

            try
            {
                if (!await _userProgressionService.DeleteAllUserProgressionsForUser(userId))
                {
                    apiResponseDTO.ErrorMessage = $"Failed to get or to delete the all the UserProgressions of the User (Id: {userId}) from the DataBase. The DeleteUser request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                    if (IS_DEBUG_MODE_ON)
                        Console.WriteLine($"DEBUG: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                    // We trigger manually the 'catch' by throwing an exception
                    throw new Exception($"The '{nameof(_userProgressionService.DeleteAllUserProgressionsForUser)}' method, failed to delete all UserProgressions of the User (Id: {userId}) from the DataBase.");
                }

                if (!await _userService.DeleteUserAsync(userId))
                {
                    apiResponseDTO.ErrorMessage = $"Failed to get the User (Id: {userId}) from the DataBase. The DeleteUser request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}";

                    if (IS_DEBUG_MODE_ON)
                        Console.WriteLine($"DEBUG: [{GetType().Name}] {apiResponseDTO.ErrorMessage}");

                    // We trigger manually the 'catch' by throwing an exception
                    throw new Exception($"The '{nameof(_userService.DeleteUserAsync)}' method, failed to delete the User (Id: {userId}) from the DataBase.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"ERROR: [{GetType().Name}] An error occurred while deleting all User's data of the User (Id: {userId}). Rollbacking the changes and returning.\nError: {exception.Message}");

                await transaction.RollbackAsync();

                return NotFound(apiResponseDTO);
            }


            await transaction.CommitAsync();

            apiResponseDTO.HasSucceeded = true;


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The DeleteUser request has succeeded. Returning:\n{ObjectToStringFormatter.ObjectToString(apiResponseDTO)}");

            return Ok(apiResponseDTO);
        }
    }
}