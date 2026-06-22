namespace Shared.DTOs.Data.User
{
	public class GetUserResponseDTO : ApiResponseDTO
    {
        public int Id { get; set; } = -1;

        public string Username { get; set; } = "";

        // Add other properties if necessary

    }
}