using UnityEngine;


public static class GameRulesProperties
{
    // ------------ //
    // -- FRENCH -- //
    // ------------ //

    public static string TITLE_TEXT_IN_FRENCH                   = "Règles du jeu :";

    public static string GAME_GOAL_TITLE_TEXT_IN_FRENCH         = "But du jeu :";
    public static string GAME_GOAL_TEXT_IN_FRENCH               = 
        "L'objectif du jeu est d'appuyer sur une des touches correspondantes à la direction affichée en haut de l'écran durant un niveau, ici, Gauche ou Droite.\n" +
        "Si le jeu vous dit Gauche, alors vous devez appuyer sur une des touches liées à l'action Gauche (plus de détail dans la section Raccourcis).";

    public static string SCORING_SYSTEM_TITLE_TEXT_IN_FRENCH    = "Système de score :";
    public static string SCORING_SYSTEM_TEXT_IN_FRENCH          = 
        $"Afin de débloquer le niveau suivant, il faut terminer le niveau en cours.\n\n" +

        $"Plus vous répondez rapidement, plus votre score sera élevé.\n" +
        $"La zone bleue dans la barre de temps au haut de l'écran durant un niveau, représente la période dans laquelle vous pouvez obtenir le score maximal pour cette question.\n" +
        $"La zone grise, elle, représente la période durant laquelle votre score baisse progressivement.\n\n" +
        
        $"Afin d'obtenir 3 étoiles sur tous les niveaux (sauf le dernier, où il faut juste dépasser un score précis), il vous faut atteindre le score maximal du niveau.\n" +
        $"Il est uniquement atteignable si vous effectuez uniquement des réponses \"<b><color=#{ColorUtility.ToHtmlStringRGBA(ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_COLORS[ResponseProperties.ResponseRemainingTimeResult.Perfect])}>{ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_NAMES_IN_FRENCH[ResponseProperties.ResponseRemainingTimeResult.Perfect]}</color></b>\".";

    public static string CLOSE_BUTTON_TEXT_IN_FRENCH            = "Compris !";

    // ------------- //
    // -- ENGLISH -- //
    // ------------- //

    public static string TITLE_TEXT_IN_ENGLISH                  = "Game rules:";

    public static string GAME_GOAL_TITLE_TEXT_IN_ENGLISH        = "Game goal:";
    public static string GAME_GOAL_TEXT_IN_ENGLISH              = 
        "The goal of the game is to press one of the keys corresponding to the direction shown at the top of the screen during a level, in this case, Left or Right.\n" +
        "If the game tells you to go Left, then you must press one of the keys associated with the Left action (see the Shortcuts section for more details).";

    public static string SCORING_SYSTEM_TITLE_TEXT_IN_ENGLISH   = "Scoring system:";
    public static string SCORING_SYSTEM_TEXT_IN_ENGLISH         =
        $"To unlock the next level, you must complete the current level.\n\n" +

        $"The faster you answer, the higher your score will be.\n" +
        $"The blue section of the timer at the top of the screen during a level represents the time frame in which you can achieve the maximum score for that question.\n" +
        $"The gray section represents the time frame during which your score gradually decreases.\n\n" +

        $"To earn 3 stars on all levels (except the last one, where you just need to exceed a specific score), you must achieve the level's maximum score.\n" +
        $"This can only be achieved if you give only \"<b><color=#{ColorUtility.ToHtmlStringRGBA(ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_COLORS[ResponseProperties.ResponseRemainingTimeResult.Perfect])}>{ResponseProperties.RESPONSE_REMAINING_TIME_RESULT_NAMES_IN_ENGLISH[ResponseProperties.ResponseRemainingTimeResult.Perfect]}</color></b>\" answers.";

    public static string CLOSE_BUTTON_TEXT_IN_ENGLISH = "Understood!";
}