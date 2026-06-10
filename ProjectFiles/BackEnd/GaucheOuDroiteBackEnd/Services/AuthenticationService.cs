using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Security;

using Shared.Constants;
using Shared.DTOs;
using Shared.Tools;


namespace GaucheOuDroiteBackEnd.Services
{
    public class AuthenticationService(PasswordHasher p_passwordHasher, UserService p_userService)
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly PasswordHasher _passwordHasher = p_passwordHasher;
        readonly UserService _userService = p_userService;


        public async Task<SignUpResultDTO> SignUpAsync(string p_username, string p_password)
        {
            // Theses variable's values will be changed during the method flow.
            SignUpResultDTO signUpResult = new()
            {
                HasSucceeded = false,
                AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty
            };

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting doing SignUp for user: '{p_username}'.");

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Verifying that the given username and password are valid.");

            AuthenticationProperties.AuthenticationErrorReasons authenticationError = default;

            if (!Tools.UserDataChecker.IsUsernameValid(p_username, out authenticationError, IS_DEBUG_MODE_ON))
            {
                signUpResult.AuthenticationError = authenticationError;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return signUpResult;
            }

            if (!Tools.UserDataChecker.IsPasswordValid(p_password, out authenticationError, IS_DEBUG_MODE_ON))
            {
                signUpResult.AuthenticationError = authenticationError;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return signUpResult;
            }

            // -- Hashing the password -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Hashing the user's password.");

            string passwordHash = _passwordHasher.HashPassword(p_password);

            // -- Creating new user's identity data and saving it inside the DataBase -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Creating the new user's identity data and saving it inside the DataBase.");

            // The CreateUserAsync() method also verify that the given username has not already an account
            User? user = await _userService.CreateUserAsync(p_username, passwordHash);

            if (user == null)
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameAlreadyExists;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The SignUp request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return signUpResult;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Creating the new user's progression data and saving it inside the DataBase.");

            // TODO: Creating the PlayerProgression (call a Service)

            // -- Returning success -- //

            signUpResult.HasSucceeded = true;

            return signUpResult;
        }

        public async Task<LogInResultDTO> LogInAsync(string p_username, string p_password)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] TODO. LogIn");

            // TODO:
            // Vérifier le mot de passe

            return new LogInResultDTO
            {
                HasSucceeded = true,
                AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty
            };
        }
    }
}