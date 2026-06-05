using Shared.Constants;

namespace GaucheOuDroiteBackEnd.DTOs
{
	public class LogInResultDTO
	{
        public required bool HasSucceeded { get; set; }

        public required AuthenticationProperties.AuthenticationErrorReasons AuthenticationError { get; set; }

		// TODO: Add other properties if necessary
	}
}