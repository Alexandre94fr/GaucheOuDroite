using System.Collections.Generic;


public static class LevelProperties
{
    // -- Localization -- //

    public static readonly Dictionary<int, string> LEVEL_NAMES_IN_FRENCH = new()
    {
        [1] = "Niveau 1",
        [2] = "Niveau 2",
        [3] = "Niveau 3",
        [4] = "Niveau 4",
        [5] = "Niveau 5",
        [6] = "Niveau 6",
        [7] = "Infini",
    };

    public static readonly Dictionary<int, string> LEVEL_NAMES_IN_ENGLISH = new()
    {
        [1] = "Level 1",
        [2] = "Level 2",
        [3] = "Level 3",
        [4] = "Level 4",
        [5] = "Level 5",
        [6] = "Level 6",
        [7] = "Infinite",
    };
}