using System;

using FrontEnd.Data.Game;


public class EventHandler
{
    // DOCUMENTATION:

    // In here we store locally all the Actions (Events) we will need for this project.

    // The reason why we don't have a cleaner Actions handling system,
    // is because we have little time to make this project and because
    // we don't have that many Event in our project.

    // -- Input events -- //

    public static Action<DirectionProperties.Direction> OnDirectionChoiceInputEvent;

    public static Action OnPauseInputEvent;

    // -- Levels events -- //

    public static Action<float> OnMaximumRemainingResponseTimeChangedEvent;
    public static Action<float> OnRemainingResponseTimeChangedEvent;

    public static Action<DirectionProperties.Direction> OnNextLevelDirectionComputedEvent;

    public static Action<int> OnScoreChangedEvent;


    public static Action<Level, int> OnLevelWonEvent;
    public static Action<Level, int> OnLevelLostEvent;
}