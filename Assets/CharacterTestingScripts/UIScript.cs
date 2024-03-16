using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Image healthBar;
    private RectTransform healthPos;
    private Vector3 originalHealth;
    // Start is called before the first frame update
    void Start()
    {
        healthPos = healthBar.transform.GetComponent<RectTransform>();
        originalHealth = healthBar.transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        print(GlobalScript.health);
        healthBar.transform.localScale = new Vector3( (GlobalScript.health / 100), 1, 1);
    }
}
