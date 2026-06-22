using System.Collections.Generic;

namespace Shared.DTOs.Data.User
{
	public class GetUserProgressionResponseDTO : ApiResponseDTO
	{
        public List<LevelProgressionDTO> LevelProgressions { get; set; } = new();

        // Add other properties if necessary

    }
}