[System.Serializable]
public class Penalty
{
    public string penaltyDescription;
    public string penaltyText;
    public PenaltyType penaltyType;
    public int penaltyValue;
    public int playerLastLocation;
    public int playerLastScore;

    public enum PenaltyType
    {
        GoBackToStart,
        SkipTurns,
        LoseStar,
        GainStars,
        QuestionToFriend,
        MoveBackSpaces,
        MoveForwardSpaces,
        GoToJail,
        JailBreakCard,
        CancelPenaltyCard,
        JudgeFriends,
        StayPut
    }

}
