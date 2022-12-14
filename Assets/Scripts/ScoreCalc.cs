using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalc : MonoBehaviour
{
    private void Start()
    {
        GameContext.sCalculator = this;
        Debug.Log("Started");
    }
    public void Finished()
    {
        Debug.Log("Game finished");
    }


}
