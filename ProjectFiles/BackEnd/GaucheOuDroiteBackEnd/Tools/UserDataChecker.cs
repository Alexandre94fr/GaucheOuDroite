using GaucheOuDroiteBackEnd.Models;

using Shared.Constants;

namespace GaucheOuDroiteBackEnd.Tools
{
    public static class UserDataChecker
    {
        static readonly string CLASS_NAME = typeof(UserDataChecker).Name;

        public static bool IsUsernameValid(string p_username, out AuthenticationProperties.AuthenticationErrorReasons p_authenticationErrorReason, bool p_isDebugModeOn = false)
        {
            return Shared.Tools.UserDataChecker.IsUsernameValid(p_username, out p_authenticationErrorReason, p_isDebugModeOn);
        }

        public static bool IsPasswordValid(string p_password, out AuthenticationProperties.AuthenticationErrorReasons p_authenticationErrorReason, bool p_isDebugModeOn = false)
        {
            return Shared.Tools.UserDataChecker.IsPasswordValid(p_password, out p_authenticationErrorReason, p_isDebugModeOn);
        }

        public static bool IsUserValid(User p_user, out AuthenticationProperties.AuthenticationErrorReasons p_authenticationErrorReason, bool p_isDebugModeOn = false)
        {
            // To silence the linter.
            // Normaly a program will not use the 'p_authenticationErrorReason' if the password is valid.
            p_authenticationErrorReason = default;

            if (p_user == null)
            {
                p_authenticationErrorReason = AuthenticationProperties.AuthenticationErrorReasons.UsernameIsEmpty;

                if (p_isDebugModeOn)
                    Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given User is null. Returning false + {p_authenticationErrorReason}.");

                return false;
            }

            if (!IsUsernameValid(p_user.Username, out p_authenticationErrorReason, p_isDebugModeOn))
                return false;

            if (!IsPasswordValid(p_user.PasswordHash, out p_authenticationErrorReason, p_isDebugModeOn))
                return false;

            if (p_isDebugModeOn)
                Console.WriteLine($"DEBUG: [{CLASS_NAME}] The given User is valid. Returning true + {p_authenticationErrorReason}.");

            return true;
        }
    }
}