using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int scoreNum;
    Text score;

    void Update()
    {
        scoreNum++;
        score.text = "Score: " + scoreNum;
    }

}
