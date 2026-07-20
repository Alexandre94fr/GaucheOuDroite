using System.Collections.Generic;
using UnityEngine;


public class ResponseProperties
{
    public enum ResponseRemainingTimeResult
    {
        Perfect,
        VeryGood,
        Good,
        Bad,
        CloseCall
    }

    public static ResponseRemainingTimeResult GetResponseRemainingTimeResult(float p_remainingTimeRatio)
    {
        if (p_remainingTimeRatio >= 1)
            return ResponseRemainingTimeResult.Perfect;

        else if (p_remainingTimeRatio >= 0.75)
            return ResponseRemainingTimeResult.VeryGood;

        else if (p_remainingTimeRatio >= 0.5)
            return ResponseRemainingTimeResult.Good;

        else if (p_remainingTimeRatio >= 0.25)
            return ResponseRemainingTimeResult.Bad;

        else
            return ResponseRemainingTimeResult.CloseCall;
    }


    public static readonly Dictionary<ResponseRemainingTimeResult, Color> RESPONSE_REMAINING_TIME_RESULT_COLORS = new()
    {
        [ResponseRemainingTimeResult.Perfect]   = new(0.00f, 0.50f, 1.00f),
        [ResponseRemainingTimeResult.VeryGood]  = new(0.00f, 0.75f, 0.00f),
        [ResponseRemainingTimeResult.Good]      = new(1.00f, 1.00f, 0.00f),
        [ResponseRemainingTimeResult.Bad]       = new(1.00f, 0.50f, 0.00f),

        [ResponseRemainingTimeResult.CloseCall] = new(1.00f, 0.00f, 0.00f),
    };

    public static readonly Dictionary<ResponseRemainingTimeResult, Vector2> RESPONSE_REMAINING_TIME_RESULT_SCALE_SIZES = new()
    {
        [ResponseRemainingTimeResult.Perfect]   = new(1.00f, 1.00f),
        [ResponseRemainingTimeResult.VeryGood]  = new(0.90f, 0.90f),
        [ResponseRemainingTimeResult.Good]      = new(0.80f, 0.80f),
        [ResponseRemainingTimeResult.Bad]       = new(0.70f, 0.70f),

        [ResponseRemainingTimeResult.CloseCall] = new(0.60f, 0.60f),
    };

    // -- Localization -- //

    public static readonly Dictionary<ResponseRemainingTimeResult, string> RESPONSE_REMAINING_TIME_RESULT_NAMES_IN_FRENCH = new()
    {
        [ResponseRemainingTimeResult.Perfect]   = "PARFAIT",
        [ResponseRemainingTimeResult.VeryGood]  = "Très bien",
        [ResponseRemainingTimeResult.Good]      = "Bien",
        [ResponseRemainingTimeResult.Bad]       = "Bof",

        [ResponseRemainingTimeResult.CloseCall] = "Juste",
    };

    public static readonly Dictionary<ResponseRemainingTimeResult, string> RESPONSE_REMAINING_TIME_RESULT_NAMES_IN_ENGLISH = new()
    {
        [ResponseRemainingTimeResult.Perfect]   = "PERFECT",
        [ResponseRemainingTimeResult.VeryGood]  = "Very good",
        [ResponseRemainingTimeResult.Good]      = "Good",
        [ResponseRemainingTimeResult.Bad]       = "Bad",

        [ResponseRemainingTimeResult.CloseCall] = "Close call",
    };
}