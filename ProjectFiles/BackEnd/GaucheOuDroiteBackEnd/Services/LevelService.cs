using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Models;


namespace GaucheOuDroiteBackEnd.Services
{
    public class LevelService(DataBaseContext p_dataBaseContext)
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly DataBaseContext _dataBaseContext = p_dataBaseContext;


        // Note:
        // For this project, it's not necessary to add the possibility for the server to create new Levels.
        // This is why there are only methods to get them for the DataBase.
        // They are created directly by the DataBase at start.

        #region - Get -

        public async Task<Level?> GetLevelAsync(int p_levelId)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting the Level (Id: {p_levelId}) from the DataBase.");

            Level? level = await _dataBaseContext.Levels.FirstOrDefaultAsync(level => level.Id == p_levelId);

            if (level == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the Level (Id: {p_levelId}) inside the DataBase. Returning null.");

                return null;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got the Level (Id: {p_levelId}). Returning the User.");

            return level;
        }

        public async Task<List<Level>> GetAllLevelsAsync()
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting all the Levels from the DataBase.");

            List<Level> levels = await _dataBaseContext.Levels.ToListAsync();

            if (levels.Count <= 0)
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] There are no Levels inside the DataBase. Returning the empty list.");

                return levels;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got all ({levels.Count}) Levels. Returning the Levels.");

            return levels;
        }

        #endregion

    }
}