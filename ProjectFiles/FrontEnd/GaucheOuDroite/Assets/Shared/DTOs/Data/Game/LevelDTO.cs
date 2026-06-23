using System.Collections.Generic;

namespace Shared.DTOs.Data.Game
{
	public class LevelDTO
    {
        public string Name { get; set; } = "";

        public int Difficulty { get; set; } = -1;

        public bool IsInfinite { get; set; } = false;

        public string ResponseSequence { get; set; } = "";

        public List<LevelResponseTimeStepDTO> LevelResponseTimeSteps { get; set; } = new();

        public int Star1MinimumScore { get; set; } = -1;

        public int Star2MinimumScore { get; set; } = -1;

        public int Star3MinimumScore { get; set; } = -1;

        // Add other properties if necessary

    }
}