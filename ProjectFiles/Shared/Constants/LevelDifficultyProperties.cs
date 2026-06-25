using System.Collections.Generic;
using System.Numerics;

using Shared.Enums;


namespace Shared.Constants
{
    public static class LevelDifficultyProperties
    {
        public static readonly Dictionary<LevelDifficulty, Vector3> DIFFICULTY_COLORS = new()
        {
            [LevelDifficulty.Easy]              = new(0.0f , 0.5f , 0.05f),
            [LevelDifficulty.Medium]            = new(0.45f, 0.5f , 0.0f ),
            [LevelDifficulty.Hard]              = new(0.5f , 0.0f , 0.0f ),

            [LevelDifficulty.Progressive]       = new(0.45f, 0.0f , 0.5f ),
        };

        // -- Localization -- //

        public static readonly Dictionary<LevelDifficulty, string> DIFFICULTY_NAMES_IN_FRENCH = new()
        {
            [LevelDifficulty.Easy]              = "Facile",
            [LevelDifficulty.Medium]            = "Normal",
            [LevelDifficulty.Hard]              = "Difficile",

            [LevelDifficulty.Progressive]       = "Progressif",
        };
    }
}