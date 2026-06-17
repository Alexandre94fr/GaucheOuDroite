using Shared.Constants;

namespace Shared.DTOs
{
	public class AuthenticationResultDTO
	{
        public bool HasSucceeded { get; set; }

        public AuthenticationProperties.AuthenticationErrorReasons AuthenticationError { get; set; }

        public string Token { get; set; } = "";

        public int UserId { get; set; }

        public string Username { get; set; } = "";

        // TODO: Add other properties if necessary

    }
}