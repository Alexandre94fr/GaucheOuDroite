using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InstantiatorPackage;

using Shared.DTOs;
using Shared.DTOs.Data.User;


namespace FrontEnd.Data.User
{
    public class UserProgression
    {
        public Dictionary<int, LevelProgression> LevelProgressions { get; internal set; } = new();


        public static explicit operator UserProgression(UserProgressionDTO p_userProgressionDTO)
        {
            UserProgression userProgression = new()
            {
                LevelProgressions = new(p_userProgressionDTO.LevelProgressions.Count)
            };

            // Converting LevelProgressionDTO into LevelProgression
            foreach (LevelProgressionDTO levelProgression in p_userProgressionDTO.LevelProgressions)
            {
                userProgression.LevelProgressions.Add(
                    levelProgression.LevelId,
                    (LevelProgression)levelProgression
                );
            }

            return userProgression;
        }

        public UserProgressionDTO ToUserProgressionDTO(int p_userId)
        {
            UserProgressionDTO userProgressionDTO = new()
            {
                LevelProgressions = new(this.LevelProgressions.Count)
            };
            
            // Converting LevelProgression into LevelProgressionDTO
            foreach ((int levelId, LevelProgression levelProgression) in this.LevelProgressions)
            {
                userProgressionDTO.LevelProgressions.Add(
                    levelProgression.ToLevelProgressionDTO(p_userId, levelId)
                );
            }

            return userProgressionDTO;
        }
    }

    public class LevelProgression
    {
        public bool IsUnlocked { get; internal set; } = false;

        public int BestScore { get; internal set; } = -1;


        public static explicit operator LevelProgression(LevelProgressionDTO p_levelProgressionDTO)
        {
            return new()
            {
                IsUnlocked = p_levelProgressionDTO.IsUnlocked,
                BestScore = p_levelProgressionDTO.BestScore
            };
        }

        public LevelProgressionDTO ToLevelProgressionDTO(int p_userId, int p_levelId)
        {
            return new()
            {
                UserId = p_userId,
                LevelId = p_levelId,

                IsUnlocked = this.IsUnlocked,
                BestScore = this.BestScore
            };
        }
    }


    /// <summary>
    /// This Class is a singleton. Only one instance of this Class can exist at the same time.
    /// This instance will exist whatever the Scene. <para> </para>
    /// 
    /// This Class will be used to store locally the player's data, 
    /// so the FrontEnd doesn't have to call the BackEnd every time he needs a player's data.
    /// </summary>
    public class UserDataManager : MonoBehaviour
    {
        public static UserDataManager Instance;

        [HideInInspector] public const string GET_USER_API_ROUTE = "user/get";
        [HideInInspector] public const string GET_USER_PROGRESSION_API_ROUTE = "user-progression/get";

        [HideInInspector] public const string SET_USER_API_ROUTE = "user/set";
        [HideInInspector] public const string SET_USER_PROGRESSION_API_ROUTE = "user-progression/set";


        [Header("----- DEBUG -----")]
        [SerializeField] bool _isDebugModeOn;

        // User data
        int _userId = -1;
        string _username = "";

        // User progressions data
        UserProgression _userProgression = new();


        void Awake()
        {
            Instance = Instantiator.GetInstance(this, Instantiator.InstanceConflictResolutions.DestroyingDuplicateObject);

            DontDestroyOnLoad(this);
        }

        void Start()
        {
            if (string.IsNullOrEmpty(GET_USER_API_ROUTE))
                Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(GET_USER_API_ROUTE)}' constant is null or empty. Please set it.");

            if (string.IsNullOrEmpty(GET_USER_PROGRESSION_API_ROUTE))
                Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(GET_USER_PROGRESSION_API_ROUTE)}' constant is null or empty. Please set it.");


