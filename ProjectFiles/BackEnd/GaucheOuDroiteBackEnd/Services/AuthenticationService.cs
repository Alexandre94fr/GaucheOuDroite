using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Security;

using Shared.Constants;
using Shared.DTOs;
using Shared.Tools;


namespace GaucheOuDroiteBackEnd.Services
{
    public class AuthenticationService(PasswordHasher p_passwordHasher, UserService p_userService, UserProgressionService p_userProgressionService)
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly PasswordHasher _passwordHasher = p_passwordHasher;
        readonly UserService _userService = p_userService;
        readonly UserProgressionService _userProgressionService = p_userProgressionService;


        public async Task<SignUpResultDTO> SignUpAsync(string p_username, string p_password)
        {
            // Theses variable's values will be changed during the method flow.
            SignUpResultDTO signUpResult = new()
            {
                HasSucceeded = false,
                AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty,

                Token = "",
                UserId = -1,
                Username = p_username,
            };

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to do SignUp for User: '{p_username}'.");

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Verifying that the given username and password are valid.");

            AuthenticationProperties.AuthenticationErrorReasons authenticationError = default;

            if (!Tools.UserDataChecker.IsUsernameValid(p_username, out authenticationError, IS_DEBUG_MODE_ON))
            {
                signUpResult.AuthenticationError = authenticationError;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given username is not valid. The SignUp request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return signUpResult;
            }

            if (!Tools.UserDataChecker.IsPasswordValid(p_password, out authenticationError, IS_DEBUG_MODE_ON))
            {
                signUpResult.AuthenticationError = authenticationError;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given password is not valid. The SignUp request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return signUpResult;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The given username and password are valid.");

            // -- Hashing the password -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Hashing the User's password.");

            string passwordHash = _passwordHasher.HashPassword(p_password);

            // -- Creating new user's identity data and saving it inside the DataBase -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Creating the new User's identity data and saving it inside the DataBase.");

            // Creating the User
            // The CreateUserAsync() method also verifies that the given username has not already an account
            User? user = await _userService.CreateUserAsync(p_username, passwordHash);

            if (user == null)
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameAlreadyExists;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to create the User. The SignUp request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");

                return signUpResult;
            }

            // Updating the returned values
            signUpResult.UserId = user.Id;

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully created the User (Id: {user.Id}, Username: {user.Username}).");

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Creating the new User's progression data and saving it inside the DataBase.");

            // Creating the UserProgressions
            // The CreateAllUserProgressionsAsync() method also verifies that the given User has not already one associated UserProgression
            List<UserProgression>? userProgressions = await _userProgressionService.CreateAllUserProgressionsAsync(user.Id);
            
            if (userProgressions == null)
            {
                signUpResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UserAlreadyHasProgressions;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to create all UserProgressions. The SignUp request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(signUpResult)}");
            
                return signUpResult;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully created all ({userProgressions.Count}) the UserProgressions (UserId: {user.Id}).");

            // -- Returning success -- //

            signUpResult.HasSucceeded = true;

            return signUpResult;
        }

        public async Task<LogInResultDTO> LogInAsync(string p_username, string p_password)
        {
            // Theses variable's values will be changed during the method flow.
            LogInResultDTO logInResult = new()
            {
                HasSucceeded = false,
                AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty,

                Token = "",
                UserId = -1,
                Username = p_username,
            };

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to do LogIn for User: '{p_username}'.");

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Verifying that the given username and password are valid.");

            AuthenticationProperties.AuthenticationErrorReasons authenticationError = default;

            if (!Tools.UserDataChecker.IsUsernameValid(p_username, out authenticationError, IS_DEBUG_MODE_ON))
            {
                logInResult.AuthenticationError = authenticationError;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given username is not valid. The LogIn request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return logInResult;
            }

            if (!Tools.UserDataChecker.IsPasswordValid(p_password, out authenticationError, IS_DEBUG_MODE_ON))
            {
                logInResult.AuthenticationError = authenticationError;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given password is not valid. The LogIn request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return logInResult;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The given username and password are valid.");

            // -- Checking if the User exist in the DataBase -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Checking if the User (Username: {p_username}) exists inside the DataBase.");

            // We use the GetUserAsync() method and not the IsUserExistingAsync() one, because we will need to access the User's data later. 
            User? user = await _userService.GetUserAsync(p_username);

            if (user == null)
            {
                logInResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameDoesNotExist;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the User (Username: {p_username}) inside the DataBase. The LogIn request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return logInResult;
            }

            // Updating the returned values
            logInResult.UserId = user.Id;

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The User (Username: {p_username}) exists inside the DataBase.");

            // -- Checking if the password is the same as the one inside the DataBase -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Checking if the given password is the same as the one saved in the DataBase for the User (Username: {p_username}).");

            if (!_passwordHasher.VerifyHashedPassword(user.PasswordHash, p_password))
            {
                logInResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.IncorrectPassword;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The given password is not the same as the one saved. The LogIn request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return logInResult;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The given password is the same as the one saved.");

            // -- Getting User's data -- //

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting the UserProgression (Username: {p_username}).");

            List<UserProgression> userProgressions = await _userProgressionService.GetAllUserProgressionsAsync(user.Id);

            if (userProgressions.Count == 0)
            {
                logInResult.AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.NoUserProgressionsFound;

                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find any UserProgression with a UserId equal to {user.Id} inside the DataBase. The LogIn request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(logInResult)}");

                return logInResult;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got all ({userProgressions.Count}) the UserProgressions (UserId: {user.Id}).");

            // -- Returning success -- //

            logInResult.HasSucceeded = true;

            return logInResult;
        }
    }
}