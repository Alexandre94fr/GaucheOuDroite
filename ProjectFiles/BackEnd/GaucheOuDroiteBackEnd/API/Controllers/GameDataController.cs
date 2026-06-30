using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Services;

using Shared.DTOs.Data.Game;
using Shared.Enums;
using Shared.Tools;


namespace GaucheOuDroiteBackEnd.API.Controllers
{
    [ApiController]
    [Route("api/game-data")]
    public class GameDataController(LevelService p_levelService, LevelResponseTimeStepService p_levelResponseTimeStepService) : ControllerBase
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly LevelService _levelService = p_levelService;
        readonly LevelResponseTimeStepService _levelResponseTimeStepService = p_levelResponseTimeStepService;


        // Note:
        // We don't want the Client (FrontEnd) to be able to change the Game's data.
        // That's why there is only a Get method.

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            // Theses variable's values will be changed during the method flow.
            GetGameDataResponseDTO getGameDataResponseDTO = new()
            {
                HasSucceeded = false,
                ErrorMessage = "",

                Levels = []
            };


            // Getting all Levels
            List<Level> levels = await _levelService.GetAllLevelsAsync();

            if (levels.Count <= 0)
            {
                getGameDataResponseDTO.ErrorMessage = $"There are no Levels inside the DataBase. The GetGameData request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(getGameDataResponseDTO)}";

                Console.WriteLine($"WARNING: [{GetType().Name}] {getGameDataResponseDTO.ErrorMessage}");

                return NotFound(getGameDataResponseDTO);
            }

            // Getting all LevelResponseTimeSteps
            List<LevelResponseTimeStep> levelResponseTimeSteps = await _levelResponseTimeStepService.GetAllLevelResponseTimeStepAsync();

            if (levelResponseTimeSteps.Count <= 0)
            {
                getGameDataResponseDTO.ErrorMessage = $"There are no LevelResponseTimeStep inside the DataBase. The GetGameData request has failed. Returning:\n{ObjectToStringFormatter.ObjectToString(getGameDataResponseDTO)}";

                Console.WriteLine($"WARNING: [{GetType().Name}] {getGameDataResponseDTO.ErrorMessage}");

                return NotFound(getGameDataResponseDTO);
            }


            // Converting the List<Level> into the Dictionary<int, LevelDTO>
            getGameDataResponseDTO.Levels = new(levels.Count);

            foreach (Level level in levels)
            {
                // Getting all LevelResponseTimeSteps for this specific level
                // AND,
                // converting them to LevelResponseTimeStepDTO
                List<LevelResponseTimeStepDTO> levelResponseTimeStepsDTOByLevel = [];

                foreach (LevelResponseTimeStep levelResponseTimeStep in levelResponseTimeSteps)
                {
                    if (levelResponseTimeStep.LevelId != level.Id)
                        continue;

                    // Convertion: List<LevelResponseTimeSteps> -> List<LevelResponseTimeStepDTO>
                    levelResponseTimeStepsDTOByLevel.Add(new()
                    {
                        MinimumCorrectResponses = levelResponseTimeStep.MinimumCorrectResponses,
                        MaximumResponseTimeInSeconds = levelResponseTimeStep.MaximumResponseTimeInSeconds,
                    });
                }

                // Convertion: List<Level> -> Dictionary<int, LevelDTO>
                getGameDataResponseDTO.Levels.Add(level.Id, new()
                {
                    Name = level.Name,
                    Difficulty = (LevelDifficulty)level.Difficulty,

                    IsInfinite = level.IsInfinite,
                    ResponseSequence = level.ResponseSequence,
                    LevelResponseTimeSteps = levelResponseTimeStepsDTOByLevel,

                    Star1MinimumScore = level.Star1MinimumScore,
                    Star2MinimumScore = level.Star2MinimumScore,
                    Star3MinimumScore = level.Star3MinimumScore,
                });
            }


            getGameDataResponseDTO.HasSucceeded = true;


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The GetGameData request has succeeded. Returning:\n{ObjectToStringFormatter.ObjectToString(getGameDataResponseDTO)}");

            return Ok(getGameDataResponseDTO);
        }
    }
}