using System.Collections.Generic;
using System.Numerics;

public static class AuthenticationProperties
{
    public enum AuthenticationMode
    {
        SignUp,
        LogIn,
    }

    public enum AuthenticationErrorReasons
    {
        UsernameIsEmpty,
        UsernameIsTooShort,
        UsernameIsTooLong,

        UsernameContainAtLeastOneSpaceCharacter,

        // --- //

        PasswordIsEmpty,
        PasswordIsTooShort,
        PasswordIsTooLong,

        PasswordContainAtLeastOneSpaceCharacter,

        PasswordDoesNotContainAnyLetters,
        PasswordDoesNotContainAnyNumbers,
        PasswordDoesNotContainAnySpecialCharacters,
    }

    // TODO: Move these properties where any script (FrontEnd and BackEnd) can access it (if possible).
    
    public static readonly int USERNAME_MINIMUM_LENGHT = 1;
    public static readonly int USERNAME_MAXIMUM_LENGHT = 16;

    public static readonly int PASSWORD_MINIMUM_LENGHT = 8;
    public static readonly int PASSWORD_MAXIMUM_LENGHT = 16;
    public static readonly bool DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_LETTER = true;
    public static readonly bool DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_NUMBER = true;
    public static readonly bool DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_SPECIAL_CHARACTER = true;

    public static readonly Vector3 AUTHENTICATION_ERROR_MESSAGE_COLOR = new(1, 0, 0);
    public static readonly Vector3 AUTHENTICATION_SUCCESS_MESSAGE_COLOR = new(0, 0.6f, 0);

    /// <summary>
    /// The values of this dictionary are in french because the project is only in french.
    /// </summary>
    public static readonly Dictionary<AuthenticationErrorReasons, string> AUTHENTICATION_ERROR_MESSAGES = new()
    {
        [AuthenticationErrorReasons.UsernameIsEmpty]                                = $"Le pseudonyme est vide.",
        [AuthenticationErrorReasons.UsernameIsTooShort]                             = $"Le pseudonyme est trop court (<{USERNAME_MINIMUM_LENGHT}).",
        [AuthenticationErrorReasons.UsernameIsTooLong]                              = $"Le pseudonyme est trop long (>{USERNAME_MAXIMUM_LENGHT}).",

        [AuthenticationErrorReasons.UsernameContainAtLeastOneSpaceCharacter]        = $"Le pseudonyme contient au moins un caractère vide.",

        // --- //

        [AuthenticationErrorReasons.PasswordIsEmpty]                                = $"Le mot de passe est vide.",
        [AuthenticationErrorReasons.PasswordIsTooShort]                             = $"Le mot de passe est trop court (<{PASSWORD_MINIMUM_LENGHT}).",
        [AuthenticationErrorReasons.PasswordIsTooLong]                              = $"Le mot de passe est trop long (>{PASSWORD_MAXIMUM_LENGHT}).",

        [AuthenticationErrorReasons.PasswordContainAtLeastOneSpaceCharacter]        = $"Le mot de passe contient au moins un caractère vide.",

        [AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters]               = $"Le mot de passe ne contient pas au moins une lettre.",
        [AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers]               = $"Le mot de passe ne contient pas au moins un chiffre.",
        [AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters]     = $"Le mot de passe ne contient pas au moins un caractère spécial.",
    };

    public static readonly string UNKNOWN_ERROR_MESSAGE = "Erreur inconnue.\nSi le problème persiste, relancez l'application.";

    public static readonly string SUCCESSFUL_AUTHENTICATION_MESSAGE = "Authentification validée localement.\nEnvoie des informations au serveur.";


    public static readonly Dictionary<AuthenticationMode, string> AUTHENTICATION_MODE_IN_FRENCH = new()
    {
        [AuthenticationMode.SignUp]     = $"Inscription",
        [AuthenticationMode.LogIn]      = $"Connexion",
    };
}