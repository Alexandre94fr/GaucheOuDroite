using System.Collections.Generic;
using System.Numerics;

namespace Shared.Constants
{
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

            UsernameAlreadyExists,
            UsernameDoesNotExist, // When there is no User with this username inside the DataBase when login in

            UsernameContainAtLeastOneSpaceCharacter,

            // --- //

            PasswordIsEmpty,
            PasswordIsTooShort,
            PasswordIsTooLong,

            PasswordContainAtLeastOneSpaceCharacter,

            PasswordDoesNotContainAnyLetters,
            PasswordDoesNotContainAnyNumbers,
            PasswordDoesNotContainAnySpecialCharacters,

            IncorrectPassword, // When the given password is not the same as the one registered on the DataBase when login in

            // --- // BackEnd (Server-side) only errors

            UserAlreadyHasProgressions, // When there is a UserProgression associated with a newly created User (full BackEnd error)
            NoUserProgressionsFound, // When there is no UserProgression associated with the User (full BackEnd error)

            InternalServerError, // When any kind of server error happens and is not registered in this enum
        }

        public const int USERNAME_MINIMUM_LENGHT = 1;
        public const int USERNAME_MAXIMUM_LENGHT = 16;

        public const int PASSWORD_MINIMUM_LENGHT = 8;
        public const int PASSWORD_MAXIMUM_LENGHT = 16;
        public const bool DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_LETTER = true;
        public const bool DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_NUMBER = true;
        public const bool DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_SPECIAL_CHARACTER = true;

        public static readonly Vector3 AUTHENTICATION_ERROR_MESSAGE_COLOR = new(1, 0, 0);
        public static readonly Vector3 AUTHENTICATION_SUCCESS_MESSAGE_COLOR = new(0, 0.6f, 0);


        public static readonly System.TimeSpan MAXIMAL_INITIAL_DATA_LOADING_TIME = new(0, 0, 10);

        // ------------ //
        // -- FRENCH -- //
        // ------------ //

        public static readonly Dictionary<AuthenticationErrorReasons, string> AUTHENTICATION_ERROR_MESSAGES_IN_FRENCH = new()
        {
            [AuthenticationErrorReasons.UsernameIsEmpty]                            = $"Le pseudonyme est vide.",
            [AuthenticationErrorReasons.UsernameIsTooShort]                         = $"Le pseudonyme est trop court (<{USERNAME_MINIMUM_LENGHT}).",
            [AuthenticationErrorReasons.UsernameIsTooLong]                          = $"Le pseudonyme est trop long (>{USERNAME_MAXIMUM_LENGHT}).",

            [AuthenticationErrorReasons.UsernameAlreadyExists]                      = $"Le pseudonyme est déjà pris par un autre utilisateur.\nSouhaitez-vous plutôt vous connecter ?",

            [AuthenticationErrorReasons.UsernameContainAtLeastOneSpaceCharacter]    = $"Le pseudonyme contient au moins un caractère vide.",

            // --- //

            [AuthenticationErrorReasons.PasswordIsEmpty]                            = $"Le mot de passe est vide.",
            [AuthenticationErrorReasons.PasswordIsTooShort]                         = $"Le mot de passe est trop court (<{PASSWORD_MINIMUM_LENGHT}).",
            [AuthenticationErrorReasons.PasswordIsTooLong]                          = $"Le mot de passe est trop long (>{PASSWORD_MAXIMUM_LENGHT}).",

            [AuthenticationErrorReasons.PasswordContainAtLeastOneSpaceCharacter]    = $"Le mot de passe contient au moins un caractère vide.",

            [AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters]           = $"Le mot de passe ne contient pas au moins une lettre.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers]           = $"Le mot de passe ne contient pas au moins un chiffre.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters] = $"Le mot de passe ne contient pas au moins un caractère spécial.",

            // --- //

            [AuthenticationErrorReasons.UsernameDoesNotExist]                       = $"Le pseudonyme n'existe pas.\nSouhaitez-vous plutôt créer un compte ?",
            [AuthenticationErrorReasons.IncorrectPassword]                          = $"Mot de passe incorrect.\nVeuillez réessayer.",

            [AuthenticationErrorReasons.UserAlreadyHasProgressions]                 = $"Des données de progression ont été détectées sur un utilisateur qui ne devrait pas en avoir.\nVeuillez réessayer.",
            [AuthenticationErrorReasons.NoUserProgressionsFound]                    = $"Aucune donnée de progression n'a été trouvée sur un utilisateur qui devrait en avoir.\nVeuillez réessayer.",

            [AuthenticationErrorReasons.InternalServerError]                        = $"Un problème interne a été détecté côté serveur.\nSi le problème persiste, relancez l'application.",
        };

        public static readonly Dictionary<AuthenticationErrorReasons, string> USERNAME_REQUIREMENTS_HELPING_MESSAGES_IN_FRENCH = new()
        {
            [AuthenticationErrorReasons.UsernameIsTooShort]                         = $"Doit faire au moins {USERNAME_MINIMUM_LENGHT} caractère(s).",
            [AuthenticationErrorReasons.UsernameIsTooLong]                          = $"Ne doit pas dépasser {USERNAME_MAXIMUM_LENGHT} caractère(s).",

            [AuthenticationErrorReasons.UsernameContainAtLeastOneSpaceCharacter]    = $"Ne doit pas contenir d'espace.",
        };

        public static readonly Dictionary<AuthenticationErrorReasons, string> PASSWORD_REQUIREMENTS_HELPING_MESSAGES_IN_FRENCH = new()
        {
            [AuthenticationErrorReasons.PasswordIsTooShort]                         = $"Doit faire au moins {PASSWORD_MINIMUM_LENGHT} caractère(s).",
            [AuthenticationErrorReasons.PasswordIsTooLong]                          = $"Ne doit pas dépasser {PASSWORD_MAXIMUM_LENGHT} caractère(s).",

            [AuthenticationErrorReasons.PasswordContainAtLeastOneSpaceCharacter]    = $"Ne doit pas contenir d'espace.",

            [AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters]           = $"Doit contenir au moins une lettre.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers]           = $"Doit contenir au moins un chiffre.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters] = $"Doit contenir au moins un caractère spécial.",
        };


        public const string SERVER_CONNECTION_ERROR_MESSAGE_IN_FRENCH               = "Échec de la connection avec le serveur.\nLe serveur n'est peut-être pas lancé.\nSi le problème persiste, relancez l'application.";
        
        public const string DATA_PROCESSING_ERROR_MESSAGE_IN_FRENCH                 = "Erreur lors du traitement des données reçues.\nDonnées corrompues ou au mauvais format.\nSi le problème persiste, relancez l'application.";
        
        public const string UNKNOWN_ERROR_MESSAGE_IN_FRENCH                         = "Erreur inconnue.\nSi le problème persiste, relancez l'application.";


        public static readonly string TOO_LONG_LOADING_DATA_ERROR_MESSAGE_IN_FRENCH = $"Temps maximal de chargement des données dépassé ({MAXIMAL_INITIAL_DATA_LOADING_TIME}s).\nSi le problème persiste, relancez l'application.";



        public const string SUCCESSFUL_LOCAL_AUTHENTICATION_MESSAGE_IN_FRENCH       = "Authentification validée localement.\nEnvoie des informations au serveur.";
        public const string SUCCESSFUL_SERVER_AUTHENTICATION_MESSAGE_IN_FRENCH      = "Authentification validée par le serveur.";


        public static readonly Dictionary<AuthenticationMode, string> AUTHENTICATION_MODE_IN_FRENCH = new()
        {
            [AuthenticationMode.SignUp] = $"Inscription",
            [AuthenticationMode.LogIn] = $"Connexion",
        };

        // ------------- //
        // -- ENGLISH -- //
        // ------------- //

        public static readonly Dictionary<AuthenticationErrorReasons, string> AUTHENTICATION_ERROR_MESSAGES_IN_ENGLISH = new()
        {
            [AuthenticationErrorReasons.UsernameIsEmpty] = $"The username is empty.",
            [AuthenticationErrorReasons.UsernameIsTooShort] = $"The username is too short (<{USERNAME_MINIMUM_LENGHT}).",
            [AuthenticationErrorReasons.UsernameIsTooLong] = $"The username is too long (>{USERNAME_MAXIMUM_LENGHT}).",

            [AuthenticationErrorReasons.UsernameAlreadyExists] = $"The username is already taken by another user.\nAre you trying to log in?",

            [AuthenticationErrorReasons.UsernameContainAtLeastOneSpaceCharacter] = $"The username contain at least one space character.",

            // --- //

            [AuthenticationErrorReasons.PasswordIsEmpty] = $"The password is empty.",
            [AuthenticationErrorReasons.PasswordIsTooShort] = $"The password is too short (<{PASSWORD_MAXIMUM_LENGHT}).",
            [AuthenticationErrorReasons.PasswordIsTooLong] = $"The password is too long (>{PASSWORD_MAXIMUM_LENGHT}).",

            [AuthenticationErrorReasons.PasswordContainAtLeastOneSpaceCharacter] = $"The password contain at least one space character.",

            [AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters] = $"The password doesn't contain any letters.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers] = $"The password doesn't contain any numbers.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters] = $"The password doesn't contain any special characters.",

            // --- //

            [AuthenticationErrorReasons.UsernameDoesNotExist] = $"The username doesn't exist.\nAre you trying to create an account?",
            [AuthenticationErrorReasons.IncorrectPassword] = $"Incorrect password\nPlease try again.",

            [AuthenticationErrorReasons.UserAlreadyHasProgressions] = $"Some progression data have been detected on a user that shouldn't have any.\nPlease try again.",
            [AuthenticationErrorReasons.NoUserProgressionsFound] = $"No progression data have been founded on a user that should have some.\nPlease try again.",

            [AuthenticationErrorReasons.InternalServerError] = $"An internal problem has been detected on the server.\nIf the problem persists, restart the application.",
        };

        public static readonly Dictionary<AuthenticationErrorReasons, string> USERNAME_REQUIREMENTS_HELPING_MESSAGES_IN_ENGLISH = new()
        {
            [AuthenticationErrorReasons.UsernameIsTooShort] = $"Must be at least {USERNAME_MINIMUM_LENGHT} character(s) long.",
            [AuthenticationErrorReasons.UsernameIsTooLong] = $"Must not be more than {USERNAME_MAXIMUM_LENGHT} character(s) long.",

            [AuthenticationErrorReasons.UsernameContainAtLeastOneSpaceCharacter] = $"Must not contain space characters.",
        };

        public static readonly Dictionary<AuthenticationErrorReasons, string> PASSWORD_REQUIREMENTS_HELPING_MESSAGES_IN_ENGLISH = new()
        {
            [AuthenticationErrorReasons.PasswordIsTooShort] = $"Must be at least {USERNAME_MINIMUM_LENGHT} character(s) long.",
            [AuthenticationErrorReasons.PasswordIsTooLong] = $"Must not be more than {USERNAME_MAXIMUM_LENGHT} character(s) long.",

            [AuthenticationErrorReasons.PasswordContainAtLeastOneSpaceCharacter] = $"Must not contain space characters.",

            [AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters] = $"Must contain a least one letter.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers] = $"Must contain a least one number.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters] = $"Must contain a least one special character.",
        };


        public const string SERVER_CONNECTION_ERROR_MESSAGE_IN_ENGLISH = "Failed to connect to the server.\nThe server may not be running.\nIf the problem persists, restart the application.";

        public const string DATA_PROCESSING_ERROR_MESSAGE_IN_ENGLISH = "Error processing the received data.\nThe data is corrupted or in the wrong format.\nIf the problem persists, restart the application.";

        public const string UNKNOWN_ERROR_MESSAGE_IN_ENGLISH = "Unknown error.\nIf the problem persists, restart the application.";


        public static readonly string TOO_LONG_LOADING_DATA_ERROR_MESSAGE_IN_ENGLISH = $"The maximum data loading time has been exceeded ({MAXIMAL_INITIAL_DATA_LOADING_TIME}s).\nIf the problem persists, restart the application.";



        public const string SUCCESSFUL_LOCAL_AUTHENTICATION_MESSAGE_IN_ENGLISH = "Authentication validated locally.\nSending information to the server.";
        public const string SUCCESSFUL_SERVER_AUTHENTICATION_MESSAGE_IN_ENGLISH = "Authentication validated by the server.";


        public static readonly Dictionary<AuthenticationMode, string> AUTHENTICATION_MODE_IN_ENGLISH = new()
        {
            [AuthenticationMode.SignUp] = $"Sign up",
            [AuthenticationMode.LogIn] = $"Log in",
        };
    }
}