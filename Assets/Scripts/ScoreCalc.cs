using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreCalc : MonoBehaviour
{
    private void Start()
    {
        DisplayValues();
    }

    private void DisplayValues()
    {
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", GameContext.CalcHighScore());//Total Score
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", GameContext.CalcMovePoints());//Moves Score
        transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = string.Format("({0})", GameContext.sMoves);//Moves
        transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", GameContext.CalcItemsPoints());//Collectables Score
        transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = string.Format("({0})", GameContext.sItems);//Collectables
        transform.GetChild(11).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", GameContext.CalcBonusPoints());//Bonus Score
        transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = string.Format("({0})", GameContext.sCracked);//Bonus Tiles
        transform.GetChild(14).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", GameContext.CalcDeathPoints());//Death Score
        transform.GetChild(15).GetComponent<TextMeshProUGUI>().text = string.Format("({0})", GameContext.sDeaths);//Deaths
    }

    public void Restart()
    {
        GameContext.Reset(true);
        SceneManager.LoadScene("NewStagesScenedup");
    }

    public void GoToMenu()
    {
        GameContext.Reset(true);
        SceneManager.LoadScene("Menuscene");
    }

}
