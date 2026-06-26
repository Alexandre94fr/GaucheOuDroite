using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.User;


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
    [SerializeField] List<GameObject> _gameObjectShownWhenDropdownOn = new();


    bool _isDropdownOn = false;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_dropdownTitleText, nameof(_dropdownTitleText)),
            (_dropdownButton, nameof(_dropdownButton)),
            (_dropdownArrow, nameof(_dropdownArrow))
        )) return;

        for (int i = 0; i < _gameObjectShownWhenDropdownOn.Count; i++)
        {
            if (_gameObjectShownWhenDropdownOn[i] == null)
            {
                Debug.LogError(
                    $"{VariablesChecker.GetCheckErrorMessagePrefix(name, nameof(_gameObjectShownWhenDropdownOn))} contains a null value. The element {i} of the list is null.\n" +
                    $"Please set it through the Unity inspector. Continuing."
                );
                continue;
            }
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

        foreach (GameObject gameObject in _gameObjectShownWhenDropdownOn)
        {
            gameObject.SetActive(_isDropdownOn);
        }
    }
}