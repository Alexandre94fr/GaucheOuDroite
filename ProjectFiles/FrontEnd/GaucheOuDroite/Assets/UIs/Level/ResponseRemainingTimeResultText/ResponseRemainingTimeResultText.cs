using TMPro;
using UnityEngine;

using VariableCheckerPackage;


public class ResponseRemainingTimeResultText : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] TextMeshProUGUI _responseRemainingTimeResultText;
    [SerializeField] TextMeshProUGUI _addedScoreText;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_responseRemainingTimeResultText, nameof(_responseRemainingTimeResultText)),
            (_addedScoreText, nameof(_addedScoreText))
        )) return;

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

        _responseRemainingTimeResultText.text = ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_NAMES_IN_FRENCH[p_responseResult.Result];
        _responseRemainingTimeResultText.color = ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_COLORS[p_responseResult.Result];

        _addedScoreText.text = "+" + p_responseResult.AddedScore;
    }
}
