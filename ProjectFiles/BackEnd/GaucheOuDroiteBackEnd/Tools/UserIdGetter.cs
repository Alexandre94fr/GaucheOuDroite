using System.Security.Claims;

namespace GaucheOuDroiteBackEnd.Tools
{
    public static class UserIdGetter
    {
        /// <summary>
        /// Returns the Id of a User.
        /// 
        /// <para> It uses the AuthenticationToken to do so. </para>
        /// </summary>
        /// <param name="p_user"> You can only get the User if you are in a Class that inherits from ControllerBase. </param>
        /// <returns></returns>
        public static int GetUserId(ClaimsPrincipal p_user)
        {
            Claim? claim = p_user.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new Exception("Missing 'NameIdentifier' claim. The JWT may be configured incorrectly.");

            return int.Parse(claim.Value);
        }
    }
}