using System.Collections;
using UnityEngine;


public static class GameContext
{
    public static bool GameIsPaused = false;
    public static bool IsGameMuted = false;
    public static int sDeaths = 0;
    public static int sItems = 0;
    public static int sMoves = 0;
    public static int sCracked = 0;
    public static SoundManagerScript sSoundManager = null;

    public static int CalcHighScore()
    {
        int score = CalcMovePoints();
        score += CalcBonusPoints();
        score += CalcItemsPoints();
        score += CalcDeathPoints();
        return score;
    }

    public static int CalcMovePoints()
    {
        //46 with items 
        //34 without items
        const int OptimalMoves = 46;
        const int multiplier = 1000;
        int points = OptimalMoves * multiplier;
        if(sMoves > OptimalMoves)
            points -= (sMoves - OptimalMoves) * multiplier;
        return points;
    }

    public static void Reset(bool deaths = false)
    {
        sCracked = 0;
        sMoves = 0;
        sItems = 0;
        if(deaths)
            sDeaths = 0;
    }

    public static int CalcItemsPoints() { return sItems * 1000; }

    public static int CalcDeathPoints() { return -sDeaths * 500; }

    public static int CalcBonusPoints() { return sCracked >= 63 ? 5000 : 0; }
}
