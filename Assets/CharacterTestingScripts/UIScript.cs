using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIScript : MonoBehaviour
{

    public Image healthBar;
    private GameObject tower;
    private RectTransform healthPos;
    private Vector3 originalHealth;
    public Image swordPixel;
    public Image brickPixel;
    public GameObject clockTower;
    public GameObject freezeTower;
    public GameObject brickTower;
    public GameObject gattlingTower;
    public Image clockTowerImage;
    public Image freezeTowerImage;
    public Image brickTowerImage;
    public Image gattlingTowerImage;
    public GameObject towerHolderUi;
    public TowerPlacer placer;

    // Start is called before the first frame update
    void Start()
    {
        //healthPos = healthBar.transform.GetComponent<RectTransform>();
        //originalHealth = healthBar.transform.localScale;
        clockTowerImage.GetComponent<Outline>().enabled = true;


    }
    
    void rangedMode()
    {

    }

    void swordmode() { 
    
    
    }

    
    

    public void Update()
    {
        print(GlobalScript.health);
        //healthBar.transform.localScale = new Vector3((GlobalScript.health / 100), 1, 1);

        if (Input.GetKeyDown("1"))
        {
            placer.towerToPlace = clockTower;
            clockTowerImage.GetComponent<Outline>().enabled = true;
            freezeTowerImage.GetComponent<Outline>().enabled = false;
            gattlingTowerImage.GetComponent<Outline>().enabled = false;
            brickTowerImage.GetComponent<Outline>().enabled = false;


        } else if (Input.GetKeyDown("2")) {
            placer.towerToPlace = freezeTower;
            freezeTowerImage.GetComponent<Outline>().enabled = true;
            clockTowerImage.GetComponent <Outline>().enabled = false;
            gattlingTowerImage.GetComponent<Outline>().enabled = false;
            brickTowerImage.GetComponent<Outline>().enabled = false;

        } else if (Input.GetKeyDown("3"))
        {
            placer.towerToPlace = brickTower;
            brickTowerImage.GetComponent<Outline>().enabled = true;
            freezeTowerImage.GetComponent<Outline>().enabled = false;
            clockTowerImage.GetComponent<Outline>().enabled = false;
            gattlingTowerImage.GetComponent<Outline>().enabled = false;
            

        } else if(Input.GetKeyDown("4"))
        {
            placer.towerToPlace = gattlingTower;
            gattlingTowerImage.GetComponent<Outline>().enabled = true;
            freezeTowerImage.GetComponent<Outline>().enabled = false;
            clockTowerImage.GetComponent<Outline>().enabled = false;
            brickTowerImage.GetComponent<Outline>().enabled = false;

        }

    }
}
