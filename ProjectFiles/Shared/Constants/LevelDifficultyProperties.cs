using System.Collections.Generic;
using System.Numerics;

using Shared.Enums;


namespace Shared.Constants
{
    public static class LevelDifficultyProperties
    {
        public static readonly Dictionary<LevelDifficulty, Vector3> DIFFICULTY_COLORS = new()
        {
            [LevelDifficulty.VeryEasy]      = new(0.00f, 0.65f, 0.00f),
            [LevelDifficulty.Easy]          = new(0.00f, 0.50f, 0.05f),
            [LevelDifficulty.Medium]        = new(0.45f, 0.50f, 0.00f),
            [LevelDifficulty.Hard]          = new(0.50f, 0.00f, 0.00f),

            [LevelDifficulty.Progressive]   = new(0.45f, 0.00f, 0.50f),
        };

        // -- Localization -- //

        public static readonly Dictionary<LevelDifficulty, string> DIFFICULTY_NAMES_IN_FRENCH = new()
        {
            [LevelDifficulty.VeryEasy]      = "Très facile",
            [LevelDifficulty.Easy]          = "Facile",
            [LevelDifficulty.Medium]        = "Normal",
            [LevelDifficulty.Hard]          = "Difficile",

            [LevelDifficulty.Progressive]   = "Progressif",
        };

        public static readonly Dictionary<LevelDifficulty, string> DIFFICULTY_NAMES_IN_ENGLISH = new()
        {
            [LevelDifficulty.VeryEasy]      = "Very easy",
            [LevelDifficulty.Easy]          = "Easy",
            [LevelDifficulty.Medium]        = "Medium",
            [LevelDifficulty.Hard]          = "Hard",

            [LevelDifficulty.Progressive]   = "Progressive",
        };
    }
}