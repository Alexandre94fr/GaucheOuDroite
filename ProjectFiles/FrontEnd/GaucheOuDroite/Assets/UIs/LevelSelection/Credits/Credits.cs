using UnityEngine;
using UnityEngine.UI;
using TMPro;

using VariableCheckerPackage;

using FrontEnd.Data.Game;


public class Credits : MonoBehaviour
{
    [Header("External references:")]
    [SerializeField] Button _creditsButton;
    [SerializeField] TextMeshProUGUI _creditsButtonText;

    [Header("Internal references:")]
    [SerializeField] GameObject _creditsUIGameObject;

    [Space][Space]
    [SerializeField] TextMeshProUGUI _titleText;

    [Space][Space]
    [SerializeField] TextMeshProUGUI _leadDeveloperTitleText;

    [Space][Space]
    [SerializeField] TextMeshProUGUI _artisticDirectorTitleText;

    [Space][Space] 
    [SerializeField] TextMeshProUGUI _testersTitleText;

    [Space]
    [SerializeField] TextMeshProUGUI _testersCategoryIndex1Text; 
    [SerializeField] TextMeshProUGUI _testersCategoryIndex2Text;

    [Space][Space]
    [SerializeField] TextMeshProUGUI _SFXsTitleText;

    [Space]
    [SerializeField] TextMeshProUGUI _SFXsCategoryIndex1Text;
    [SerializeField] TextMeshProUGUI _SFXsCategoryIndex2Text;
    [SerializeField] TextMeshProUGUI _SFXsCategoryIndex3Text;

    [Space][Space]
    [SerializeField] TextMeshProUGUI _closeButtonText;

    [Space][Space] 
    [SerializeField] TextMeshProUGUI _gameVersionText;


    // Localization
    string CREDITS_BUTTON_TEXT = null;


    string TITLE_TEXT = null;


    string LEAD_DEVELOPER_TITLE_TEXT = null;


    string ARTISTIC_DIRECTOR_TITLE_TEXT = null;


    string TESTERS_TITLE_TEXT = null;

    string TESTERS_CATEGORY_INDEX_1 = null;
    string TESTERS_CATEGORY_INDEX_2 = null;


    string SFXS_TITLE_TEXT = null;

    string SFXS_CATEGORY_INDEX_1 = null;
    string SFXS_CATEGORY_INDEX_2 = null;
    string SFXS_CATEGORY_INDEX_3 = null;


    string CLOSE_BUTTON_TEXT = null;


    string GAME_VERSION_TEXT = null;


