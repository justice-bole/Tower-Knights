using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textToDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject dialogBox;
    public bool isDialogPresent = true;


    private void Start()
    {
        StartCoroutine(Type());
    }

    private void Update()
    {
        if(textToDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }
    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            textToDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);    
        if(index < sentences.Length - 1)
        {
            index++;
            textToDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textToDisplay.text = "";
            continueButton.SetActive(false);
            dialogBox.SetActive(false);
            isDialogPresent = false;
        }
    }
}
