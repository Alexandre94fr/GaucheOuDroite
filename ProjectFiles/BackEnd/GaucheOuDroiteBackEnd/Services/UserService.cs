using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Models;


namespace GaucheOuDroiteBackEnd.Services
{
    public class UserService(DataBaseContext p_dataBaseContext)
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly DataBaseContext _dataBaseContext = p_dataBaseContext;
        // TODO: Fix typos

        #region - Is existing -

        public async Task<bool> IsUserExistingAsync(string p_username)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting checking if the user '{p_username}' exist inside the DataBase.");

            if (await GetUserAsync(p_username) == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The user '{p_username}' doesn't exist inside the DataBase. Returning false.");

                return false;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The user '{p_username}' does exist inside the DataBase. Returning true.");

            return true;
        }

        public async Task<bool> IsUserExistingAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting checking if the user '{p_id}' exist inside the DataBase.");

            if (await GetUserAsync(p_id) == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The user '{p_id}' doesn't exist inside the DataBase. Returning false.");

                return false;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The user '{p_id}' does exist inside the DataBase. Returning true.");

            return true;
        }

        #endregion


        #region - Create -

        /// <summary>
        /// Creates and saves in the DataBase a new user. <para></para>
        /// 
        /// Checks before creating any User, if a User with the same username already exists. <para></para>
        /// In that case, it returns null and prints a debug log if '<see cref="IS_DEBUG_MODE_ON"/>' is at true.
        /// </summary>
        /// <param name="p_username"></param>
        /// <param name="p_passwordHash"></param>
        /// <returns> The created User or null if another User as the same username. </returns>
        public async Task<User?> CreateUserAsync(string p_username, string p_passwordHash)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting creating a new user '{p_username}' and saving it inside the DataBase.");

            if (await IsUserExistingAsync(p_username))
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Another user already have the '{p_username}' username. Returning null.");

                return null;
            }

            User user = new()
            {
                // The Id is generated automatically by the DataBase
                Username = p_username,
                PasswordHash = p_passwordHash
            };

            _dataBaseContext.Users.Add(user);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully created a new user '{p_username}' and saving it inside the DataBase. Returning the created User.");

            return user;
        }

        #endregion

        #region - Get -

        public async Task<User?> GetUserAsync(string p_username)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting trying getting the user '{p_username}' from the DataBase.");

            User? user = await _dataBaseContext.Users.FirstOrDefaultAsync(user => user.Username == p_username);

            if (user == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to get the user '{p_username}'. He is not inside the DataBase. Returning null.");

                return null;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got the user '{p_username}'. Returning the User.");

            return user;
        }

        public async Task<User?> GetUserAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting trying getting the user '{p_id}' from the DataBase.");

            User? user = await _dataBaseContext.Users.FindAsync(p_id);

            if (user == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to get the user '{p_id}'. He is not inside the DataBase. Returning null.");

                return null;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got the user '{p_id}'. Returning the User.");

            return user;
        }

        #endregion

        #region - Update -

        public async Task UpdateUserAsync(User p_user)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting updating the user '{p_user.Username}'.");

            _dataBaseContext.Users.Update(p_user);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully updated the user '{p_user.Username}'.");
        }

        #endregion

        #region - Delete -

        public async Task<bool> DeleteUserAsync(string p_username)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting deleting the user '{p_username}' and updating the DataBase.");

            User? user = await GetUserAsync(p_username);

            if (user == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to get the User to delete from the DataBase. Returning false.");

                return false;
            }

            _dataBaseContext.Users.Remove(user);
            
            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully deleted the user '{p_username}' from the the DataBase. Returning true.");

            return true;
        }

        public async Task<bool> DeleteUserAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting deleting the user '{p_id}' and updating the DataBase.");

            User? user = await GetUserAsync(p_id);

            if (user == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to get the User to delete from the DataBase. Returning false.");

                return false;
            }

            _dataBaseContext.Users.Remove(user);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully deleted the user '{p_id}' from the the DataBase. Returning true.");

            return true;
        }

        #endregion

    }
}