            if (string.IsNullOrEmpty(SET_USER_API_ROUTE))
                Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(SET_USER_API_ROUTE)}' constant is null or empty. Please set it.");

            if (string.IsNullOrEmpty(SET_USER_PROGRESSION_API_ROUTE))
                Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(SET_USER_PROGRESSION_API_ROUTE)}' constant is null or empty. Please set it.");
        }


        #region -- Load server data --

        #region - LoadDataFromServer sub-methods -

        IEnumerator LoadUserDataFromServer(int p_userId, Action<GetUserResponseDTO> p_onServerRequestResponse)
        {
            GetUserRequestDTO getUserDataDTO = new()
            {
                Id = p_userId,
            };

            yield return ServerRequestManager.SendRequest<GetUserResponseDTO>(
                GET_USER_API_ROUTE,
                ServerRequestManager.RequestType.Get,

                getUserDataDTO,
                true,

                result =>
                {
                    p_onServerRequestResponse?.Invoke(result);
                },

                request =>
                {
                    Debug.LogWarning($"WARNING: [{GetType().Name}] The GetUser request sent to the server failed. Returning null. {ServerRequestManager.RequestResponseToString(request)}");

                    p_onServerRequestResponse?.Invoke(null);
                }
            );
        }

        IEnumerator LoadUserProgressionsDataFromServer(int p_userId, Action<UserProgressionDTO> p_onServerRequestResponse)
        {
            GetUserRequestDTO getUserDataDTO = new()
            {
                Id = p_userId,
            };

            yield return ServerRequestManager.SendRequest<UserProgressionDTO>(
                GET_USER_PROGRESSION_API_ROUTE,
                ServerRequestManager.RequestType.Get,

                getUserDataDTO,
                true,

                result =>
                {
                    p_onServerRequestResponse?.Invoke(result);
                },

                request =>
                {
                    Debug.LogWarning($"WARNING: [{GetType().Name}] The GetUserProgressions request sent to the server failed. Returning null. {ServerRequestManager.RequestResponseToString(request)}");

                    p_onServerRequestResponse?.Invoke(null);
                }
            );
        }

        #endregion

        public IEnumerator LoadDataFromServer(int p_userId)
        {
            // -- Sending request to server -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Sending requests to the server.");

            GetUserResponseDTO userData = null;
            UserProgressionDTO userProgression = null;

            // Note:
            // In a real project, we will send the requests in parallel to avoid waiting for to each request to finish.

            yield return LoadUserDataFromServer(
                p_userId,
                result => userData = result
            );

            yield return LoadUserProgressionsDataFromServer(
                p_userId,
                result => userProgression = result
            );

            // -- Checking if the request succeeded -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Checking if the requests sent to the server succeeded.");

            if (userData == null)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The GetUser request sent to the server failed. Returning.");
                yield break;
            }

            if (userProgression == null)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The GetUserProgressions request sent to the server failed. Returning.");
                yield break;
            }

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The GetUser and GetUserProgressions requests sent to the server succeeded.");

            // -- Setting local variables (User + UserProgressions) -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Setting up local variables.");

            // - User data - //

            // Converting the GetUserResponseDTO into usable variables
            _userId = userData.Id;
            _username = userData.Username;

            // - User progressions data - //

            // Converting the UserProgressionDTO into UserProgression
            _userProgression = (UserProgression)userProgression;
        }

        #endregion


        #region -- Get local data --

        #region - User data -

        public int GetUserId()
        {
            return _userId;
        }

        public string GetUsername()
        {
            return _username;
        }

        #endregion

        #region - User progression data -

        public LevelProgression GetLevelProgression(int p_levelId)
        {
            if (!_userProgression.LevelProgressions.TryGetValue(p_levelId, out LevelProgression levelProgressionData))
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] There is no {nameof(LevelProgression)} associated with level id '{p_levelId}'. Returning null.");
                return null;
            }

            return levelProgressionData;
        }

        /// <summary>
        /// Returns a read-only version of all LevelProgressions.
        /// 
        /// <para> If you want to modify any LevelProgression, use the <see cref="UpdateLevelProgression"/> method. </para>
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<int, LevelProgression> GetAllLevelProgressions()
        {
            return _userProgression.LevelProgressions;
        }

        #endregion

        #endregion

        #region -- Update local data --

        #region - User data -

        // There are no method to modify the User data.
        // These values should not be modified.

        #endregion

        #region - User progression data -

        public void UpdateLevelProgression(int p_levelId, LevelProgression p_newLevelProgressionData)
        {
            // Note:
            // We don't replace the variable because we don't want to break possible references.
            // That's why we replace the values of the LevelProgression but not the LevelProgression itself.
            
            LevelProgression levelProgressionData = GetLevelProgression(p_levelId);

            if (levelProgressionData == null)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] There is no {nameof(LevelProgression)} associated with the level id: {p_levelId}. No modifications have been done. Returning.");
                return;
            }

            levelProgressionData.IsUnlocked = p_newLevelProgressionData.IsUnlocked;
            levelProgressionData.BestScore = p_newLevelProgressionData.BestScore;
        }

        #endregion

        #endregion


        #region -- Save local data --

        #region - User data -

        // There are no method to save the User data.
        // These values should not be modified in the first place, so having methods to saves those changes will be useless.

        #endregion

        #region - User progression data -

        public IEnumerator SaveAllUserProgressions()
        {
            // -- Converting UserProgression into UserProgressionDTO -- //

            UserProgressionDTO userProgressionDTO = _userProgression.ToUserProgressionDTO(_userId);

            // -- Sending request -- //

            yield return ServerRequestManager.SendRequest<ApiResponseDTO>(
                SET_USER_PROGRESSION_API_ROUTE,
                ServerRequestManager.RequestType.Put,

                userProgressionDTO,
                true,

                result =>
                {
                    if (_isDebugModeOn)
                        Debug.Log($"DEBUG: [{GetType().Name}] The SaveAllUserProgressions request sent to the server succeeded.");
                },

                request =>
                {
                    Debug.LogWarning($"WARNING: [{GetType().Name}] The SaveAllUserProgressions request sent to the server failed. Returning. {ServerRequestManager.RequestResponseToString(request)}");
                    return;
                }
            );
        }

        #endregion

        /// <summary>
        /// Sends a request to the server to save all local User's data.
        /// 
        /// <para>
        /// <b>BEWARE:</b> This method is a coroutine and must be started using
        /// <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/>.
        /// </para>
        /// </summary>
        public IEnumerator SaveAllUserData()
        {
            // -- Sending request to server -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Sending a request to the server to save all local User's data inside the DataBase.");

            yield return SaveAllUserProgressions();
        }

        #endregion


        #region -- Session management --

        /// <summary>
        /// Should be called when disconnecting or deleting the User account.
        /// </summary>
        public void ClearAllLocalData()
        {
            _userId = -1;
            _username = "";

            _userProgression = new();
        }

        #endregion
    }
}