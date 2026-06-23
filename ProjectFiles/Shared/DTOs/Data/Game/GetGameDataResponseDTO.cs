using System.Collections.Generic;

namespace Shared.DTOs.Data.Game
{
	public class GetGameDataResponseDTO : ApiResponseDTO
    {
        /// <summary>
        /// Keys = Level's id, Value = Level's data
        /// </summary>
        public Dictionary<int, LevelDTO> Levels { get; set; } = new();

        // Add other properties if necessary

    }
}