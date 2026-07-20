using System.Collections.Generic;
using UnityEngine;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.Game;

using Shared.Constants;


public class AuthenticationInformation : MonoBehaviour
{
    [Header("----- DEBUG -----")]
    [SerializeField] bool _isDebugModeOn;


    [Header("External references:")]
    [SerializeField] GameObject _ruleTextPrefab;

    [Header("Internal references:")]
    [SerializeField] GameObject _usernameTextsGameObject;
    [SerializeField] GameObject _passwordTextsGameObject;


    Dictionary<AuthenticationProperties.AuthenticationErrorReasons, string> USERNAME_REQUIREMENTS_HELPING_MESSAGES = new();
    Dictionary<AuthenticationProperties.AuthenticationErrorReasons, string> PASSWORD_REQUIREMENTS_HELPING_MESSAGES = new();


    void Start()
    {
        if (!VariablesChecker.AreVariablesValid(name, null,
            (_ruleTextPrefab, nameof(_ruleTextPrefab)),
            (_usernameTextsGameObject, nameof(_usernameTextsGameObject)),
            (_passwordTextsGameObject, nameof(_passwordTextsGameObject))
        )) return;

        // -- Setting the different dictionaries to the right language -- //

        switch (GameDataManager.Instance.GameLanguage)
        {
            case GameLanguage.French:

                USERNAME_REQUIREMENTS_HELPING_MESSAGES = AuthenticationProperties.USERNAME_REQUIREMENTS_HELPING_MESSAGES_IN_FRENCH;
                PASSWORD_REQUIREMENTS_HELPING_MESSAGES = AuthenticationProperties.PASSWORD_REQUIREMENTS_HELPING_MESSAGES_IN_FRENCH;

                break;

            case GameLanguage.English:

                USERNAME_REQUIREMENTS_HELPING_MESSAGES = AuthenticationProperties.USERNAME_REQUIREMENTS_HELPING_MESSAGES_IN_ENGLISH;
                PASSWORD_REQUIREMENTS_HELPING_MESSAGES = AuthenticationProperties.PASSWORD_REQUIREMENTS_HELPING_MESSAGES_IN_ENGLISH;

                break;

            default:
                Debug.LogError($"ERROR: [{GetType().Name}] There is no case planned in the switch for '{GameDataManager.Instance.GameLanguage}'. Returning.");
                return;
        }

        // -- Initializing all the texts -- //

        // Before the game launches, some texts GameObject may already be inside the '_usernameTextsGameObject' or '_passwordTextsGameObject'.
        // To avoid any issues, we will destroy all the GameObjects inside them.
        DestroyAllGameObjectsIn(_usernameTextsGameObject);
        DestroyAllGameObjectsIn(_passwordTextsGameObject);

        InstantiateAllTexts(USERNAME_REQUIREMENTS_HELPING_MESSAGES, _ruleTextPrefab, _usernameTextsGameObject);
        InstantiateAllTexts(PASSWORD_REQUIREMENTS_HELPING_MESSAGES, _ruleTextPrefab, _passwordTextsGameObject);
    }


    void DestroyAllGameObjectsIn(GameObject p_gameObject)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Destroying all childs inside the '{p_gameObject.name}' GameObject.");

        for (int i = 0; i < p_gameObject.transform.childCount; i++)
        {
            GameObject objectToDestroy = p_gameObject.transform.GetChild(i).gameObject;

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Destroying child n°{i} '{objectToDestroy.name}' of the '{p_gameObject.name}' GameObject.");

            Destroy(objectToDestroy);
        }
    }

    void InstantiateAllTexts(Dictionary<AuthenticationProperties.AuthenticationErrorReasons, string> p_texts, GameObject p_textObjectToInstantiate, GameObject p_textsParent)
    {
        if (_isDebugModeOn)
            Debug.Log($"DEBUG: [{GetType().Name}] Instantiating all text childs inside the '{p_textsParent.name}' GameObject.");

        foreach (KeyValuePair<AuthenticationProperties.AuthenticationErrorReasons, string> text in p_texts)
        {
            #region Excluding disable rules

            // -- Excluding disable rules -- // 

            if (text.Key == AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnyLetters && !AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_LETTER)
            {
                if (_isDebugModeOn)
                    Debug.Log($"DEBUG: [{GetType().Name}] The 'AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_LETTER' is at false, we will not show this rule. Continuing.");

                continue;
            }

            if (text.Key == AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnyNumbers && !AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_NUMBER)
            {
                if (_isDebugModeOn)
                    Debug.Log($"DEBUG: [{GetType().Name}] The 'AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_NUMBER' is at false, we will not show this rule. Continuing.");

                continue;
            }

            if (text.Key == AuthenticationProperties.AuthenticationErrorReasons.PasswordDoesNotContainAnySpecialCharacters && !AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_SPECIAL_CHARACTER)
            {
                if (_isDebugModeOn)
                    Debug.Log($"DEBUG: [{GetType().Name}] The 'AuthenticationProperties.DOES_PASSWORD_MUST_HAVE_AT_LEAST_ONE_SPECIAL_CHARACTER' is at false, we will not show this rule. Continuing.");

                continue;
            }

            #endregion

            // -- Instantiating the new text GameObject -- //

            if (_isDebugModeOn)
                Debug.Log($"DEBUG: [{GetType().Name}] Instantiating text child '{p_textObjectToInstantiate.name}' inside the '{p_textsParent.name}' GameObject. Text value: '{text.Value}'.");

            GameObject ruleText = Instantiate(p_textObjectToInstantiate, Vector3.zero, Quaternion.identity, p_textsParent.transform);

            if (!ruleText.TryGetComponent(out TextMeshProUGUI textComponent))
            {
                Debug.LogWarning($"WARNING: [{GetType().Name}] There is no 'TextMeshProUGUI' component inside the '{ruleText.name}' Prefab. Destroying '{ruleText.name}'. Returning.");
                Destroy(ruleText);
                return;
            }

            // -- Applying the text (rule) -- //

            textComponent.text = $"- {text.Value}";
        }
    }
}