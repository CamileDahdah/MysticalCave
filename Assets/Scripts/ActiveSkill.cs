using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour{
	
    int maxSkill;
	public Image filledImage;

    void Awake(){

		maxSkill = SkillComponents.MAX_UPGRADES;
    }


	public void SetActiveButton(int skillNum){
		filledImage.fillAmount = (float) (skillNum) / SkillComponents.MAX_UPGRADES;

	}

}