    void Start()
    {
        // -- Class properties verifications -- //

        if (!VariablesChecker.AreVariablesValid(name, null,
            (_creditsButton, nameof(_creditsButton)),
            (_creditsButtonText, nameof(_creditsButtonText)),

            (_creditsUIGameObject, nameof(_creditsUIGameObject)),


            (_titleText, nameof(_titleText)),


            (_leadDeveloperTitleText, nameof(_leadDeveloperTitleText)),


            (_artisticDirectorTitleText, nameof(_artisticDirectorTitleText)),


            (_testersTitleText, nameof(_testersTitleText)),

            (_testersCategoryIndex1Text, nameof(_testersCategoryIndex1Text)),
            (_testersCategoryIndex2Text, nameof(_testersCategoryIndex2Text)),


            (_SFXsTitleText, nameof(_SFXsTitleText)),

            (_SFXsCategoryIndex1Text, nameof(_SFXsCategoryIndex1Text)),
            (_SFXsCategoryIndex2Text, nameof(_SFXsCategoryIndex2Text)),
            (_SFXsCategoryIndex3Text, nameof(_SFXsCategoryIndex3Text)),


            (_closeButtonText, nameof(_closeButtonText)),


            (_gameVersionText, nameof(_gameVersionText))
        )) return;

        // -- Setting the different properties to the right language -- //

        switch (GameDataManager.Instance.GameLanguage)
        {
            case GameLanguage.French:

                CREDITS_BUTTON_TEXT = CreditsProperties.CREDITS_BUTTON_TEXT_IN_FRENCH;
                
                
                TITLE_TEXT = CreditsProperties.TITLE_TEXT_IN_FRENCH;
                
                
                LEAD_DEVELOPER_TITLE_TEXT = CreditsProperties.LEAD_DEVELOPER_TITLE_TEXT_IN_FRENCH;
                
                
                ARTISTIC_DIRECTOR_TITLE_TEXT = CreditsProperties.ARTISTIC_DIRECTOR_TITLE_TEXT_IN_FRENCH;
                
                
                TESTERS_TITLE_TEXT = CreditsProperties.TESTERS_TITLE_TEXT_IN_FRENCH;
                
                TESTERS_CATEGORY_INDEX_1 = CreditsProperties.TESTERS_CATEGORY_INDEX_1_IN_FRENCH;
                TESTERS_CATEGORY_INDEX_2 = CreditsProperties.TESTERS_CATEGORY_INDEX_2_IN_FRENCH;
                
                
                SFXS_TITLE_TEXT = CreditsProperties.SFXS_TITLE_TEXT_IN_FRENCH;
                
                SFXS_CATEGORY_INDEX_1 = CreditsProperties.SFXS_CATEGORY_INDEX_1_IN_FRENCH;
                SFXS_CATEGORY_INDEX_2 = CreditsProperties.SFXS_CATEGORY_INDEX_2_IN_FRENCH;
                SFXS_CATEGORY_INDEX_3 = CreditsProperties.SFXS_CATEGORY_INDEX_3_IN_FRENCH;


                CLOSE_BUTTON_TEXT = CreditsProperties.CLOSE_BUTTON_TEXT_IN_FRENCH;


                GAME_VERSION_TEXT = CreditsProperties.GAME_VERSION_TEXT_IN_FRENCH;

                break;

            case GameLanguage.English:

                CREDITS_BUTTON_TEXT = CreditsProperties.CREDITS_BUTTON_TEXT_IN_ENGLISH;


                TITLE_TEXT = CreditsProperties.TITLE_TEXT_IN_ENGLISH;


                LEAD_DEVELOPER_TITLE_TEXT = CreditsProperties.LEAD_DEVELOPER_TITLE_TEXT_IN_ENGLISH;


                ARTISTIC_DIRECTOR_TITLE_TEXT = CreditsProperties.ARTISTIC_DIRECTOR_TITLE_TEXT_IN_ENGLISH;


                TESTERS_TITLE_TEXT = CreditsProperties.TESTERS_TITLE_TEXT_IN_ENGLISH;

                TESTERS_CATEGORY_INDEX_1 = CreditsProperties.TESTERS_CATEGORY_INDEX_1_IN_ENGLISH;
                TESTERS_CATEGORY_INDEX_2 = CreditsProperties.TESTERS_CATEGORY_INDEX_2_IN_ENGLISH;


                SFXS_TITLE_TEXT = CreditsProperties.SFXS_TITLE_TEXT_IN_ENGLISH;

                SFXS_CATEGORY_INDEX_1 = CreditsProperties.SFXS_CATEGORY_INDEX_1_IN_ENGLISH;
                SFXS_CATEGORY_INDEX_2 = CreditsProperties.SFXS_CATEGORY_INDEX_2_IN_ENGLISH;
                SFXS_CATEGORY_INDEX_3 = CreditsProperties.SFXS_CATEGORY_INDEX_3_IN_ENGLISH;


                CLOSE_BUTTON_TEXT = CreditsProperties.CLOSE_BUTTON_TEXT_IN_ENGLISH;


                GAME_VERSION_TEXT = CreditsProperties.GAME_VERSION_TEXT_IN_ENGLISH;

                break;

            default:
                Debug.LogError($"ERROR: [{GetType().Name}] There is no case planned in the switch for '{GameDataManager.Instance.GameLanguage}'. Returning.");
                return;
        }

        // -- Setting up the right texts -- //

        InitializeTexts();
    }


    void InitializeTexts()
    {
        _creditsButtonText.text = CREDITS_BUTTON_TEXT;


        _titleText.text = TITLE_TEXT;


        _leadDeveloperTitleText.text = LEAD_DEVELOPER_TITLE_TEXT;


        _artisticDirectorTitleText.text = ARTISTIC_DIRECTOR_TITLE_TEXT;


        _testersTitleText.text = TESTERS_TITLE_TEXT;

        _testersCategoryIndex1Text.text = TESTERS_CATEGORY_INDEX_1;
        _testersCategoryIndex2Text.text = TESTERS_CATEGORY_INDEX_2;


        _SFXsTitleText.text = SFXS_TITLE_TEXT;

        _SFXsCategoryIndex1Text.text = SFXS_CATEGORY_INDEX_1;
        _SFXsCategoryIndex2Text.text = SFXS_CATEGORY_INDEX_2;
        _SFXsCategoryIndex3Text.text = SFXS_CATEGORY_INDEX_3;


        _closeButtonText.text = CLOSE_BUTTON_TEXT;


        _gameVersionText.text = GAME_VERSION_TEXT;
    }

    public void OnButtonPressed()
    {
        _creditsUIGameObject.SetActive(false);

        _creditsButton.interactable = true;
    }
}