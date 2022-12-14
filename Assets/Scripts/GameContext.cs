using System.Collections;
using UnityEngine;


    public static class GameContext
    {
    public static bool GameIsPaused = false;
    public static int sDeaths = 0;
    public static int sItems = 0;
    public static int sMoves = 0;
    public static int sCracked = 0;
    public static ScoreCalc sCalculator = null;

    

    public static int CalcHighScore(int mItems, int mMoves, int mCracked, int mDeaths)
    {

        //46 with items 
        //34 without items
        const int OptimalMoves = 10;
        int score = OptimalMoves*1000;
        if (mCracked >= 62)
            score = 5000;
        score += mItems * 1000;
        score -= mDeaths * 500;
        score -= (mMoves - OptimalMoves) * 1000;
        return score;
    }
}
