using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Services;

using Shared.DTOs.Data.User;


namespace GaucheOuDroiteBackEnd.Security.DTOValidators
{

    public class UserProgressionDTOValidator(LevelService p_levelService, LevelProgressionDTOValidator p_levelProgressionDTOValidator)
    {
        const bool IS_DEBUG_MODE_ON = true;


        readonly LevelService _levelService = p_levelService;
        readonly LevelProgressionDTOValidator _levelProgressionDTOValidator = p_levelProgressionDTOValidator;


        bool IsPreviousLevelUnlocked(in List<LevelProgressionDTO> p_levelProgressions, int p_i)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try checking if the Level (LevelId: {p_levelProgressions[p_i].LevelId}) has the previous Level unlocked.");

            // We can pass this check if the Level we are checking is the first Level, because it was no previous Level.
            if (p_levelProgressions[p_i].LevelId == 1)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Tried to check the first Level of the game. It has no previous Level. Returning true.");

                return true;
            }

            LevelProgressionDTO currentLevelProgression = p_levelProgressions[p_i];
            LevelProgressionDTO? previousLevelProgression = p_levelProgressions.FirstOrDefault(levelProgression => levelProgression.LevelId == currentLevelProgression.LevelId - 1);

            if (previousLevelProgression == null)
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] Failed to found the previous Level (LevelProgression) that has the LevelId equal to {currentLevelProgression.LevelId - 1}. Returning false.");
                return false;
            }

            if (previousLevelProgression.IsUnlocked == false)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The previous Level (LevelId: {previousLevelProgression.LevelId}) is not unlocked. Returning false.");

                return false;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The previous Level (LevelId: {previousLevelProgression.LevelId}) is unlocked. Returning true.");

            return true;
        }


        /// <summary>
        /// Verifies if the given <see cref="UserProgressionDTO"/> is valid or not.
        /// </summary>
        /// 
        /// <returns> Returns a <see cref="DTOValidationResult"/> when the validation is finished. It will tell if the validation succeeded or not, and why. </returns>
        public async Task<DTOValidationResult> ValidateAsync(UserProgressionDTO? p_userProgressionDTO)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to check if the given UserProgressionDTO is valid.");


            // Getting all Levels' properties.
            // Will be used to verify if the number of LevelProgression in UserProgressionDTO is the same as the number of Levels in the game.
            List<Level> levels = await _levelService.GetAllLevelsAsync();


            if (p_userProgressionDTO == null)
            {
                return DTOValidationResult.Invalid($"The given UserProgressionDTO argument is null.");
            }


            if (p_userProgressionDTO.LevelProgressions == null)
            {
                return DTOValidationResult.Invalid($"The given UserProgressionDTO.LevelProgressions argument is null.");
            }


            if (p_userProgressionDTO.LevelProgressions.Count <= 0)
            {
                return DTOValidationResult.Invalid($"The given UserProgressionDTO.LevelProgressions argument is an empty list.");
            }

            if (p_userProgressionDTO.LevelProgressions.Count != levels.Count)
            {
                return DTOValidationResult.Invalid($"The given UserProgressionDTO.LevelProgressions argument is a list that doesn't have the same count as the list of Levels in the game.");
            }


            // First int = LevelId, second int = i
            Dictionary<int, int> levelIdSeen = new(p_userProgressionDTO.LevelProgressions.Count);

            for (int i = 0; i < p_userProgressionDTO.LevelProgressions.Count; i++)
            {
                DTOValidationResult levelProgressionValidationResult = await _levelProgressionDTOValidator.ValidateAsync(p_userProgressionDTO.LevelProgressions[i], levels);

                if (levelProgressionValidationResult.IsValid == false)
                {
                    return DTOValidationResult.Invalid(levelProgressionValidationResult.ErrorMessage);
                }


                if (levelIdSeen.TryGetValue(p_userProgressionDTO.LevelProgressions[i].LevelId, out int seenLevelProgressionIndex))
                {
                    return DTOValidationResult.Invalid($"The given UserProgressionDTO.LevelProgressions[{i}].LevelId (LevelId: {p_userProgressionDTO.LevelProgressions[i].LevelId}) argument has the same LevelId as the UserProgressionDTO.LevelProgressions[{seenLevelProgressionIndex}].");
                }

                levelIdSeen.Add(p_userProgressionDTO.LevelProgressions[i].LevelId, i);


                if (p_userProgressionDTO.LevelProgressions[i].IsUnlocked && !IsPreviousLevelUnlocked(p_userProgressionDTO.LevelProgressions, i))
                {
                    return DTOValidationResult.Invalid($"The given UserProgressionDTO.LevelProgressions[{i}].IsUnlocked (LevelId: {p_userProgressionDTO.LevelProgressions[i].LevelId}) argument is at true, but the previous Level (LevelId: {p_userProgressionDTO.LevelProgressions[i].LevelId - 1}) is not unlocked.");
                }
            }


            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The given UserProgressionDTO is valid. Returning a valid DTOValidationResult.");

            return DTOValidationResult.Valid();
        }
    }
}