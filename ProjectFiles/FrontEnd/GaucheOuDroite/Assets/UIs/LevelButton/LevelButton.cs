using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.Game;
using FrontEnd.Data.User;

using Shared.Constants;
using Shared.Enums;


[Serializable]
public class LevelButtonStar
{
    public Image FillImage;
    public Image FillShadowImage;

    public bool IsFilled = false;

    
    public void SetFilled(bool p_isFilled, Color p_emptyColor, Color p_filledColor)
    {
        if (p_isFilled)
        {
            IsFilled = true;

            FillImage.color = p_filledColor;
            FillShadowImage.color = p_filledColor;
        }
        else
        {
            IsFilled = false;

            FillImage.color = p_emptyColor;
            FillShadowImage.color = p_emptyColor;
        }
    }
}

public class LevelButton : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] SceneChanger _sceneChanger;

    [Space]
    [SerializeField] Button _levelButton;

    [Space]
    [SerializeField] TextMeshProUGUI _levelNameText;
    [SerializeField] TextMeshProUGUI _difficultyText;

    [Space]
    [SerializeField] TextMeshProUGUI _bestScoreValueText;

    [Space]
    [SerializeField] Image _fogLockImage;

    [Space]
    [SerializeField] LevelButtonStar _star1;
    [SerializeField] LevelButtonStar _star2;
    [SerializeField] LevelButtonStar _star3;


    [Header("Properties:")]
    [SerializeField] int _associatedLevelId = -1;

    [Space]
    [SerializeField] string _levelSceneName = "Level";

    // These properties can technically be inside the GameData
    [Space]
    [SerializeField] Color _emptyStarColor = Color.white;
    [SerializeField] Color _fullStarColor = new(0.85f, 0.7725f, 0.18f); // Gold

    [Space]
    [Space]
    [SerializeField] float _minRandomScaleMultiplier = 0.85f;
    [SerializeField] float _maxRandomScaleMultiplier = 1.15f;

    [Space]
    [SerializeField] float _minRandomRotation = 0.0f;
    [SerializeField] float _maxRandomRotation = 360.0f;

    [Space]
    [SerializeField] float _minRandomTransparencyMultiplier = 0.975f;
    [SerializeField] float _maxRandomTransparencyMultiplier = 1.05f;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_sceneChanger, nameof(_sceneChanger)),

            (_levelButton, nameof(_levelButton)),

            (_levelNameText, nameof(_levelNameText)),
            (_difficultyText, nameof(_difficultyText)),

            (_bestScoreValueText, nameof(_bestScoreValueText)),

            (_fogLockImage, nameof(_fogLockImage)),

            (_star1.FillImage, nameof(_star1) + "." + nameof(_star1.FillImage)),
            (_star1.FillShadowImage, nameof(_star1) + "." + nameof(_star1.FillShadowImage)),

            (_star2.FillImage, nameof(_star2) + "." + nameof(_star2.FillImage)),
            (_star2.FillShadowImage, nameof(_star2) + "." + nameof(_star2.FillShadowImage)),

            (_star3.FillImage, nameof(_star3) + "." + nameof(_star3.FillImage)),
            (_star3.FillShadowImage, nameof(_star3) + "." + nameof(_star3.FillShadowImage))
        )) return;

        if (_associatedLevelId <= 0)
        {
            Debug.LogError(
                $"{VariablesChecker.GetCheckErrorMessagePrefix(name, nameof(_associatedLevelId))} is inferior or equal to 0, " +
                $"please set it through the Unity inspector. Returning."
            );
            return;
        }

        if (string.IsNullOrEmpty(_levelSceneName))
        {
            Debug.LogError(
                $"{VariablesChecker.GetCheckErrorMessagePrefix(name, nameof(_levelSceneName))} is null or empty, " +
                $"please set it through the Unity inspector. Returning."
            );
            return;
        }

        // -- Getting the Game's and User's data -- //

        Level levelData = GameDataManager.Instance.GetLevel(_associatedLevelId);
        LevelProgression userLevelProgressionData = UserDataManager.Instance.GetLevelProgression(_associatedLevelId);
        
        // -- Updating the LevelButton visuals based on the Game's and User's data -- //

        UpdateName(levelData.Name);
        UpdateDifficulty(levelData.Difficulty);
        UpdateBestScore(userLevelProgressionData.BestScore);

        UpdateStars(
            userLevelProgressionData.BestScore,
            levelData.Star1MinimumScore,
            levelData.Star2MinimumScore,
            levelData.Star3MinimumScore
        );

        UpdateFogLock(userLevelProgressionData.IsUnlocked);

        // -- Updating the LevelButton fonctionnalities based on the Game's and User's data -- //

        UpdateLevelAccecibility(userLevelProgressionData.IsUnlocked);
    }


    #region -- Updating visuals --

    void UpdateName(string p_levelName)
    {
        _levelNameText.text = p_levelName;
    }

    void UpdateDifficulty(LevelDifficulty p_levelDifficulty)
    {
        Debug.Log($"Level difficulty for {_levelButton.transform.parent.name}: {p_levelDifficulty}");

        // Updating text
        _difficultyText.text = LevelDifficultyProperties.DIFFICULTY_NAMES_IN_FRENCH[p_levelDifficulty];

        // Updating text's color
        Color newColor = new(
            LevelDifficultyProperties.DIFFICULTY_COLORS[p_levelDifficulty].X,
            LevelDifficultyProperties.DIFFICULTY_COLORS[p_levelDifficulty].Y,
            LevelDifficultyProperties.DIFFICULTY_COLORS[p_levelDifficulty].Z
        );

        _difficultyText.color = newColor;
    }

    void UpdateBestScore(int p_bestScore)
    {
        _bestScoreValueText.text = p_bestScore.ToString();
    }

    void UpdateStars(int p_bestScore, int p_star1MinimumScore, int p_star2MinimumScore, int p_star3MinimumScore)
    {
        _star1.SetFilled(false, _emptyStarColor, _fullStarColor);
        _star2.SetFilled(false, _emptyStarColor, _fullStarColor);
        _star3.SetFilled(false, _emptyStarColor, _fullStarColor);

        if (p_bestScore >= p_star1MinimumScore)
            _star1.SetFilled(true, _emptyStarColor, _fullStarColor);

        if (p_bestScore >= p_star2MinimumScore)
            _star2.SetFilled(true, _emptyStarColor, _fullStarColor);

        if (p_bestScore >= p_star3MinimumScore)
            _star3.SetFilled(true, _emptyStarColor, _fullStarColor);
    }

    void UpdateFogLock(bool p_isLevelUnlocked)
    {
        // -- Making the fog visible depending on Level's lock state -- //

        if (p_isLevelUnlocked)
        {
            _fogLockImage.enabled = false;
        }
        else
        {
            _fogLockImage.enabled = true;
        }

        // -- Randomizing a little bit some fog's properties -- //

        RectTransform fogRectTransform = _fogLockImage.GetComponent<RectTransform>();

        float randomScaleMultiplier = UnityEngine.Random.Range(_minRandomScaleMultiplier, _maxRandomScaleMultiplier);
        float randomRotation = UnityEngine.Random.Range(_minRandomRotation, _maxRandomRotation);
        float randomTransparencyMultiplier = UnityEngine.Random.Range(_minRandomTransparencyMultiplier, _maxRandomTransparencyMultiplier);

        fogRectTransform.localScale = new Vector3(
            fogRectTransform.localScale.x * randomScaleMultiplier,
            fogRectTransform.localScale.y * randomScaleMultiplier,
            fogRectTransform.localScale.z * randomScaleMultiplier
        );

        fogRectTransform.rotation = Quaternion.Euler(
            fogRectTransform.rotation.x,
            fogRectTransform.rotation.y,
            randomRotation
        );

        _fogLockImage.color = new Color(
            _fogLockImage.color.r,
            _fogLockImage.color.g,
            _fogLockImage.color.b,
            _fogLockImage.color.a * randomTransparencyMultiplier
        );
    }

    #endregion

    #region -- Updating functionalities --

    void UpdateLevelAccecibility(bool p_isLevelUnlocked)
    {
        if (p_isLevelUnlocked)
        {
            _levelButton.interactable = true;
        }
        else
        {
            _levelButton.interactable = false;
        }
    }

    #endregion


    public void OnButtonPressed()
    {
        // -- Telling the LevelManager which Level we will load -- //

        // TODO:

        // -- Switching Scene -- //

        _sceneChanger.LoadAsync(_levelSceneName);
    }
}