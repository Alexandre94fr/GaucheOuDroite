using UnityEngine;
using UnityEngine.UI;
using TMPro;

using VariableCheckerPackage;


public class PasswordVisualizer : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    [Header("Internal references:")]
    [SerializeField] Image _passwordVisualizerButtonImage;

    [Space]
    [SerializeField] TMP_InputField _passwordInputField;

    [Header("Properties:")]
    [SerializeField] Sprite _passwordEyeOpen;
    [SerializeField] Sprite _passwordEyeClose;



    bool _isPasswordVisible = false;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_passwordVisualizerButtonImage, nameof(_passwordVisualizerButtonImage)),

            (_passwordInputField, nameof(_passwordInputField)),

            (_passwordEyeOpen, nameof(_passwordEyeOpen)),
            (_passwordEyeClose, nameof(_passwordEyeClose))
        )) return;
    }


    public void OnButtonPressed()
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] The '{_passwordVisualizerButtonImage.gameObject.name}' button has been pressed. Updating password visibility: {_isPasswordVisible} -> {!_isPasswordVisible}.");

        _isPasswordVisible = !_isPasswordVisible;

        UpdatePasswordVisibility(_isPasswordVisible);
        UpdatePasswordVisibilityIcon(_isPasswordVisible);
    }

    void UpdatePasswordVisibility(bool p_isPasswordVisible)
    {
        if (p_isPasswordVisible)
        {
            _passwordInputField.contentType = TMP_InputField.ContentType.Standard;
            _passwordInputField.ForceLabelUpdate();
            return;
        }

        _passwordInputField.contentType = TMP_InputField.ContentType.Password;
        _passwordInputField.ForceLabelUpdate();
    }

    void UpdatePasswordVisibilityIcon(bool p_isPasswordVisible)
    {
        if (p_isPasswordVisible)
        {
            _passwordVisualizerButtonImage.sprite = _passwordEyeOpen;
            return;
        }

        _passwordVisualizerButtonImage.sprite = _passwordEyeClose;
    }
}