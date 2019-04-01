
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class SkillComponents : MonoBehaviour
{

    private static string connectionString;
	public const int MAX_UPGRADES = 7;
    public ActiveSkill activeSkill;
    int id = -1;
    int skillNum = 0;
    int upgrades = 0;
    int[] costArray = { 0, 2, 5, 8, 12, 16, 21, 27 };
    SkillsManager managerSkill;
	Color[] textColors = new Color[]{Color.red, Color.white,Color.yellow, new Color(0, 155f/255, 1), Color.red};

    void Start(){

        connectionString = GameManager.connectionString;
        managerSkill = transform.parent.transform.parent.GetComponent<SkillsManager>();
    }

    public int GetSkill(){
        return skillNum;
    }
		

    public void SetSkill(int x){
        skillNum = x;
    }

    public void SetUpgrade(int x){
        upgrades = x;
        activeSkill.SetActiveButton(x);
        SetSkill(x);

    }

    public void GetDescription(){

        string description = "";

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery;
				if (upgrades < GetMaxUpgrade ()) {
					sqlQuery = "SELECT * FROM skillsDescription WHERE skill_id = " + id
					+ " AND upgrade_id = " + upgrades;
				} else {
					sqlQuery = "SELECT * FROM skillsDescription WHERE skill_id = " + id
					+ " AND upgrade_id = " + (upgrades - 1);
				}

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read()){
						
                        Debug.Log(reader.GetString(3));
                        description = reader.GetString(3);

                    }
                    Debug.Log("\nCost " + CostUpgrade(upgrades));

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        UpdateDescriptionText.descriptionText.text = description + "\n"
                           + "Cost " + CostUpgrade(upgrades);
		UpdateDescriptionText.descriptionText.color = textColors [id];

    }

    public static int GetDescriptionValue(int id, int upgrade){
		
		if (upgrade == 0) {
			return 0;
		}

        int finalAns = 0;

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM skillsDescription WHERE skill_id = "
                    + id + " AND upgrade_id = " + (upgrade - 1);

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    if (reader.Read()){
						
                        string desc = reader.GetString(3);
                        string num = desc.Split(' ')[0];

						if (num.Contains ("+")) {
							
							finalAns = int.Parse (num.Split ('+') [1]);
							Debug.Log (finalAns);
							reader.Close ();

							for (int i = 1; i < upgrade; i++) {

								string newQuery = "SELECT * FROM skillsDescription WHERE skill_id = "
								                                          + id + " AND upgrade_id = " + (i - 1);
								dbCmd.CommandText = newQuery;
								using (IDataReader reader1 = dbCmd.ExecuteReader ()) {
									while (reader1.Read ()) {
										
										string desc1 = reader1.GetString (3);
										string num1 = desc1.Split (' ') [0];
										finalAns += int.Parse (num1.Split ('+') [1]);
									}

									reader1.Close ();
								}
							}
						} else {
							finalAns = int.Parse (num.Split ('%') [0]);
						}

                    }

                    dbConnection.Close();

					if (!reader.IsClosed) {
						reader.Close ();
					}

                }

            }

        }

        return finalAns;

    }
    

    public int GetUpgrade(){
        return upgrades;
    }

    public void SetID(int idNum){
        id = idNum;

    }

    public int GetMaxUpgrade(){
        return MAX_UPGRADES;
    }

    public int GetID(){
        return id;

    }

    public void IncreaseSkill(){
		
        int cost = CostUpgrade(upgrades);

		if (managerSkill.GetTotalPoints () == 0 || managerSkill.GetTotalPoints () - cost < 0 || upgrades >= MAX_UPGRADES) {
			return ;
		}
       
        skillNum++;
		activeSkill.SetActiveButton(skillNum);

        GetDescription();

        managerSkill.SetTotalPoints(managerSkill.GetTotalPoints() - cost);

        managerSkill.UpdateText(managerSkill.GetTotalPoints());

        //if(upgrades < MAXUPGRADES)
        upgrades++;
    }

    public void DecreaseSkill(){
		
        int cost = CostUpgrade(upgrades - 1);

        if (skillNum == 0 || managerSkill.GetTotalPoints() == managerSkill.GetMaxPoints())
            return;

        skillNum--;
		activeSkill.SetActiveButton(skillNum);


        managerSkill.SetTotalPoints(managerSkill.GetTotalPoints() + cost);
        managerSkill.UpdateText(managerSkill.GetTotalPoints());
        upgrades--;
    }

    public int CostUpgrade(int upgrade){
        if (upgrade == 0)
            return 2;
        else if (upgrade <= 2)
            return 3;
        else if (upgrade <= 4)
            return 4;
        else if (upgrade == 5)
            return 5;
        else
            return 6;

    }

    public int Cost(int upgrade){
		//cost 2-3-3-4-4-5-6-6
        return costArray[upgrade];
    }

}