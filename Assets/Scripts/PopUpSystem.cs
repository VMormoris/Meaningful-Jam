using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public GameObject Nextbtn;
    public GameObject Player;
    public Animator animator;
    public string [] Tips;

    private int index;

    public void ShowNextTip()
    {
        PopUp(Tips[index++]);
        if (index >= Tips.Length)
            Nextbtn.SetActive(false);
    }

    private void Start()
    {
        index = 0;
        ShowNextTip();
        Player.GetComponent<TestScript>().enabled = false;
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
        Player.GetComponent<TestScript>().enabled = true;
    }
}
