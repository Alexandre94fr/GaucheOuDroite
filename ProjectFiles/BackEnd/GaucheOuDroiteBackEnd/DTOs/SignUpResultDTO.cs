using Shared.Constants;

namespace GaucheOuDroiteBackEnd.DTOs
{
    public class SignUpResultDTO
    {
        public required bool HasSucceeded { get; set; }

        public required AuthenticationProperties.AuthenticationErrorReasons AuthenticationError { get; set; }

        // TODO: Add other properties if necessary
    }
}