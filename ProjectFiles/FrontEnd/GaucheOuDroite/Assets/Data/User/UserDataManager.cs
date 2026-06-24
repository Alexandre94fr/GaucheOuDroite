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

        public static explicit operator UserProgressionDTO(UserProgression p_userProgression)
        {
            UserProgressionDTO userProgressionDTO = new()
            {
                LevelProgressions = new(p_userProgression.LevelProgressions.Count)
            };

            // Converting LevelProgression into LevelProgressionDTO
            foreach ((int levelId, LevelProgression levelProgression) in p_userProgression.LevelProgressions)
            {
                userProgressionDTO.LevelProgressions.Add(
                    levelProgression.ToLevelProgressionDTO(levelId)
                );
            }

            return userProgressionDTO;
        }
    }

    public class LevelProgression
    {
        public int Id { get; internal set; } = -1;

        // We don't store the LevelId associated with this LevelProgression,
        // because the UserProgression.LevelProgressions dictionary already store the LevelId as Keys.

        public bool IsUnlocked { get; internal set; } = false;

        public int BestScore { get; internal set; } = -1;


        public static explicit operator LevelProgression(LevelProgressionDTO p_levelProgressionDTO)
        {
            return new()
            {
                Id = p_levelProgressionDTO.Id,

                IsUnlocked = p_levelProgressionDTO.IsUnlocked,
                BestScore = p_levelProgressionDTO.BestScore
            };
        }

        public LevelProgressionDTO ToLevelProgressionDTO(int p_levelId)
        {
            return new()
            {
                Id = this.Id,
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
        // TODO: Implement a Dirty system, so when we need to send data to the server,
        // we can send only the dirty data.
        //
        // Note: For now it's not necessary because there are not a lot of data to send to the server,
        // but if it was a bigger, and real project, the system will be necessary.


        public static UserDataManager Instance;

        // Note:
        // Because we are using the REST design,
        // the Get, Put, and more, actions are directly linked to the Class' API route.
        // This is why each action doesn't have its own route.
        [HideInInspector] public const string USER_API_ROUTE = "users";
        [HideInInspector] public const string USER_PROGRESSION_API_ROUTE = "user-progressions";


        /// <summary>
        /// Has the manager loaded the data inside the DataBase on the server-side (BackEnd). <para></para>
        /// 
        /// If you want to load the data, call the: <see cref="LoadDataFromServerAsync"/> method.
        /// </summary>
        [HideInInspector] public bool HasLoadedServerData = false;


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
            if (string.IsNullOrEmpty(USER_API_ROUTE))
                Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(USER_API_ROUTE)}' constant is null or empty. Please set it.");

            if (string.IsNullOrEmpty(USER_PROGRESSION_API_ROUTE))
                Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(USER_PROGRESSION_API_ROUTE)}' constant is null or empty. Please set it.");
        }


        #region -- Load server data --

        #region - LoadDataFromServer sub-methods -

        IEnumerator LoadUserDataFromServerAsync(int p_userId, Action<GetUserResponseDTO> p_onServerRequestResponse)
        {
            yield return ServerRequestManager.SendRequest<GetUserResponseDTO>(
                USER_API_ROUTE,
                ServerRequestManager.RequestType.Get,

                null,
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

        IEnumerator LoadUserProgressionDataFromServerAsync(int p_userId, Action<GetUserProgressionResponseDTO> p_onServerRequestResponse)
        {
            yield return ServerRequestManager.SendRequest<GetUserProgressionResponseDTO>(
                USER_PROGRESSION_API_ROUTE,
                ServerRequestManager.RequestType.Get,

                null,
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

        public IEnumerator LoadDataFromServerAsync(int p_userId, Action p_onLoadSuccess = null) // TODO: Remove the p_userId, not necessary
        {
            // -- Sending request to server -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Sending requests to the server.");

            GetUserResponseDTO userResponseDTO = null;
            GetUserProgressionResponseDTO userProgressionResponseDTO = null;

            UserProgressionDTO userProgressionDTO = new();

            // Note:
            // In a real project, we will send the requests in parallel to avoid waiting for to each request to finish.

            yield return LoadUserDataFromServerAsync(
                p_userId,
                result => userResponseDTO = result
            );

            yield return LoadUserProgressionDataFromServerAsync(
                p_userId,
                result => userProgressionResponseDTO = result
            );

            // -- Checking if the request succeeded -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Checking if the requests sent to the server succeeded.");


            if (userResponseDTO == null)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The GetUser request sent to the server failed. Received null.");
                yield break;
            }

            if (!userResponseDTO.HasSucceeded)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The GetUser request sent to the server failed. Returning.\nError: {userResponseDTO.ErrorMessage}");
                yield break;
            }


            if (userResponseDTO == null)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The GetUserProgression request sent to the server failed. Received null.");
                yield break;
            }

            if (!userProgressionResponseDTO.HasSucceeded)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The GetUserProgression request sent to the server failed. Returning.\nError: {userProgressionResponseDTO.ErrorMessage}");
                yield break;
            }


            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The GetUser and GetUserProgression requests sent to the server succeeded.");

            // -- Setting local variables (User + UserProgressions) -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Setting up local variables.");

            // - User data - //

            // Converting the GetUserResponseDTO into usable variables
            _userId = userResponseDTO.Id;
            _username = userResponseDTO.Username;

            // - User progressions data - //

            // Converting the UserProgressionDTO into UserProgression
            userProgressionDTO.LevelProgressions = userProgressionResponseDTO.LevelProgressions;

            _userProgression = (UserProgression)userProgressionDTO;

            // -- Updating manager's state -- //

            HasLoadedServerData = true;

            // -- Calling the callback -- //

            p_onLoadSuccess?.Invoke();
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

        /// <summary>
        /// Returns a reference to a specific LevelProgression for a Level.
        /// </summary>
        /// <param name="p_levelId"> The Id of the Level associated with the LevelProgression you want. </param>
        /// <returns></returns>
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

        public IEnumerator SaveAllUserProgressionsAsync()
        {
            // -- Converting UserProgression into UserProgressionDTO -- //

            UserProgressionDTO userProgressionDTO = (UserProgressionDTO)_userProgression;

            // -- Sending request -- //

            yield return ServerRequestManager.SendRequest<ApiResponseDTO>(
                USER_PROGRESSION_API_ROUTE,
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
        public IEnumerator SaveAllUserDataAsync()
        {
            // -- Sending request to server -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Sending a request to the server to save all local User's data inside the DataBase.");

            yield return SaveAllUserProgressionsAsync();
        }

        #endregion


        #region -- Session management --

        /// <summary>
        /// Should be called when disconnecting or deleting the User account.
        /// </summary>
        public void ClearAllLocalData()
        {
            HasLoadedServerData = false;

            _userId = -1;
            _username = "";

            _userProgression = new();
        }

        #endregion
    }
}