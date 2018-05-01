using UnityEngine;
using UnityEngine.UI;

public class RemainingSkillText : MonoBehaviour
{
 
    private Text remainingSkillsText;
    private int totalSkills;
    int maxPoints;
    int totalPoints;
	public static RemainingSkillText instance;

    void Awake(){

		if (instance == null) {
			instance = this;

		} else {
			Destroy (this);
		}

        remainingSkillsText = GetComponent<Text>();
		UpdateScoreText ();
       
    }

	public void UpdateScoreText(){

		int myTotalCost = PlayerPrefs.GetInt("CostSkillPoints", 0);       
		maxPoints = ScoreManager.ConvertToSkills();
		totalPoints = maxPoints - myTotalCost;
		UpdateText(totalPoints);
	}

	private int GetTotalPoints() {
        return totalPoints;
    }
  
	private void UpdateText(int total){
        remainingSkillsText.text =  total.ToString();
    }

	private int GetMaxPoints(){
        return maxPoints;
    }

   
   
  
}