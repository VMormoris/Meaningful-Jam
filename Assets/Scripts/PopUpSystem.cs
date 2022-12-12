using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public string [] Tips;

    private int index;

    public void ShowNextTip()
    {
        PopUp(Tips[index++]);
    }

    private void Start()
    {
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
}
