namespace Contracts
{
    public static class Constants
    {
        public const string MatchAlreadyInProgress = "Match already in progress, please complete this before starting a new match";
        public const string NoMatchInProgress = "There is no match in progress, please start a new match first";
        public const string GameInProgress = "There is already game in progress, please complete this first";
        public const string NoGameProgress = "There is no game in progress, please request a move first";
        public const string InvalidParameters = "Invalid parameters";
        public const string InvalidNumberOfDynamite = "You cannot have more dynamite than the number of games";
        public const string GameSuccessfullySetup = "Game successfully setup";

        public const string TooManyDynamite = "You can't use dynamite again as you have reached the maximum number of {0} already";

        public const string NoGamesLeftToPlay = "There are no games left to play, please complete the match";
        public const string InvalidOutcome = "Outcome does not match with moves made";
        
        public const string OverallWin = "Woohoo!! I won, better luck next time";
        public const string OverallDraw = "We drew, shall we play again?";
        public const string OverallLose = "Congratulation, you won this time, but I will win our next match :)";

        public const string NotAllGamesCompleted = "Cannot save overall result, not all games have been completed";
    }
}
