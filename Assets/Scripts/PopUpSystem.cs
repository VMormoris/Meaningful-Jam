using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Transform Player;
    public Animator animator;
    public string [] Tips;

    private int index;

    public void ShowNextTip()
    {
        Player.GetComponent<PlayerMovement>().enabled = false;
        PopUp(Tips[index++]);
    }

    private void Start()
    {
        Debug.Log("Started");
        index = 0;
        ShowNextTip();
        
    }
    public void PopUp(string text)
    {
        popUpBox.SetActive(true);
        GetComponent<TextMeshProUGUI>().text = text;
        animator.SetTrigger("pop");
        animator.ResetTrigger("close");
    }
    public void ResetState()
    {
        Player.GetComponent<PlayerMovement>().enabled = true;
    }
}
