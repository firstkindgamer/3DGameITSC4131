using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Skill;
using static Tower;

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

        SkillLevels = new int[6];
        SkillCaps = new [] {1, 5, 5, 2, 10, 10};

        SkillNames = new[] {"Fire Rate Increase", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Booster 5", "Booster 6"};
        SkillDescriptions = new[]
        {
            "Increases tower fire rate",
            "Does a cooler thing",
            "Does a underwhelming thing",
            "Does your mom",
            "Is the meta talent",
            "Corrupts your game",
        };

        foreach (var skill in SkillHolder.GetComponentsInChildren<Skill>()) SkillList.Add(skill);
        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>()) ConnectorList.Add(connector.gameObject);

        for (var i = 0; i < SkillList.Count; i++) SkillList[i].id = i;
        
        SkillList[0].ConnectedSkills = new[] {1, 2, 3};
        SkillList[3].ConnectedSkills = new[] {4, 5};

        for(var i = 0; i < 6; i++) SkillLevels[i] = 0;

        UpdateAllSkillUi();
    }

    public void UpdateAllSkillUi()
    {
        foreach (var skill in SkillList) skill.UpdateUI();
    }

    public void UpdatePlayerStats(int talentID)
    {
        switch(talentID)
        {
            case 0:
            tower.attackRate -= .5f;
            break;
        }
    }
}
