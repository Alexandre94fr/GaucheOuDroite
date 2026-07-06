public readonly struct ResponseResult
{
    public readonly int AddedScore;

    public readonly float RemainingTimeRatio;
    public readonly ResponseProperties.ResponseRemainingTimeResult Result;

    public ResponseResult(
        int p_addedScore,
        float p_responseRemainingTimeRatio,

        ResponseProperties.ResponseRemainingTimeResult p_responseRemainingTimeResult)
    {
        AddedScore = p_addedScore;

        RemainingTimeRatio = p_responseRemainingTimeRatio;
        Result = p_responseRemainingTimeResult;
    }
}