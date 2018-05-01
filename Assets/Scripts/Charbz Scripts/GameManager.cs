
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{

    private static GameManager instance = null;
    public static int currentLevel;
    public static int normalWave, lightWave, bounceWave, attackWave;
    public Texture2D emptyProgressBar; // Set this in inspector.
    public Texture2D fullProgressBar; // Set this in inspector.
    private AsyncOperation async = null; // When assigned, load is in progress.
    static string mysticalCave = "mysticalCave.sqlite";
    public static int health;
    public static int numLevels=12;
    public LevelManager levelManager;
    public static string connectionString = "";
    static string dbPath = "";
    private string filePath;
    int height = 20;
    int width = 100;
    public static String lastLevel ;
	private List <int> notActiveRoomsList = new List<int>();

    public static GameManager Instance{

        get{
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }

        set { }

    }
    void Awake(){

		Application.targetFrameRate = 60;
        //  PlayerPrefs.SetInt("totalscore", 0);
        OpenDB(mysticalCave);
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        normalWave = lightWave = bounceWave = attackWave = 0;
    }


    static void OpenDB(string databaseName){

        //check if file exists in Application.persistentDataPath

	#if UNITY_EDITOR
        dbPath = string.Format(@"Assets/StreamingAssets/{0}", databaseName);
		//Debug.Log(Application.streamingAssetsPath + "/" + databaseName);


	#elif UNITY_STANDALONE
        dbPath = Application.dataPath + "/StreamingAssets/"+databaseName;
	#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, databaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

	#if UNITY_ANDROID
            var loadDb = new WWW ("jar:file://" + Application.dataPath + "!/assets/" + databaseName);  
            // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't
            //let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);


	#elif UNITY_IOS

		File.Copy(Application.streamingAssetsPath + "/" + databaseName,  filepath);

	#endif

          }
              var dbPath = filepath;
      
	#endif


        connectionString = "URI=file:" + dbPath;



    }

	void OnLoadLevel(Scene scene, LoadSceneMode mode){
       
    
        if (instance != null)
        {
            instance.async = null;
            instance.StopCoroutine(LoadALevel(name));
        }
       
        InitializeWaves();

		levelManager.ClearActivatedRoomList ();
		if (notActiveRoomsList != null) {
			levelManager.SetActiveRoomList (notActiveRoomsList);
		}
//        EnemyAttack.enemyCounter = 0;

    }

	void OnEnable(){
		//Tell our 'OnLoadLevel' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLoadLevel;
	}

	void OnDisable(){
		//Tell our 'OnLoadLevel' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLoadLevel;
	}

    void OnGUI(){

        if (instance !=null && instance.async != null)
        {

            GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height), emptyProgressBar);
            GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width * instance.async.progress, height), fullProgressBar);
        }

    }

    private IEnumerator LoadALevel(string levelName){

        instance.async = SceneManager.LoadSceneAsync(levelName);

        yield return instance.async;

    }

    public void SetLevel(string level){
		
        lastLevel = SceneManager.GetActiveScene().name;

        SaveMenu.SetOpenMenu();
        instance.StartCoroutine(LoadALevel(level));

    }

    public void SetLevel(int level){
		
        lastLevel = SceneManager.GetActiveScene().name;
        SaveMenu.SetOpenMenu();
        currentLevel = level;
        LoadLevel();

    }

	public void SetLevel(){

		lastLevel = SceneManager.GetActiveScene().name;
		SaveMenu.SetOpenMenu();
		LoadLevel();

	}

    public void IncrementLevel(){
		
        lastLevel = SceneManager.GetActiveScene().name;
        currentLevel++;
		GameManager.Instance.SetLevel(currentLevel);
    }

	public void ReloadLevel(){
		SetLevel(currentLevel);
	}

    public void LoadLevel(){
		
		notActiveRoomsList.Clear ();

		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCmd = dbConnection.CreateCommand ()) {
				string sqlQuery = "SELECT * FROM levelDescription WHERE level_id = " + currentLevel;

				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {

						int time = reader.GetInt32 (1);
						int enemies = reader.GetInt32 (2);
						int score = reader.GetInt32 (3);
						int path = reader.GetInt32 (4);
						int offset = reader.GetInt32 (5);
		

						if (levelManager != null) {

							levelManager.InitializeLevel (currentLevel, time, enemies, score, path, offset);

						}
					}
				}
				sqlQuery = "SELECT * FROM secretRooms WHERE level_id = " + currentLevel + " AND visited = 0";

					dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader ()) {
						while (reader.Read ()) {

							int roomNumber = reader.GetInt32 (2);
		
							notActiveRoomsList.Add (roomNumber);
							
						}

	
				
						reader.Close ();
					}
					sqlQuery = "SELECT COUNT(*) FROM secretRooms WHERE visited = 1";
					dbCmd.CommandText = sqlQuery;
					int numDiscoveredRooms = (int) (Int64) dbCmd.ExecuteScalar ();
					Activation.secretRoomCounter = numDiscoveredRooms;
					sqlQuery = "SELECT COUNT(*) FROM secretRooms";
					dbCmd.CommandText = sqlQuery;
					int allRooms = (int) (Int64) dbCmd.ExecuteScalar ();
					Activation.allRooms = 4;
					dbConnection.Close ();
				}
				
				String level = "Level" + currentLevel;

				if (currentLevel < numLevels + 1) {
					instance.StartCoroutine (LoadALevel (level));
				}
				else {

				}
			}

    }
    void InitializeWaves(){

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM numwaves WHERE id = " + (currentLevel - 1)  ;

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                
                    while (reader.Read())
                    {
                        health = 100;
                        health = health + (int)(health * (PlayerPrefs.GetInt("percentagehealth", 0) * 0.01f));


                        normalWave = reader.GetInt32(1) + PlayerPrefs.GetInt("plusnormalwave", 0);
                        attackWave = reader.GetInt32(2) + PlayerPrefs.GetInt("plusattackwave", 0);
                        bounceWave = reader.GetInt32(3) + PlayerPrefs.GetInt("plusbouncewave", 0);
                        lightWave = reader.GetInt32(4) + PlayerPrefs.GetInt("pluslightwave", 0);
                      

                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }

        }
    }



		public int GetWorldScore(){

			int bloodCount = 0;


			using (IDbConnection dbConnection = new SqliteConnection(connectionString))
			{

				dbConnection.Open();

				using (IDbCommand dbCmd = dbConnection.CreateCommand())
				{ 

					string sqlQuery = String.Format("SELECT SUM(blood_score) FROM scoresLevel");

					dbCmd.CommandText = sqlQuery;
					bloodCount = (int) (Int64)dbCmd.ExecuteScalar();
					

				}
				
				dbConnection.Close ();
			}

			return bloodCount;

		}



