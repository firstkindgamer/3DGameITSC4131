using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Skill;

using static Tower;
using static GattlingTower;
using static BrickTower;
using static FreezeTower;
using static Projectile;
using UnityEditor.UI;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;

    public int[] SkillLevels;
    public int[] SkillCaps;
    public string[] SkillNames;
    public string[] SkillDescriptions;

    public List<Skill> SkillList;
    public GameObject SkillHolder;

    public List<GameObject> ConnectorList;
    public GameObject ConnectorHolder;
    public int SkillPoints;

    public void Start()
    {
        SkillPoints = 20;

        SkillLevels = new int[9];
        SkillCaps = new [] {3, 3, 3, 2, 2, 3, 3, 1, 2};

        SkillNames = new[] 
        {"Unlock Towers", 
        "More Bricks", 
        "Ice Cream Truck", 
        "Ol' Reliable", 
        "BOING!", 
        "Strong Arm",
        "Drum Solo",
        "We've Been Waiting for This",
        "Temp Talent"};

        SkillDescriptions = new[]
        {
            "Unlock a new tower to play with",
            "Increase Brick Tower Fire Rate",
            "Increase Freeze Tower Fire Rate",
            "Increase Basic Tower Fire Rate",
            "Increases maximum bounce targets",
            "Brick tower does more damage, flies faster",
            "Increases ALL tower damage",
            "Gattling bullets bounce to multiple targets",
            "Increases Fire Rate"
        };

        foreach (var skill in SkillHolder.GetComponentsInChildren<Skill>()) SkillList.Add(skill);
        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>()) ConnectorList.Add(connector.gameObject);

        for (var i = 0; i < SkillList.Count; i++) SkillList[i].id = i;
        
        //SkillList[4].ConnectedSkills = new[] {7};
        //SkillList[3].ConnectedSkills = new[] {4, 5};

        for(var i = 0; i < 9; i++) SkillLevels[i] = 0;

        UpdateAllSkillUi();
    }

    public void UpdateAllSkillUi()
    {
        foreach (var skill in SkillList) skill.UpdateUI();
    }

    public void UpdatePlayerStats(int talentID)
    {
        switch(talentID) //update stats based on talent
        {
            case 0:
            //temp
            break;
            case 1:
            bTower.towerScriptableObject.attackRate *= .9f; //this doesn't scale correctly
            break;
            case 2:
            fTower.towerScriptableObject.attackRate *= .9f; //none of these scale correctly LMAO
            break;
            case 3:
            tower.towerScriptableObject.attackRate *= .9f;
            break;
            case 4:
            projectile.cleaveNumber++;
            break;
            case 5:
            bTower.towerScriptableObject.projectileDamage *= 1.1f;
            bTower.towerScriptableObject.projectileSpeed *= 1.2f;
            break;
            case 6:
            bTower.towerScriptableObject.projectileDamage *= 1.03f;
            tower.towerScriptableObject.projectileDamage *= 1.03f;
            fTower.towerScriptableObject.projectileDamage *= 1.03f;
            gTower.towerScriptableObject.projectileDamage *= 1.03f;
            break;
            case 7:
            gTower.towerScriptableObject.bulletCleave = true;
            break;
            case 8:
            bTower.towerScriptableObject.attackRate *= .97f;
            tower.towerScriptableObject.attackRate *= .97f;
            fTower.towerScriptableObject.attackRate *= .97f;
            gTower.towerScriptableObject.attackRate *= .97f;
            break;
        }
    }
}