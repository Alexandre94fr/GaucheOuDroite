using System;
using System.Linq;

using Shared.Constants;

namespace Shared.Tools
{
    public static class UserDataChecker
    {
        static readonly string CLASS_NAME = typeof(UserDataChecker).Name;

        public static bool IsUsernameValid(string p_username, out AuthenticationProperties.AuthenticationErrorReasons p_authenticationErrorReason, bool p_isDebugModeOn = false)
        {
            // To silence the linter.
            // Normaly a program will not use the 'p_authenticationErrorReason' if the password is valid.
            p_authenticationErrorReason = default;

            if (string.IsNullOrEmpty(p_username))
            {
                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;

                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given username is null. Returning false + {p_authenticationErrorReason}.");

                return false;
            }

            if (p_username.Length < AuthenticationProperties.USERNAME_MINIMUM_LENGHT)
            {
                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsTooShort;

                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given username is too short (<{AuthenticationProperties.USERNAME_MINIMUM_LENGHT}). Returning false + {p_authenticationErrorReason}.");

                return false;
            }

            if (p_username.Length > AuthenticationProperties.USERNAME_MAXIMUM_LENGHT)
            {
                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsTooLong;

                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given username is too long (>{AuthenticationProperties.USERNAME_MAXIMUM_LENGHT}). Returning false + {p_authenticationErrorReason}.");

                return false;
            }

            if (p_isDebugModeOn)
                Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given username is valid. Returning true + {p_authenticationErrorReason}.");

            return true;
        }

        public static bool IsPasswordValid(string p_password, out AuthenticationProperties.AuthenticationErrorReasons p_authenticationErrorReason, bool p_isDebugModeOn = false)
        {
            // To silence the linter.
            // Normaly a program will not use the 'p_authenticationErrorReason' if the password is valid.
            p_authenticationErrorReason = default;

            if (string.IsNullOrEmpty(p_password))
            {
                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsEmpty;

                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given password is null. Returning false + {p_authenticationErrorReason}.");

                return false;
            }

            if (p_password.Length < AuthenticationProperties.PASSWORD_MINIMUM_LENGHT)
            {
                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsTooShort;

                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given password is too short (<{AuthenticationProperties.PASSWORD_MINIMUM_LENGHT}). Returning false + {p_authenticationErrorReason}.");

                return false;
            }

            if (p_password.Length > AuthenticationProperties.PASSWORD_MAXIMUM_LENGHT)
            {
                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordIsTooLong;

                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given password is too long (>{AuthenticationProperties.PASSWORD_MAXIMUM_LENGHT}). Returning false + {p_authenticationErrorReason}.");

                return false;
            }


            if (p_password.Contains(' '))
            {
                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordContainAtLeastOneSpaceCharacter;

                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given password contains at least one space character. Returning false + {p_authenticationErrorReason}.");

                return false;
            }


            if (AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_LETTER && !p_password.Any(char.IsLetter))
            {
                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given password doesn't contain at least one letter. Returning false + {p_authenticationErrorReason}.");

                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters;
                return false;
            }

            if (AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_NUMBER && !p_password.Any(char.IsDigit))
            {
                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given password doesn't contain at least one number. Returning false + {p_authenticationErrorReason}.");

                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers;
                return false;
            }

            if (AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_SPECIAL_CHARACTER && !p_password.Any(c => !char.IsLetterOrDigit(c)))
            {
                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given password doesn't contain at least one special character. Returning false + {p_authenticationErrorReason}.");

                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters;
                return false;
            }

            if (p_isDebugModeOn)
                Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given password is valid. Returning true + {p_authenticationErrorReason}.");

            return true;
        }
    }
}