#if UNITY_EDITOR
    static void UpdateDatabase(){    

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {


            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery;
                for (int i = 1; i <= numLevels; i++)
                {

                    sqlQuery = String.Format("UPDATE scoresLevel SET score = " + 0
                        + " WHERE level_id = " + i);

                    dbCmd.CommandText = sqlQuery;
                    dbCmd.ExecuteScalar();
                }
     
            }

			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				string sqlQuery;
				for (int i = 1; i <= numLevels; i++)
				{

					sqlQuery = String.Format("UPDATE scoresLevel SET blood_score = " + 0
					+ " WHERE level_id = " + i);

					dbCmd.CommandText = sqlQuery;
					dbCmd.ExecuteScalar();
				}

			}

			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				string sqlQuery;
				for (int i = 1; i <= numLevels; i++)
				{

					sqlQuery = String.Format("UPDATE secretRooms SET visited = " + 0
						+ " WHERE level_id = " + i);

					dbCmd.CommandText = sqlQuery;
					dbCmd.ExecuteScalar();
				}
				dbConnection.Close();
			}
        }

    }

   [MenuItem("File/RESET SCORE ON BUILD")]
    static void OnBuild(){
		
        OpenDB(mysticalCave);
        UpdateDatabase();
        PlayerPrefs.SetInt("totalscore", 0);
    }
#endif
   
}
