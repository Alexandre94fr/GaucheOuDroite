using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.User;


[RequireComponent(typeof(RectTransform))]
public class Dropdown : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] Button _dropdownButton;
    [SerializeField] TextMeshProUGUI _dropdownTitleText;
    [SerializeField] GameObject _dropdownArrow;

    [Header("Properties:")]
    [SerializeField] int _dropdownArrowOrientationWhenDropdownOff = 0;
    [SerializeField] int _dropdownArrowOrientationWhenDropdownOn = -90;

    [Space]
    [SerializeField] Vector2 _firstGameObjectShownPositionOffset = new(0, -90);
    [SerializeField] Vector2 _otherGameObjectsShownPositionOffset = new(0, -65);


    [Space]
    [SerializeField] List<GameObject> _gameObjectsShownWhenDropdownOn = new();


    bool _isDropdownOn = false;

    RectTransform _rectTransform;
    List<RectTransform> _gameObjectsShownWhenDropdownOnRectTransforms = new();


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_dropdownTitleText, nameof(_dropdownTitleText)),
            (_dropdownButton, nameof(_dropdownButton)),
            (_dropdownArrow, nameof(_dropdownArrow))
        )) return;

        // Dropdown RectTransform caching
        _rectTransform = GetComponent<RectTransform>();

        _gameObjectsShownWhenDropdownOnRectTransforms = new(_gameObjectsShownWhenDropdownOn.Count);

        for (int i = 0; i < _gameObjectsShownWhenDropdownOn.Count; i++)
        {
            // Checking if there are any null values inside the _gameObjectsShownWhenDropdownOn properties
            if (_gameObjectsShownWhenDropdownOn[i] == null)
            {
                Debug.LogError(
                    $"{VariablesChecker.GetCheckErrorMessagePrefix(name, nameof(_gameObjectsShownWhenDropdownOn))} contains a null value. The element {i} of the list is null.\n" +
                    $"Please set it through the Unity inspector. Continuing."
                );
                continue;
            }

            // Other GameObjects RectTransform caching + checking
            if (!_gameObjectsShownWhenDropdownOn[i].TryGetComponent(out RectTransform rectTransform))
            {
                Debug.LogError(
                    $"{VariablesChecker.GetCheckErrorMessagePrefix(name, nameof(_gameObjectsShownWhenDropdownOn))} contains a GameObject that doesn't have RectTransform component. " + 
                    "The element {i} of the list is not correct. Continuing."
                );
                continue;
            }

            _gameObjectsShownWhenDropdownOnRectTransforms.Add(rectTransform);
        }

        // -- Updating the Dropdown visuals based on the User's data -- //

        UpdateTitleText(UserDataManager.Instance.GetUsername());
    }


    void UpdateTitleText(string p_newText)
    {
        _dropdownTitleText.text = p_newText;
    }


    void ChangeDropdownArrowOrientation(int p_newOrientation)
    {
        _dropdownArrow.transform.rotation = Quaternion.Euler(new Vector3(
            _dropdownArrow.transform.rotation.x,
            _dropdownArrow.transform.rotation.y,
            p_newOrientation
        ));
    }

    public void OnDropdownButtonPressed()
    {
        _isDropdownOn = !_isDropdownOn;

        // -- Visuals -- //

        if (_isDropdownOn)
            ChangeDropdownArrowOrientation(_dropdownArrowOrientationWhenDropdownOn);
        else
            ChangeDropdownArrowOrientation(_dropdownArrowOrientationWhenDropdownOff);

        // -- Functionalities -- // 

        for (int i = 0; i < _gameObjectsShownWhenDropdownOn.Count; i++)
        {
            // Visibility
            _gameObjectsShownWhenDropdownOn[i].SetActive(_isDropdownOn);

            // Position
            if (i == 0)
            {
                _gameObjectsShownWhenDropdownOnRectTransforms[i].anchoredPosition = _rectTransform.anchoredPosition + _firstGameObjectShownPositionOffset;
                
                continue;
            }

            _gameObjectsShownWhenDropdownOnRectTransforms[i].anchoredPosition = _gameObjectsShownWhenDropdownOnRectTransforms[i - 1].anchoredPosition + _otherGameObjectsShownPositionOffset;
        }
    }
}