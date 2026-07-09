namespace Shared.DTOs.Data.User
{
	public class LevelProgressionDTO
	{
        public int Id { get; set; } = -1;

        // There is no need to store UserId, because the Server can get it directly.
        // Plus, the Client doesn't need to store this information.

        public int LevelId { get; set; } = -1;

        public bool IsUnlocked { get; set; } = false;

        public int BestScore { get; set; } = -1;

        // Add other properties if necessary

    }
}