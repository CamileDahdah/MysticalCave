using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{


    private enum TypeOfSkills
    {
        health,//cost 2-3-3-4-4-5-6-6
        echoWave,
        attackWave,
        lightWave,
        bouncyWave,
        vampireBat,
        doubleTapLight,
        count
    }

    public Text totalPointsText;
    public Transform skillPanel;
    public GameObject[] skill;
    int[] additionalWaves = new int[4];
    private int totalSkills;
    int maxPoints;
    int totalPoints;
    List<SkillComponents> components = new List<SkillComponents>();
    List<GameObject> skillsObjects = new List<GameObject>();

    void Awake(){
		
   //     PlayerPrefs.SetInt("totalpoints", 0);
        int mytotalcost = 0;
        totalSkills = skill.Length;
        maxPoints = ScoreManager.ConvertToSkills();

        for (int i = 0; i < totalSkills; i++){
			
            GameObject skills = (GameObject) Instantiate (skill[i]);
            skills.transform.SetParent(skillPanel);
            skillsObjects.Add(skills);
            components.Add(skills.GetComponent<SkillComponents>());
            components[i].SetID(i);

            components[i].SetUpgrade(PlayerPrefs.GetInt("skill" + (i + 1), 0));
            components[i].transform.localScale = new Vector3(1.55f, 1.45f, 1);
            mytotalcost += components[i].Cost(components[i].GetUpgrade());

        }
        totalPoints = maxPoints - mytotalcost;
        UpdateText(totalPoints);


    }

    void ResetAll(){


        UpdateText(maxPoints);
        totalPoints = maxPoints;
        for (int i = 0; i < totalSkills; i++)
        {
            GameObject skills = (GameObject)Instantiate(skill[i]);
            skills.transform.SetParent(skillPanel);
            skillsObjects.Add(skills);
            components.Add(skills.GetComponent<SkillComponents>());
            components[i].SetID(i);
            //components[i].setSkill(0);
            components[i].SetUpgrade(0);
            components[i].transform.localScale = new Vector3(1.55f, 1.45f, 1); ;

        }

    }

    void Update(){

        // totalPoints = 0;
        for (int i = 0; i < totalSkills; i++){
			
            GameObject skills = skillsObjects[i];
            GameObject plusButton = skills.transform.GetChild(1).gameObject;
            GameObject minusButton = skills.transform.GetChild(2).gameObject;
            int upgrade = components[i].GetUpgrade();

			if (upgrade == 0) {
				minusButton.SetActive (false);
			} else {
				minusButton.SetActive (true);
			}

            int cost = components[i].CostUpgrade(components[i].GetUpgrade());

			if (cost > totalPoints || upgrade >= components [0].GetMaxUpgrade ()) {
				plusButton.SetActive (false);
			} else {
				plusButton.SetActive (true);
			}

        }

        SaveUpgradesValues();
    }

    public int GetTotalPoints(){
        return totalPoints;
    }

	public void SetTotalPoints(int points){
        totalPoints = points;
    }

    public void UpdateText(int total){
        totalPointsText.text = total.ToString();
    }

    public int GetMaxPoints(){
        return maxPoints;
    }

    public void Reset(){
		
        for (int i = 0; i < totalSkills; i++)
            Destroy(skillsObjects[i]);

        components = new List<SkillComponents>();
        skillsObjects = new List<GameObject>();

        ResetAll();
    }

    void SaveUpgradesValues(){

        for (int i = 0; i < totalSkills; i++)
        {
            TypeOfSkills name = (TypeOfSkills)i;
            PlayerPrefs.SetInt(name + "", components[i].GetUpgrade());

        }
    }

    void OnDisable(){

        for (int i = 0; i < totalSkills; i++)
        {
            PlayerPrefs.SetInt("skill" + (i + 1), components[i].GetUpgrade());
			Debug.Log ("skill" + (i + 1) + components [i].GetUpgrade ());

        }

        for (int i = 1; i < 4; i++)
        {
            additionalWaves[(i - 1)] = SkillComponents.GetDescriptionValue(i, components[i].GetUpgrade());

        }

        PlayerPrefs.SetInt("percentagehealth", SkillComponents.GetDescriptionValue(0, components[0].GetUpgrade()));
        PlayerPrefs.SetInt("plusnormalwave", additionalWaves[0]);
		PlayerPrefs.SetInt("pluslightwave", additionalWaves[1]);
		PlayerPrefs.SetInt("plusbouncewave", additionalWaves[2]);
		//PlayerPrefs.SetInt("plusattackwave", additionalWaves[4]);
        PlayerPrefs.SetInt("CostSkillPoints", maxPoints - totalPoints);
		Debug.Log (maxPoints - totalPoints);
		PlayerPrefs.Save ();
		RemainingSkillText.instance.UpdateScoreText ();
    }

}