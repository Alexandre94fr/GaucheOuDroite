using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using GaucheOuDroiteBackEnd.Services;

using Shared.Constants;
using Shared.DTOs;


namespace GaucheOuDroiteBackEnd.API.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController(AuthenticationService p_authenticationService) : ControllerBase
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly AuthenticationService _authenticationService = p_authenticationService;


        static string GetResultToString(object p_result, Formatting p_formating = Formatting.Indented)
        {
            return $"{JsonConvert.SerializeObject(p_result, p_formating)}";
        }


        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUpDTO p_signUpDTO)
        {
            // Theses variable's values will be changed during the method flow.
            SignUpResultDTO signUpResult = new()
            {
                HasSucceeded = false,
                AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty
            };

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Receiving request from FrontEnd, starting validating received values.");

            #region Validating the given data

            if (p_signUpDTO == null)
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package is null. Returning:\n{GetResultToString(signUpResult)}");

                return BadRequest(signUpResult);
            }


            if (string.IsNullOrEmpty(p_signUpDTO.Username))
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package.Username is null or empty. Returning:\n{GetResultToString(signUpResult)}");

                return BadRequest(signUpResult);
            }

            if (string.IsNullOrEmpty(p_signUpDTO.Password))
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package.Password is null or empty. Returning:\n{GetResultToString(signUpResult)}");

                return BadRequest(signUpResult);
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The received values are valid, starting AuthenticationService.");

            #endregion


            signUpResult = await _authenticationService.SignUpAsync(
                p_signUpDTO.Username,
                p_signUpDTO.Password
            );

            if (!signUpResult.HasSucceeded)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has failed. Returning:\n{GetResultToString(signUpResult)}");

                return BadRequest(signUpResult);
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has succeeded. Returning:\n{GetResultToString(signUpResult)}");

            return Ok(signUpResult);
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogIn(LogInDTO p_logInDTO)
        {
            if (p_logInDTO == null)
            {
                if (IS_DEBUG_MODE_ON)
                    // TODO: Print a log

                return BadRequest(AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty);
            }


            if (string.IsNullOrEmpty(p_logInDTO.Username))
            {
                if (IS_DEBUG_MODE_ON)
                    // TODO: Print a log

                return BadRequest(AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty);
            }

            if (string.IsNullOrEmpty(p_logInDTO.Password))
            {
                if (IS_DEBUG_MODE_ON)
                    // TODO: Print a log

                return BadRequest(AuthenticationProperties.AuthenticationErrorReasons.PasswordIsEmpty);
            }


            LogInResultDTO logInResult = await _authenticationService.LogInAsync(
                p_logInDTO.Username,
                p_logInDTO.Password
            );

            if (!logInResult.HasSucceeded)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] TODO.");
                    // TODO: Print a log

                return BadRequest(logInResult.AuthenticationError);
            }

            return Ok(new
            {
                HasSucceeded = true // TODO: Is that necessary?

                // TODO: Put the real data here
            });
        }
    }
}
