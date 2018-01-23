using UnityEngine;
using System.Collections.Generic;


public class LevelManager : MonoBehaviour
{
    [HideInInspector]
    public static int levelID;
    static int minimumTime;
    static int numEnemies;
    static int minimumScore;
    static int numSecretPath;
    static int offsetDamage;
    private static int numSkills = 6;
    private int[] upgrades;
	[HideInInspector]
	public static int gollumDamageHit = 20, miniBossDamageHit = 25;
	public static LevelManager instance;
	List<int> numActiveRoomList;
	public List<int> activatedRoomList;
	Dictionary<int, GameObject> roomTriggers;

    private enum TypeOfSkills
    {
        health,		//cost 2-3-3-4-4-5-6-6
        echoWave,	//cost
        attackWave,
        lightWave,
        bouncyWave,
        vampireBat,
        doubleTapLight,
        count
    }

	void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy (instance.gameObject);
		activatedRoomList = new List<int> ();

	}

    void Start(){

        upgrades = new int[numSkills];

    }


	public void InitializeLevel(int id, int time, int enemies, int score, int path, int offset){
        levelID = id;
        minimumTime = time;
        numEnemies = enemies;
        minimumScore = score;
        numSecretPath = path;
        offsetDamage = offset;

    }

	public void SetActiveRoomList(List<int> numSecretRooms){
	
		roomTriggers = new Dictionary<int, GameObject>();
		GameObject[] roomTriggersGO = GameObject.FindGameObjectsWithTag ("SecretRoomTrigger");		//Getting all room triggers and inserting them in Dictionary
		for (int i = 0; i < roomTriggersGO.Length; i++) {
			roomTriggersGO [i].GetComponent<SwitchToSecretCamera> ().roomID = i + 1;		//Set Room ID
			roomTriggers.Add (i + 1, roomTriggersGO[i]);				//Rooms Add to Dictionary with key
		}

		foreach (KeyValuePair<int, GameObject> room in roomTriggers) {		//Turn every room trigger OFF
			room.Value.SetActive(false);
			room.Value.GetComponent<SwitchToSecretCamera> ().AutoOpenDoor ();
		}

		if (numSecretRooms.Count > 0) {
			numActiveRoomList = numSecretRooms;			//Turn unvisited room triggers ON
			foreach (int numRoom in numActiveRoomList) {
				roomTriggers [numRoom].SetActive (true);

			}
		}
	}

	public void SetActivatedRoomList(int roomID){
		activatedRoomList.Add (roomID);
	}
	public void ClearActivatedRoomList(){
		activatedRoomList.Clear();
	}
    public int GetLevelId(){
        return levelID;
    }

    public static int GetNumSecretPath(){
        return numSecretPath;
    }

    public static int GetMinimumTime(){
        return minimumTime;
    }

    public static int GetMinimumScore(){
        return minimumScore;
    }

    public static int GetOffset(){
        return offsetDamage;
    }

    public static int GetNumEnemies(){
        return numEnemies;
    }
		
    public void SetLevelId(int level){
        levelID = level;
    }

    void ConvertSkillsValues()
    {
        for (int i = 0; i < 6; i++){

            TypeOfSkills name = (TypeOfSkills)i;
            upgrades[i] = PlayerPrefs.GetInt(name + "");

        }
    }
}
