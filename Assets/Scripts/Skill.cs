using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;

public class Skill : MonoBehaviour
{
    public int id;

    public Text TitleText;
    public Text DescriptionText;

    public int[] ConnectedSkills;

    public void UpdateUI()
    {
        TitleText.text = $"{skillTree.SkillLevels[id]}/{skillTree.SkillCaps[id]}\n{skillTree.SkillNames[id]}";
        DescriptionText.text = $"{skillTree.SkillDescriptions[id]}"; 

        //changes color based on talent progress
        //doesnt work
        GetComponent<Image>().color = skillTree.SkillLevels[id] >= skillTree.SkillCaps[id] ? Color.yellow 
        : skillTree.SkillPoints > 0 ? Color.green : Color.white;

        //Are you ready to read this shit storm?
        skillTree.SkillList[1].gameObject.SetActive(skillTree.spentSkillPoints >= 3);
        skillTree.SkillList[2].gameObject.SetActive(skillTree.spentSkillPoints >= 3);
        skillTree.SkillList[3].gameObject.SetActive(skillTree.spentSkillPoints >= 3);

        skillTree.SkillList[4].gameObject.SetActive(skillTree.spentSkillPoints >= 6);
        skillTree.SkillList[5].gameObject.SetActive(skillTree.spentSkillPoints >= 6);

        skillTree.SkillList[6].gameObject.SetActive(skillTree.spentSkillPoints >= 9);

        skillTree.SkillList[7].gameObject.SetActive(skillTree.spentSkillPoints >= 12);
        skillTree.SkillList[8].gameObject.SetActive(skillTree.spentSkillPoints >= 12);


        // foreach (var ConnectedSkill in ConnectedSkills)
        // {
        //     //Reveal choices if required node was bought
        //     skillTree.SkillList[ConnectedSkill].gameObject.SetActive(skillTree.SkillLevels[id] >= skillTree.SkillCaps[id]);
        //     skillTree.ConnectorList[ConnectedSkill].SetActive(skillTree.SkillLevels[id] >= skillTree.SkillCaps[id]);
        // }
    }

    public void Buy()
    {
        if (skillTree.SkillPoints < 1 || skillTree.SkillLevels[id] >= skillTree.SkillCaps[id])
        {
            FindObjectOfType<AudioManager>().Play("talentFail");
            return; //If at skill cap or dont have points
        } else
        {
            FindObjectOfType<AudioManager>().Play("talentBuy");
            skillTree.SkillPoints -= 1;
            skillTree.spentSkillPoints++;
            skillTree.SkillLevels[id]++;
            skillTree.UpdateAllSkillUi();
            skillTree.UpdatePlayerStats(id);
        }
        
    }
}