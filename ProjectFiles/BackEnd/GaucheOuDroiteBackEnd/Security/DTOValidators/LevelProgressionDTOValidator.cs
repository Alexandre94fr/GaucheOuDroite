using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Services;

using Shared.DTOs.Data.User;


namespace GaucheOuDroiteBackEnd.Security.DTOValidators
{

    public class LevelProgressionDTOValidator(LevelService p_levelService)
    {
        const bool IS_DEBUG_MODE_ON = true;

        // The DataBase stores the BestScore using an 'int', we don't want a User to break the DataBase by passing a value bigger than an 'int'.
        // Technically speaking, the BestScore property is an 'int', so nobody can pass a value bigger than an 'int'.
        // This check is still there in case the maximum allowed value changes.
        const int INFINITE_LEVEL_MAXIMAL_SCORE = int.MaxValue;


        readonly LevelService _levelService = p_levelService;


        /// <summary>
        /// Verifies if the given <see cref="LevelProgressionDTO"/> is valid or not.
        /// 
        /// <para> The <paramref name="p_levels"/> parameter is optional. 
        /// It prevents the need to call "<c>_levelService.GetAllLevelsAsync()</c>" every time a <see cref="LevelProgressionDTO"/> is checked within a <see cref="UserProgressionDTO"/>.</para>
        /// </summary>
        /// 
        /// <returns> Returns a <see cref="DTOValidationResult"/> when the validation is finished. It will tell if the validation succeeded or not, and why. </returns>
        public async Task<DTOValidationResult> ValidateAsync(LevelProgressionDTO? p_levelProgressionDTO, List<Level>? p_levels = null)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to check if the given LevelProgressionDTO is valid.");


            // Getting all Levels' properties.
            // Will be used to verify received LevelProgressionDTO data.
            if (p_levels == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The {nameof(p_levels)} argument is null. No Levels have been given. Getting the Levels using LevelService.");

                p_levels = await _levelService.GetAllLevelsAsync();
            }


            if (p_levelProgressionDTO == null)
            {
                return DTOValidationResult.Invalid($"The given LevelProgressionDTO argument is null.");
            }


            if (p_levelProgressionDTO.Id <= 0)
            {
                return DTOValidationResult.Invalid($"The given LevelProgressionDTO.Id argument is under or equal 0.");
            }


            if (p_levelProgressionDTO.LevelId <= 0)
            {
                return DTOValidationResult.Invalid($"The given LevelProgressionDTO.LevelId argument is under or equal to 0.");
            }

            if (p_levelProgressionDTO.LevelId > p_levels.Count)
            {
                return DTOValidationResult.Invalid($"The given LevelProgressionDTO.LevelId argument is superior to the number of Levels in the game.");
            }


            if (p_levelProgressionDTO.BestScore < 0)
            {
                return DTOValidationResult.Invalid($"The given LevelProgressionDTO.BestScore argument is under 0.");
            }


            Level associatedLevel = p_levels.First(level => level.Id == p_levelProgressionDTO.LevelId);

            // Note: If the Level is not infinite you can't have more score than the 'Star3MinimumScore'.
            if (associatedLevel.IsInfinite == false && p_levelProgressionDTO.BestScore > associatedLevel.Star3MinimumScore)
            {
                return DTOValidationResult.Invalid($"The given LevelProgressionDTO.BestScore argument is superior to the maximal score for a finite Level.");
            }

            // Note: The DataBase stores the BestScore using an 'int', we don't want a User to break the DataBase by passing a value bigger than an 'int'.
            if (associatedLevel.IsInfinite == true && p_levelProgressionDTO.BestScore > INFINITE_LEVEL_MAXIMAL_SCORE)
            {
                return DTOValidationResult.Invalid($"The given LevelProgressionDTO.BestScore argument is superior to the maximal score for an infinite Level.");
            }


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The given LevelProgressionDTO is valid. Returning a valid DTOValidationResult.");

            return DTOValidationResult.Valid();
        }
    }
}