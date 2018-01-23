using UnityEngine;
using UnityEngine.UI;

public class RemainingSkillText : MonoBehaviour
{
 
    private Text remainingSkillsText;
    private int totalSkills;
    int maxPoints;
    int totalPoints;

    void Awake()
    {
        remainingSkillsText = GetComponent<Text>();
        int myTotalCost = PlayerPrefs.GetInt("CostSkillPoints", 0);       
        maxPoints = ScoreManager.ConvertToSkills();
        totalPoints = maxPoints - myTotalCost;
        UpdateText(totalPoints);
    }

    public int GetTotalPoints()
    {
        return totalPoints;
    }
  
    public void UpdateText(int total)
    {
        remainingSkillsText.text =  total.ToString();
    }

    public int GetMaxPoints()
    {
        return maxPoints;
    }

   
   
  
}