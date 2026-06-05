using Shared.Constants;

namespace Shared.DTOs
{
    public class SignUpResultDTO
    {
        public bool HasSucceeded { get; set; }

        public AuthenticationProperties.AuthenticationErrorReasons AuthenticationError { get; set; }

        // TODO: Add other properties if necessary
    }
}