using System.Collections.Generic;

namespace Shared.DTOs.Data.User
{
	public class UserProgressionDTO
	{
        public List<LevelProgressionDTO> LevelProgressions { get; set; } = new();

        // Add other properties if necessary

    }
}