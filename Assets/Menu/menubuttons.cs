using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menubuttons : MonoBehaviour
{

    public Image titleBG;
    public Image title;
    public Image bg;
    public Image buttonBG;
    public GameObject startButton;
    public GameObject stopButton;
    public GameObject optionButton;
    public GameObject optioncontrol;
    public Image control1;
    public Image control2;
    public Image control3;
    public Image control4;
    public void startGame()
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void endGame()
    {
        Application.Quit();
    }
    public void optionMenu()
    {
        titleBG.enabled = false;
        buttonBG.enabled = false;
        title.enabled = false;
        startButton.SetActive(false);
        stopButton.SetActive(false);
        optionButton.SetActive(false);
        
        optioncontrol.SetActive(true);
        control1.enabled = true;
        control2.enabled = true;
        control3.enabled = true;
        control4.enabled = true;
    }
    public void exitOptionMenu()
    {
        titleBG.enabled = true;
        buttonBG.enabled = true;
        title.enabled = true;
        startButton.SetActive(true);
        stopButton.SetActive(true);
        optionButton.SetActive(true);

        optioncontrol.SetActive(false);
        control1.enabled = false;
        control2.enabled = false;
        control3.enabled = false;
        control4.enabled = false;
    }

    public void Start()
    {
        optioncontrol.SetActive(false);
        control1.enabled = false;
        control2.enabled = false;
        control3.enabled = false;
        control4.enabled = false;
    }
}
