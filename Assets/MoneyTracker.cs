using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoneyTracker : MonoBehaviour
{
    public Text textComponent;
    public GameObject player;

    public int myIntValue;
    void Start()
    {
        textComponent.text = "$" + myIntValue;
    }
    void Update() //this is inefficient but we're running out of time
    {
        myIntValue = player.GetComponent<PlayerController>().money;
    }
    

    void Awake () {
        //If text hasn't been assigned, disable ourselves
        if (textComponent == null)
        {
            Debug.Log("You must assign a text component!");
            this.enabled = false;
            return;
        }
        UpdateText(myIntValue);
    }

    void UpdateText (int value) {
        //Update the text shown in the text component by setting the `text` variable
        textComponent.text = "$" + value;
    }
}
