using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InstantiatorPackage;

using Shared.Enums;
using Shared.DTOs.Data.Game;


namespace FrontEnd.Data.Game
{
    public enum GameLanguage
    {
        French,
        English,
    }


    public class GameData
    {
        public Dictionary<int, Level> Levels { get; internal set; } = new();


        public static explicit operator GameData(GetGameDataResponseDTO p_getGameDataResponseDTO)
        {
            // Converting GetGameDataResponseDTO.Levels into GameData.Levels
            Dictionary<int, Level> levels = new(p_getGameDataResponseDTO.Levels.Count);

            foreach ((int levelId, LevelDTO levelDTO) in p_getGameDataResponseDTO.Levels)
            {
                levels.Add(levelId, (Level)levelDTO);
            }

            return new()
            {
                Levels = levels
            };
        }
    }

    public class Level
    {
        // We don't store the Level's Id associated with this Level,
        // because the GameData.Levels dictionary already store the LevelId as Keys.

        public string Name { get; internal set; } = "";

        public LevelDifficulty Difficulty { get; internal set; } = 0;

        public bool IsInfinite { get; internal set; } = false;

        public string ResponseSequence { get; internal set; } = "";

        public List<LevelResponseTimeStep> LevelResponseTimeSteps { get; internal set; } = new();

        public int Star1MinimumScore { get; internal set; } = 0;

        public int Star2MinimumScore { get; internal set; } = 0;

        public int Star3MinimumScore { get; internal set; } = 0;


        public static explicit operator Level(LevelDTO p_levelDTO)
        {
            // Converting the LevelResponseTimeStepsDTO to LevelResponseTimeSteps
            List<LevelResponseTimeStep> levelResponseTimeSteps = new(p_levelDTO.LevelResponseTimeSteps.Count);

            foreach (LevelResponseTimeStepDTO levelResponseTimeStepDTO in p_levelDTO.LevelResponseTimeSteps)
            {
                levelResponseTimeSteps.Add((LevelResponseTimeStep)levelResponseTimeStepDTO);
            }

            // Converting the LevelDTO to Level
            return new()
            {
                Name = p_levelDTO.Name,
                Difficulty = p_levelDTO.Difficulty,

                IsInfinite = p_levelDTO.IsInfinite,
                ResponseSequence = p_levelDTO.ResponseSequence,
                LevelResponseTimeSteps = levelResponseTimeSteps,

                Star1MinimumScore = p_levelDTO.Star1MinimumScore,
                Star2MinimumScore = p_levelDTO.Star2MinimumScore,
                Star3MinimumScore = p_levelDTO.Star3MinimumScore,
            };
        }
    }

    public class LevelResponseTimeStep
    {
        public float MinimumCorrectResponses { get; set; }

        public float MaximumResponseTimeInSeconds { get; set; }


        public static explicit operator LevelResponseTimeStep(LevelResponseTimeStepDTO p_levelResponseTimeStepDTO)
        {
            return new()
            {
                MinimumCorrectResponses = p_levelResponseTimeStepDTO.MinimumCorrectResponses,
                MaximumResponseTimeInSeconds = p_levelResponseTimeStepDTO.MaximumResponseTimeInSeconds,
            };
        }
    }


    /// <summary>
    /// This Class is a singleton. Only one instance of this Class can exist at the same time.
    /// This instance will exist whatever the Scene. <para> </para>
    /// 
    /// This Class will be used to store locally the game's data, 
    /// so the FrontEnd doesn't have to call the BackEnd every time he needs a game's data.
    /// </summary>
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance;

        // Note:
        // Because we are using the REST design,
        // the Get, Put, and more, actions are directly linked to the Class' API route.
        // This is why each action doesn't have its own route.
        [HideInInspector] public const string GAME_DATA_API_ROUTE = "game-data";


        /// <summary>
        /// Has the manager loaded the data inside the DataBase on the server-side (BackEnd). <para></para>
        /// 
        /// If you want to load the data, call the: <see cref="LoadDataFromServerAsync"/> method.
        /// </summary>
        [HideInInspector] public bool HasLoadedServerData = false;

        /// <summary>
        /// Tells in what language the FrontEnd (Unity) is. <para></para>
        /// 
        /// If the project scales up, this property may be saved on the server and the property will be used to update ALL the UIs automatically.
        /// </summary>
        [HideInInspector] public readonly GameLanguage GameLanguage = GameLanguage.English;


