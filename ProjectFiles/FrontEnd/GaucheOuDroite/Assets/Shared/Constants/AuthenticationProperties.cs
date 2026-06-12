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

        /// <summary>
        /// The values of this dictionary are in french because the project is only in french.
        /// </summary>
        public static readonly Dictionary<AuthenticationErrorReasons, string> AUTHENTICATION_ERROR_MESSAGES = new()
        {
            [AuthenticationErrorReasons.UsernameIsEmpty] = $"Le pseudonyme est vide.",
            [AuthenticationErrorReasons.UsernameIsTooShort] = $"Le pseudonyme est trop court (<{USERNAME_MINIMUM_LENGHT}).",
            [AuthenticationErrorReasons.UsernameIsTooLong] = $"Le pseudonyme est trop long (>{USERNAME_MAXIMUM_LENGHT}).",

            [AuthenticationErrorReasons.UsernameAlreadyExists] = $"Le pseudonyme est déjà pris par un autre utilisateur.\nSouhaitez-vous plutôt vous connecter ?",

            [AuthenticationErrorReasons.UsernameContainAtLeastOneSpaceCharacter] = $"Le pseudonyme contient au moins un caractère vide.",

            // --- //

            [AuthenticationErrorReasons.PasswordIsEmpty] = $"Le mot de passe est vide.",
            [AuthenticationErrorReasons.PasswordIsTooShort] = $"Le mot de passe est trop court (<{PASSWORD_MINIMUM_LENGHT}).",
            [AuthenticationErrorReasons.PasswordIsTooLong] = $"Le mot de passe est trop long (>{PASSWORD_MAXIMUM_LENGHT}).",

            [AuthenticationErrorReasons.PasswordContainAtLeastOneSpaceCharacter] = $"Le mot de passe contient au moins un caractère vide.",

            [AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters] = $"Le mot de passe ne contient pas au moins une lettre.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers] = $"Le mot de passe ne contient pas au moins un chiffre.",
            [AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters] = $"Le mot de passe ne contient pas au moins un caractère spécial.",

            // --- //

            [AuthenticationErrorReasons.UsernameDoesNotExist] = $"Le pseudonyme n'existe pas.\nSouhaitez-vous plutôt créer un compte ?",
            [AuthenticationErrorReasons.IncorrectPassword] = $"Mot de passe incorrect.\nVeuillez réessayer.",

            [AuthenticationErrorReasons.UserAlreadyHasProgressions] = $"Des données de progression ont été détectées sur un utilisateur qui ne devrait pas en avoir.\nVeuillez réessayer.",
            [AuthenticationErrorReasons.NoUserProgressionsFound] = $"Aucune donnée de progression n'a été trouvée sur un utilisateur qui devrait en avoir.\nVeuillez réessayer.",

            [AuthenticationErrorReasons.InternalServerError] = $"Un problème interne a été détecté côté serveur.\nSi le problème persiste, relancez l'application.",
        };


        public const string SERVER_CONNECTION_ERROR_MESSAGE = "Échec de la connection avec le serveur.\nLe serveur n'est peut-être pas lancé.\nSi le problème persiste, relancez l'application.";
        
        public const string DATA_PROCESSING_ERROR_MESSAGE = "Erreur lors du traitement des données reçues.\nDonnées corrompues ou au mauvais format.\nSi le problème persiste, relancez l'application.";
        
        public const string UNKNOWN_ERROR_MESSAGE = "Erreur inconnue.\nSi le problème persiste, relancez l'application.";

         

        public const string SUCCESSFUL_LOCAL_AUTHENTICATION_MESSAGE = "Authentification validée localement.\nEnvoie des informations au serveur.";
        public const string SUCCESSFUL_SERVER_AUTHENTICATION_MESSAGE = "Authentification validée par le serveur.";


        public static readonly Dictionary<AuthenticationMode, string> AUTHENTICATION_MODE_IN_FRENCH = new()
        {
            [AuthenticationMode.SignUp] = $"Inscription",
            [AuthenticationMode.LogIn] = $"Connexion",
        };
    }
}