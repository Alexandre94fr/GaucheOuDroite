namespace Shared.DTOs.Data.User
{
	public class LevelProgressionDTO
	{
        public int UserId { get; set; } = -1;

        public int LevelId { get; set; } = -1;

        public bool IsUnlocked { get; set; } = false;

        public int BestScore { get; set; } = -1;

        // Add other properties if necessary

    }
}