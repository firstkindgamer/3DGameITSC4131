using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menubuttons : MonoBehaviour
{
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
}
