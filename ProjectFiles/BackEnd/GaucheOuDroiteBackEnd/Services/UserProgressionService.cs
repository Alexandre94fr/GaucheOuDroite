using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Models;


namespace GaucheOuDroiteBackEnd.Services
{
    public class UserProgressionService(DataBaseContext p_dataBaseContext, UserService p_userService)
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly DataBaseContext _dataBaseContext = p_dataBaseContext;
        readonly UserService _userService = p_userService;

        #region - Is existing -

        public async Task<bool> IsUserProgressionExistingAsync(int p_userId, int p_levelId)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to check if the UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) exist inside the DataBase.");

            if (await GetUserProgressionAsync(p_userId, p_levelId) == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) doesn't exist inside the DataBase. Returning false.");

                return false;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) does exist inside the DataBase. Returning true.");

            return true;
        }

        public async Task<bool> IsUserProgressionExistingAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to check if the UserProgression (Id: {p_id}) exist inside the DataBase.");

            if (await GetUserProgressionAsync(p_id) == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The UserProgression (Id: {p_id}) doesn't exist inside the DataBase. Returning false.");

                return false;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The UserProgression (Id: {p_id}) does exist inside the DataBase. Returning true.");

            return true;
        }

        #endregion

        #region - Create -

        /// <summary>
        /// Creates and saves in the DataBase a new UserProgression. <para></para>
        /// 
        /// Checks before creating any UserProgression, if the given UserId and LevelId are not valid <b>AND</b> if a UserProgression with the same UserId and LevelId already exists. <para></para>
        /// In that case, it returns null and prints out a warning. <para></para>
        /// 
        /// <b>Note:</b> If you want to create all the UserProgressions for a newly created User, use the '<see cref="CreateAllUserProgressionAsync"/>' method instead.
        /// </summary>
        /// <param name="p_userId"></param>
        /// <param name="p_levelId"></param>
        /// <returns> The created UserProgression or null if any check fails. </returns>
        public async Task<UserProgression?> CreateUserProgressionAsync(int p_userId, int p_levelId)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to create a new UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) and saving it inside the DataBase.");

            // Checking if UserId is valid (if the User exist)
            if (!await _userService.IsUserExistingAsync(p_userId))
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] Failed to find the User {p_userId} inside the DataBase. Returning null.");

                return null;
            }

            // Checking if LevelId is valid (if the Level exist)
            Level? level = await _dataBaseContext.Levels.FindAsync(p_levelId);

            if (level == null)
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] Failed to find the Level {p_levelId} inside the DataBase. Returning null.");

                return null;
            }


            // Checking if there is already a UserProgression with the given UserId and LevelId
            if (await IsUserProgressionExistingAsync(p_userId, p_levelId))
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] There is already a UserProgression for User {p_userId} for Level {p_levelId}. Returning null.");

                return null;
            }

            UserProgression userProgression = new()
            {
                // The Id will be generated automatically by the DataBase
                UserId = p_userId,
                LevelId = p_levelId,

                IsUnlocked = false,
                BestScore = 0
            };

            _dataBaseContext.UserProgressions.Add(userProgression);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully created a new UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) and saving it inside the DataBase. Returning the created UserProgression.");

            return userProgression;
        }

        /// <summary>
        /// Creates and saves in the DataBase all the UserProgressions a newly created User needs. <para></para>
        /// 
        /// Checks before creating any UserProgression, if the given UserId is not valid <b>AND</b> if the given User has already at least one UserProgression. <para></para>
        /// In that case, it returns null and prints out a warning.
        /// </summary>
        /// <param name="p_userId"></param>
        /// <returns> The created UserProgressions or null if any check fails. </returns>
        public async Task<List<UserProgression>?> CreateAllUserProgressionAsync(int p_userId)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to create all UserProgressions for the User {p_userId} and saving them inside the DataBase. The number of UserProgression depends on the number of Levels.");

            // Checking if UserId is valid
            if (!await _userService.IsUserExistingAsync(p_userId))
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] Failed to find the User {p_userId} inside the DataBase. Returning null.");

                return null;
            }

            // Checking if the User already has one UserProgression
            // In the game there will be at least one Level, that's why we check for the Level1
            if (await IsUserProgressionExistingAsync(p_userId, 1))
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] The User {p_userId} already have at least one UserProgression. It shouldn't have any when using this method. Returning null.");

                return null;
            }

            // Creating all UserProgressions
            List<UserProgression> userProgressions = new(_dataBaseContext.Levels.Count());

            foreach (Level level in _dataBaseContext.Levels)
            {
                userProgressions.Add(new()
                {
                    // The Id will be generated automatically by the DataBase
                    UserId = p_userId,
                    LevelId = level.Id,

                    IsUnlocked = level.Id == 1, // We want the first Level to be unlocked
                    BestScore = 0
                });
            }

            _dataBaseContext.UserProgressions.AddRange(userProgressions);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully created all ({userProgressions.Count}) the UserProgressions for the User {p_userId} and saving them inside the DataBase. Returning the created UserProgressions.");

            return userProgressions;
        }

        #endregion

        #region - Get -

        public async Task<UserProgression?> GetUserProgressionAsync(int p_userId, int p_levelId)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting the UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) from the DataBase.");

            UserProgression? userProgression = await _dataBaseContext.UserProgressions.FirstOrDefaultAsync(
                userProg => userProg.UserId == p_userId &&
                userProg.LevelId == p_levelId
            );

            if (userProgression == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) inside the DataBase. Returning null.");

                return null;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got the UserProgression (UserId: {p_userId}, LevelId: {p_levelId}). Returning the UserProgression.");

            return userProgression;
        }

        public async Task<UserProgression?> GetUserProgressionAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting the UserProgression (Id: {p_id}) from the DataBase.");

            UserProgression? userProgression = await _dataBaseContext.UserProgressions.FindAsync(p_id);

            if (userProgression == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the UserProgression (Id: {p_id}) inside the DataBase. Returning null.");

                return null;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got the UserProgression (Id: {p_id}). Returning the UserProgression.");

            return userProgression;
        }

        #endregion
        
        #region - Update -

        public async Task UpdateUserAsync(UserProgression p_userProgression)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to update the UserProgression (Id: {p_userProgression.Id}, UserId: {p_userProgression.UserId}, LevelId: {p_userProgression.LevelId}).");

            _dataBaseContext.UserProgressions.Update(p_userProgression);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully updated the UserProgression (Id: {p_userProgression.Id}, UserId: {p_userProgression.UserId}, LevelId: {p_userProgression.LevelId}).");
        }

        #endregion
        
        #region - Delete -

        public async Task<bool> DeleteUserAsync(int p_userId, int p_levelId)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to delete the UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) and updating the DataBase.");

            UserProgression? userProgression = await GetUserProgressionAsync(p_userId, p_levelId);

            if (userProgression == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) to delete from the DataBase. Returning false.");

                return false;
            }

            _dataBaseContext.UserProgressions.Remove(userProgression);
            
            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully deleted the UserProgression (UserId: {p_userId}, LevelId: {p_levelId}) from the the DataBase. Returning true.");

            return true;
        }

        public async Task<bool> DeleteUserAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to delete the UserProgression (Id: {p_id}) and updating the DataBase.");

            UserProgression? userProgression = await GetUserProgressionAsync(p_id);

            if (userProgression == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the UserProgression (Id: {p_id}) to delete from the DataBase. Returning false.");

                return false;
            }

            _dataBaseContext.UserProgressions.Remove(userProgression);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully deleted the UserProgression (Id: {p_id}) from the the DataBase. Returning true.");

            return true;
        }

        #endregion

    }
}