using FrontEnd.Data.Game;
using TMPro;
using UnityEngine;

using VariableCheckerPackage;


public class ScoreBar : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] RectTransform _barRectTransform;
    [SerializeField] RectTransform _barFillRectTransform;

    [Space]
    [SerializeField] TextMeshProUGUI _playerScoreText;

    [Space]
    [SerializeField] LevelButtonStar _star1;
    [SerializeField] LevelButtonStar _star2;
    [SerializeField] LevelButtonStar _star3;

    [Space]
    [SerializeField] TextMeshProUGUI _star1ScoreText;
    [SerializeField] TextMeshProUGUI _star2ScoreText;
    [SerializeField] TextMeshProUGUI _star3ScoreText;


    [Header("Properties:")]
    // These properties can technically be inside the GameData
    [SerializeField] Color _emptyStarColor = Color.white;
    [SerializeField] Color _fullStarColor = new(0.85f, 0.7725f, 0.18f); // Gold


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_barRectTransform, nameof(_barRectTransform)),
            (_barFillRectTransform, nameof(_barFillRectTransform)),

            (_playerScoreText, nameof(_playerScoreText)),

            (_star1.FillImage, nameof(_star1) + "." + nameof(_star1.FillImage)),
            (_star1.FillShadowImage, nameof(_star1) + "." + nameof(_star1.FillShadowImage)),

            (_star2.FillImage, nameof(_star2) + "." + nameof(_star2.FillImage)),
            (_star2.FillShadowImage, nameof(_star2) + "." + nameof(_star2.FillShadowImage)),

            (_star3.FillImage, nameof(_star3) + "." + nameof(_star3.FillImage)),
            (_star3.FillShadowImage, nameof(_star3) + "." + nameof(_star3.FillShadowImage)),
            
            (_star1ScoreText, nameof(_star1ScoreText)),
            (_star2ScoreText, nameof(_star2ScoreText)),
            (_star3ScoreText, nameof(_star3ScoreText))
        )) return;
    }


    public void UpdateVisuals(Level p_levelProperties, int p_playerScore)
    {
        // -- Updating the visuals based on the Game's and player's score -- //

        UpdatePlayerScoreText(p_playerScore);
        UpdatePlayerScorePosition(p_playerScore, p_levelProperties.Star3MinimumScore);

        UpdateStarsScoreText(p_levelProperties.Star1MinimumScore, p_levelProperties.Star2MinimumScore, p_levelProperties.Star3MinimumScore);
        UpdateStarsVisuals(p_playerScore, p_levelProperties.Star1MinimumScore, p_levelProperties.Star2MinimumScore, p_levelProperties.Star3MinimumScore);
        UpdateStarsPosition(p_levelProperties.Star1MinimumScore, p_levelProperties.Star2MinimumScore, p_levelProperties.Star3MinimumScore);

        UpdateFiller(p_playerScore, p_levelProperties.Star3MinimumScore);
    }

    #region -- Update visuals sub-methods --

    float GetScoreRatio(int p_score, int p_maximumScore)
    {
        if (p_maximumScore <= 0)
        {
            Debug.LogWarning($"WARNING: [{GetType().Name}] The given '{nameof(p_maximumScore)}' is equal or inferior to 0. Returning 0.");
            return 0;
        }

        return Mathf.Clamp01((float)p_score / p_maximumScore);
    }

    float GetBarXPositionFromScore(int p_score, int p_maximumScore)
    {
        return Mathf.Lerp(
            0,
            _barRectTransform.rect.width,
            GetScoreRatio(p_score, p_maximumScore)
        );
    }


    void UpdatePlayerScoreText(int p_playerScore)
    {
        _playerScoreText.text = p_playerScore.ToString();
    }

    void UpdatePlayerScorePosition(int p_playerScore, int p_star3MinimumScore)
    {
        RectTransform playerScoreRectTransform = _playerScoreText.gameObject.GetComponent<RectTransform>();

        playerScoreRectTransform.anchoredPosition = new(
            GetBarXPositionFromScore(p_playerScore, p_star3MinimumScore),
            playerScoreRectTransform.anchoredPosition.y
        );
    }

    void UpdateStarsScoreText(int p_star1MinimumScore, int p_star2MinimumScore, int p_star3MinimumScore)
    {
        _star1ScoreText.text = p_star1MinimumScore.ToString();
        _star2ScoreText.text = p_star2MinimumScore.ToString();
        _star3ScoreText.text = p_star3MinimumScore.ToString();
    }

    void UpdateStarsVisuals(int p_playerScore, int p_star1MinimumScore, int p_star2MinimumScore, int p_star3MinimumScore)
    {
        _star1.SetFilled(false, _emptyStarColor, _fullStarColor);
        _star2.SetFilled(false, _emptyStarColor, _fullStarColor);
        _star3.SetFilled(false, _emptyStarColor, _fullStarColor);

        if (p_playerScore >= p_star1MinimumScore)
            _star1.SetFilled(true, _emptyStarColor, _fullStarColor);

        if (p_playerScore >= p_star2MinimumScore)
            _star2.SetFilled(true, _emptyStarColor, _fullStarColor);

        if (p_playerScore >= p_star3MinimumScore)
            _star3.SetFilled(true, _emptyStarColor, _fullStarColor);
    }

    void UpdateStarsPosition(int p_star1MinimumScore, int p_star2MinimumScore, int p_star3MinimumScore)
    {
        RectTransform star1RectTransform = _star1.FillImage.transform.parent.gameObject.GetComponent<RectTransform>();
        RectTransform star2RectTransform = _star2.FillImage.transform.parent.gameObject.GetComponent<RectTransform>();
        RectTransform star3RectTransform = _star3.FillImage.transform.parent.gameObject.GetComponent<RectTransform>();

        star1RectTransform.anchoredPosition = new(
            GetBarXPositionFromScore(p_star1MinimumScore, p_star3MinimumScore),
            star1RectTransform.anchoredPosition.y
        );

        star2RectTransform.anchoredPosition = new(
            GetBarXPositionFromScore(p_star2MinimumScore, p_star3MinimumScore),
            star2RectTransform.anchoredPosition.y
        );

        star3RectTransform.anchoredPosition = new(
            GetBarXPositionFromScore(p_star3MinimumScore, p_star3MinimumScore),
            star3RectTransform.anchoredPosition.y
        );
    }

    void UpdateFiller(int p_playerScore, int p_star3MinimumScore)
    {
        _barFillRectTransform.sizeDelta = new(
            GetBarXPositionFromScore(p_playerScore, p_star3MinimumScore),
            _barFillRectTransform.rect.height
        );
    }

    #endregion
}