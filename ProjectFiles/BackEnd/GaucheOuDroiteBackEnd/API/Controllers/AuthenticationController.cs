using Microsoft.AspNetCore.Mvc;

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


        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUpDTO p_signUpDTO)
        {
            // TODO: Le p_signUpDTO arrive Empty (des strings vide : "")
            // Va check le code de Unity pour qu'il serialise bien le package a envoyer

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Receiving request from FrontEnd, starting validating received values.");

            if (p_signUpDTO == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package is null. Returning '{AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty}'.");

                return BadRequest(AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty);
            }


            if (string.IsNullOrEmpty(p_signUpDTO.Username))
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package.Username is null or empty. Returning '{AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty}'.");

                // TODO: Faire que les BadRequest est renvoie toujours un SignUpResultDTO, ça permettra à Unity et au BackEnd de se synchroniser
                // Ainsi quand Unity recevra ResponseBody: 0, il sera que c'est un l'Enum n°0 d'AuthenticationProperties.AuthenticationErrorReasons.

                return BadRequest(AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty);
            }

            if (string.IsNullOrEmpty(p_signUpDTO.Password))
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given package.Password is null or empty. Returning '{AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty}'.");

                return BadRequest(AuthenticationProperties.AuthenticationErrorReasons.PasswordIsEmpty);
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The received values are valid, starting AuthenticationService.");


            SignUpResultDTO signUpResult = await _authenticationService.SignUpAsync(
                p_signUpDTO.Username,
                p_signUpDTO.Password
            );

            if (!signUpResult.HasSucceeded)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has failed. Returning '{signUpResult.AuthenticationError}'.");

                return BadRequest(signUpResult.AuthenticationError);
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has succeeded. Returning '{"--- TODO ---"}'.");
                // TODO: Print a log

            return Ok(new
            {
                HasSucceeded = true // TODO: Is that necessary?

                // TODO: Put the real data here
            });
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
