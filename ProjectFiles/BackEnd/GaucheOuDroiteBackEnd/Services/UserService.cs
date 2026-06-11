using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Models;


namespace GaucheOuDroiteBackEnd.Services
{
    public class UserService(DataBaseContext p_dataBaseContext)
    {
        const bool IS_DEBUG_MODE_ON = true;

        readonly DataBaseContext _dataBaseContext = p_dataBaseContext;


        #region - Is existing -

        public async Task<bool> IsUserExistingAsync(string p_username)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to check if the User (Id: {p_username}) exists inside the DataBase.");

            if (await GetUserAsync(p_username) == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The User (Id: {p_username}) doesn't exist inside the DataBase. Returning false.");

                return false;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The User (Id: {p_username}) does exist inside the DataBase. Returning true.");

            return true;
        }

        public async Task<bool> IsUserExistingAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to check if the User (Id: {p_id}) exists inside the DataBase.");

            if (await GetUserAsync(p_id) == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] The User (Id: {p_id}) doesn't exist inside the DataBase. Returning false.");

                return false;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] The User (Id: {p_id}) does exist inside the DataBase. Returning true.");

            return true;
        }

        #endregion


        #region - Create -

        /// <summary>
        /// Creates and saves in the DataBase a new User. <para></para>
        /// 
        /// Checks before creating any User, if a User with the same username already exists. <para></para>
        /// In that case, it returns null and prints out a warning.
        /// </summary>
        /// <param name="p_username"></param>
        /// <param name="p_passwordHash"></param>
        /// <returns> The created User or null if another User as the same username. </returns>
        public async Task<User?> CreateUserAsync(string p_username, string p_passwordHash)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to create a new User (Username: {p_username}) and saving it inside the DataBase.");

            if (await IsUserExistingAsync(p_username))
            {
                Console.WriteLine($"WARNING: [{GetType().Name}] Another User already have the '{p_username}' username. Returning null.");

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
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully created a new User (Username: {p_username}) and saving it inside the DataBase. Returning the created User.");

            return user;
        }

        #endregion

        #region - Get -

        public async Task<User?> GetUserAsync(string p_username)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting the User (Username: {p_username}) from the DataBase.");

            User? user = await _dataBaseContext.Users.FirstOrDefaultAsync(user => user.Username == p_username);

            if (user == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the User (Username: {p_username}) inside the DataBase. Returning null.");

                return null;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got the User (Username: {p_username}). Returning the User.");

            return user;
        }

        public async Task<User?> GetUserAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to try getting the User (Id: {p_id}) from the DataBase.");

            User? user = await _dataBaseContext.Users.FindAsync(p_id);

            if (user == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the User (Id: {p_id}) inside the DataBase. Returning null.");

                return null;
            }

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully got the User (Id: {p_id}). Returning the User.");

            return user;
        }

        #endregion

        #region - Update -

        public async Task UpdateUserAsync(User p_user)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to update the User (Id: {p_user.Id}, Username: {p_user.Username}).");

            _dataBaseContext.Users.Update(p_user);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully updated the User (Id: {p_user.Id}, Username: {p_user.Username}).");
        }

        #endregion

        #region - Delete -

        public async Task<bool> DeleteUserAsync(string p_username)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to delete the User (Username: {p_username}) and updating the DataBase.");

            User? user = await GetUserAsync(p_username);

            if (user == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to find the User (Username: {p_username}) to delete from the DataBase. Returning false.");

                return false;
            }

            _dataBaseContext.Users.Remove(user);
            
            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully deleted the User (Username: {p_username}) from the the DataBase. Returning true.");

            return true;
        }

        public async Task<bool> DeleteUserAsync(int p_id)
        {
            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Starting to delete the User (Id: {p_id}) and updating the DataBase.");

            User? user = await GetUserAsync(p_id);

            if (user == null)
            {
                if (IS_DEBUG_MODE_ON)
                    Console.WriteLine($"DEBUG: [{GetType().Name}] Failed to get the User (Id: {p_id}) to delete from the DataBase. Returning false.");

                return false;
            }

            _dataBaseContext.Users.Remove(user);

            await _dataBaseContext.SaveChangesAsync();

            if (IS_DEBUG_MODE_ON)
                Console.WriteLine($"DEBUG: [{GetType().Name}] Successfully deleted the User (Id: {p_id}) from the the DataBase. Returning true.");

            return true;
        }

        #endregion

    }
}