using System.Collections.Generic;
using UnityEngine;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.Game;


public class ResponseRemainingTimeResultText : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] TextMeshProUGUI _responseRemainingTimeResultText;
    [SerializeField] TextMeshProUGUI _addedScoreText;


    Dictionary<ResponseProperties.ResponseRemainingTimeResult, string> RESPONSE_REMAINING_TIME_RESULT_NAMES = new();


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_responseRemainingTimeResultText, nameof(_responseRemainingTimeResultText)),
            (_addedScoreText, nameof(_addedScoreText))
        )) return;

        // -- Setting the different properties to the right language -- //

        switch (GameDataManager.Instance.GameLanguage)
        {
            case GameLanguage.French:

                RESPONSE_REMAINING_TIME_RESULT_NAMES = ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_NAMES_IN_FRENCH;

                break;

            case GameLanguage.English:

                RESPONSE_REMAINING_TIME_RESULT_NAMES = ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_NAMES_IN_ENGLISH;

                break;

            default:
                Debug.LogError($"ERROR: [{GetType().Name}] There is no case planned in the switch for '{GameDataManager.Instance.GameLanguage}'. Using the Level name inside the DataBase.");
                break;
        }

        // -- Handling events -- //

        EventHandler.OnCorrectResponseProcessedEvent += OnCorrectResponseProcessed;
    }

    void OnDestroy()
    {
        EventHandler.OnCorrectResponseProcessedEvent -= OnCorrectResponseProcessed;
    }


    void OnCorrectResponseProcessed(ResponseResult p_responseResult)
    {
        if (_responseRemainingTimeResultText.enabled == false)
            _responseRemainingTimeResultText.enabled = true;

        if (_addedScoreText.enabled == false)
            _addedScoreText.enabled = true;

        _responseRemainingTimeResultText.text = RESPONSE_REMAINING_TIME_RESULT_NAMES[p_responseResult.Result];
        _responseRemainingTimeResultText.color = ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_COLORS[p_responseResult.Result];

        _addedScoreText.text = "+" + p_responseResult.AddedScore;
    }
}