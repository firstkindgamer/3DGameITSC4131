using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public GameObject UI;
    void Start()
    {
        UI.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            userToggle();
        }
    }
    
    public void userToggle()
    {
        if (UI.activeSelf == false)
        {
            UI.SetActive(true);
        } else UI.SetActive(false);
    }
}
