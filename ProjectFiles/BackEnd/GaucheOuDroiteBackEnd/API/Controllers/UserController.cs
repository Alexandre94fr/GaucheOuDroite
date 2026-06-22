using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Services;
using GaucheOuDroiteBackEnd.Tools;

using Shared.DTOs; // Needed if you un-comment the Put method
using Shared.DTOs.Data.User;
using Shared.Tools;


namespace GaucheOuDroiteBackEnd.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(UserService p_userService) : ControllerBase
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly UserService _userService = p_userService;


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

                return BadRequest(getUserResponseDTO);
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
    }
}