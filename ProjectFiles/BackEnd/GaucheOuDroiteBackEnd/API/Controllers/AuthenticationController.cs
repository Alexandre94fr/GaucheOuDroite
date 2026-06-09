using Microsoft.AspNetCore.Mvc;

using GaucheOuDroiteBackEnd.Services;
using GaucheOuDroiteBackEnd.Tools;

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


        // Note:
        // It seems like the code of the SignUp and LogIn methods are almost perfectly the same.
        // And that we should abstract it.
        // But no, maybe in the future, the SignUp and LogIn methods will have to do differents things,
        // having them separatly will allow us to modify faster the code base.

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
                Console.WriteLine($"DEBUG: [{GetType().Name}] Receiving a SignUp request from FrontEnd, starting validating received values.");

            #region Validating the given data

            if (p_signUpDTO == null)
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package is null. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return BadRequest(signUpResult);
            }


            if (string.IsNullOrEmpty(p_signUpDTO.Username))
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package.Username is null or empty. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return BadRequest(signUpResult);
            }

            if (string.IsNullOrEmpty(p_signUpDTO.Password))
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package.Password is null or empty. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

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
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return BadRequest(signUpResult);
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has succeeded. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

            return Ok(signUpResult);
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogIn(LogInDTO p_logInDTO)
        {
            // Theses variable's values will be changed during the method flow.
            LogInResultDTO logInResult = new()
            {
                HasSucceeded = false,
                AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty
            };

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Receiving a LogIn request from FrontEnd, starting validating received values.");

            #region Validating the given data

            if (p_logInDTO == null)
            {
                logInResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package is null. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return BadRequest(logInResult);
            }


            if (string.IsNullOrEmpty(p_logInDTO.Username))
            {
                logInResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package.Username is null or empty. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return BadRequest(logInResult);
            }

            if (string.IsNullOrEmpty(p_logInDTO.Password))
            {
                logInResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsEmpty;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package.Password is null or empty. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return BadRequest(logInResult);
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The received values are valid, starting AuthenticationService.");

            #endregion


            logInResult = await _authenticationService.LogInAsync(
                p_logInDTO.Username,
                p_logInDTO.Password
            );

            if (!logInResult.HasSucceeded)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The LogIn request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return BadRequest(logInResult);
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The LogIn request has succeeded. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

            return Ok(logInResult);
        }
    }
}