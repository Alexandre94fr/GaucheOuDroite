using GaucheOuDroiteBackEnd.DTOs;
using Shared.Constants;

namespace GaucheOuDroiteBackEnd.Services
{
    public class AuthenticationService
    {
        const bool IS_DEBUG_MODE_ON = true;

        public async Task<SignUpResultDTO> SignUpAsync(string p_username, string p_password)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] TODO. SignUp");

            // TODO:
            // Vérifier si l'utilisateur existe
            // Hasher le mot de passe
            // Sauvegarder dans SQLite

            return new SignUpResultDTO {
                HasSucceeded = true,
                AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty
            };
        }

        public async Task<LogInResultDTO> LogInAsync(string p_username, string p_password)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] TODO. LogIn");

            // TODO:
            // Vérifier l'utilisateur
            // Vérifier le mot de passe

            return new LogInResultDTO
            {
                HasSucceeded = true,
                AuthenticationError = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty
            };
        }
    }
}