using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Models;


namespace GaucheOuDroiteBackEnd.Services
{
    public class LevelResponseTimeStepService(DataBaseContext p_dataBaseContext)
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly DataBaseContext _dataBaseContext = p_dataBaseContext;


        // Note:
        // For this project, it's not necessary to add the possibility for the server to create new LevelResponseTimeSteps.
        // This is why there are only methods to get them for the DataBase.
        // They are created directly by the DataBase at start.

        #region - Get -

        public async Task<LevelResponseTimeStep?> GetLevelResponseTimeStepAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting the LevelResponseTimeStep (Id: {p_id}) from the DataBase.");

            LevelResponseTimeStep? levelResponseTimeStep = await _dataBaseContext.LevelResponseTimeSteps.FirstOrDefaultAsync(levelResponseTimeStep => levelResponseTimeStep.Id == p_id);

            if (levelResponseTimeStep == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the LevelResponseTimeStep (Id: {p_id}) inside the DataBase. Returning null.");

                return null;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got the LevelResponseTimeStep (Id: {p_id}). Returning the User.");

            return levelResponseTimeStep;
        }

        public async Task<List<LevelResponseTimeStep>> GetAllLevelResponseTimeStepByLevelAsync(int p_levelId)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting all the LevelResponseTimeSteps for Level {p_levelId} from the DataBase.");

            List<LevelResponseTimeStep> levelResponseTimeSteps = await _dataBaseContext.LevelResponseTimeSteps
                .Where(levelResponseTimeStep => levelResponseTimeStep.LevelId == p_levelId)
                .ToListAsync();

            if (levelResponseTimeSteps.Count <= 0)
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] There are no LevelResponseTimeSteps for Level {p_levelId} inside the DataBase. Returning the empty list.");

                return levelResponseTimeSteps;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got all ({levelResponseTimeSteps.Count}) LevelResponseTimeSteps for Level {p_levelId}. Returning the LevelResponseTimeStep.");

            return levelResponseTimeSteps;
        }

        public async Task<List<LevelResponseTimeStep>> GetAllLevelResponseTimeStepAsync()
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting all the LevelResponseTimeSteps from the DataBase.");

            List<LevelResponseTimeStep> levelResponseTimeSteps = await _dataBaseContext.LevelResponseTimeSteps.ToListAsync();

            if (levelResponseTimeSteps.Count <= 0)
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] There are no LevelResponseTimeSteps inside the DataBase. Returning the empty list.");

                return levelResponseTimeSteps;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got all ({levelResponseTimeSteps.Count}) LevelResponseTimeSteps. Returning the LevelResponseTimeSteps.");

            return levelResponseTimeSteps;
        }

        #endregion

    }
}