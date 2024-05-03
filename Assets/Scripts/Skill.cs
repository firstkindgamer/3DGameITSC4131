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

        foreach (var ConnectedSkill in ConnectedSkills)
        {
            skillTree.SkillList[ConnectedSkill].gameObject.SetActive(skillTree.SkillLevels[id] >= skillTree.SkillCaps[id]);
            skillTree.ConnectorList[ConnectedSkill].SetActive(skillTree.SkillLevels[id] >= skillTree.SkillCaps[id]);
        }
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
            skillTree.SkillLevels[id]++;
            skillTree.UpdateAllSkillUi();
            skillTree.UpdatePlayerStats(id);
        }
        
    }
}