        [Header("----- DEBUG -----")]
        [SerializeField] bool _isDebugModeOn;

        // User progressions data
        GameData _gameData = new();


        void Awake()
        {
            Instance = Instantiator.GetInstance(this, Instantiator.InstanceConflictResolutions.DestroyingDuplicateObject);

            DontDestroyOnLoad(this);
        }

        void Start()
        {
            if (string.IsNullOrEmpty(GAME_DATA_API_ROUTE))
                Debug.LogWarning($"WARNING: [{GetType().Name}] The '{nameof(GAME_DATA_API_ROUTE)}' constant is null or empty. Please set it.");
        }


        #region -- Load server data --

        #region - LoadDataFromServer sub-methods -

        IEnumerator LoadGameDataFromServerAsync(Action<GetGameDataResponseDTO> p_onServerRequestResponse)
        {
            yield return ServerRequestManager.SendRequest<GetGameDataResponseDTO>(
                GAME_DATA_API_ROUTE,
                ServerRequestManager.RequestType.Get,

                null,
                true,

                result =>
                {
                    p_onServerRequestResponse?.Invoke(result);
                },

                request =>
                {
                    Debug.LogWarning($"WARNING: [{GetType().Name}] The GetGameData request sent to the server failed. Returning null. {ServerRequestManager.RequestResponseToString(request)}");

                    p_onServerRequestResponse?.Invoke(null);
                }
            );
        }

        #endregion

        public IEnumerator LoadDataFromServerAsync(Action p_onLoadSuccess = null)
        {
            // -- Sending request to server -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Sending requests to the server.");

            GetGameDataResponseDTO gameDataResponseDTO = null;

            // Note:
            // In a real project, we will send the requests in parallel to avoid waiting for to each request to finish.

            yield return LoadGameDataFromServerAsync(
                result => gameDataResponseDTO = result
            );

            // -- Checking if the request succeeded -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Checking if the requests sent to the server succeeded.");


            if (gameDataResponseDTO == null)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The GetGameData request sent to the server failed. Received null.");
                yield break;
            }

            if (!gameDataResponseDTO.HasSucceeded)
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] The GetGameData request sent to the server failed. Returning.\nError: {gameDataResponseDTO.ErrorMessage}");
                yield break;
            }


            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] The GetGameData request sent to the server succeeded.");

            // -- Setting local variables (GameData) -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Setting up local variables.");

            // - Game data - //

            // Converting the GetGameDataResponseDTO into GameData
            _gameData = (GameData)gameDataResponseDTO;

            // -- Updating manager's state -- //

            HasLoadedServerData = true;

            // -- Calling the callback -- //

            p_onLoadSuccess?.Invoke();
        }

        #endregion


        #region -- Get local data --

        #region - Game's data -

        /// <summary>
        /// Returns if the method managed to get the <see cref="Level"/> reference. <para></para>
        /// 
        /// If true, the <paramref name="p_level"/> will contain the <see cref="Level"/> reference, otherwise null.
        /// </summary>
        /// <param name="p_levelId"> The Id of the <see cref="Level"/> you want. </param>
        public bool TryGetLevel(int p_levelId, out Level p_level)
        {
            p_level = null;

            if (!_gameData.Levels.TryGetValue(p_levelId, out Level level))
            {
                if (_isDebugModeOn)
                    Debug.Log($"DEBUG: [{GetType().Name}] Failed to get the {nameof(Level)} {p_levelId}. There is no {nameof(Level)} associated with {nameof(Level)} Id: {p_levelId}. Returning false.");

                return false;
            }

            p_level = level;

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Successfully got the {nameof(Level)} {p_levelId}. Returning true.");
            
            return true;
        }

        /// <summary>
        /// Returns a read-only version of GameData.Levels.
        /// 
        /// <para> You should not modify any of the Game's data. </para>
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<int, Level> GetAllLevels()
        {
            return _gameData.Levels;
        }

        #endregion

        #endregion


        #region -- Session management --

        /// <summary>
        /// Should be called when disconnecting or deleting the User account.
        /// </summary>
        public void ClearAllLocalData()
        {
            HasLoadedServerData = false;

            _gameData = new();
        }

        #endregion
    }
}