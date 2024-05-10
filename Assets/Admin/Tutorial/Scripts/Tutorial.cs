using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] tutorialText;
    [SerializeField] private int index;

    private void Start()
    {
        index = 0;
    }

    public void nextPrompt()
    {
        tutorialText[index].alpha = 0;
        index++;
        if (index >= tutorialText.Length)
        {
            index = 0;
        }

        tutorialText[index].alpha = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tutorialText[index].alpha = 1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        tutorialText[index].alpha = 0f;
    }
}
