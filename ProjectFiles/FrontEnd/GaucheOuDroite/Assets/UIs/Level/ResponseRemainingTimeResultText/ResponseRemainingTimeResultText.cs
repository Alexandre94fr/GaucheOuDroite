using UnityEngine;
using TMPro;

using VariableCheckerPackage;


public class ResponseRemainingTimeResultText : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] TextMeshProUGUI _responseRemainingTimeResultText;
    [SerializeField] TextMeshProUGUI _addedScoreText;

    [Header("Properties:")]
    [SerializeField] float _minimumRandomRotation = -5;
    [SerializeField] float _maximumRandomRotation = 5;


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
        _responseRemainingTimeResultText.transform.parent.GetComponent<RectTransform>().localScale = ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_SCALE_SIZES[p_responseResult.Result];

        RandomizeRotation();

        _addedScoreText.text = "+" + p_responseResult.AddedScore;
    }

    void RandomizeRotation()
    {
        RectTransform responseRemainingTimeResultTextRectTransform = _responseRemainingTimeResultText.GetComponent<RectTransform>();
        RectTransform addedScoreTextRectTransform = _addedScoreText.GetComponent<RectTransform>();

        float randomRotation = UnityEngine.Random.Range(_minimumRandomRotation, _maximumRandomRotation);

        responseRemainingTimeResultTextRectTransform.rotation = Quaternion.Euler(
            responseRemainingTimeResultTextRectTransform.rotation.x,
            responseRemainingTimeResultTextRectTransform.rotation.y,
            randomRotation
        );

        addedScoreTextRectTransform.rotation = Quaternion.Euler(
            addedScoreTextRectTransform.rotation.x,
            addedScoreTextRectTransform.rotation.y,
            randomRotation
        );
    }
}