using UnityEngine;
using UnityEngine.UI;

using VariableCheckerPackage;


public class NextLevelButton : MonoBehaviour
{
    [Header("Internal references:")]
    [SerializeField] Button _button;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_button, nameof(_button))
        )) return;
    }


    public void OnButtonPressed()
    {
        SetButtonInteractability(false);

        LevelManager.Instance.StartNextLevel();
    }

    public void SetButtonInteractability(bool p_newValue)
    {
        _button.interactable = p_newValue;
    }
}