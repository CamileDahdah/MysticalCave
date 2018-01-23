using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{


    static int[] scoresPerLevel;
    public GameObject scoreGameObject;
    private static Text scoreText;
    static int enemyScore = 50;
    static int damageScore = 25;
    static int totalDamageScore = damageScore * 4;
    static int perfectScoreTime = 400;
    static int scoreSecretPath = 150;
    static int scorePerSkill = 100;
  //static int damageConversion = 25;
    static int numLevels = GameManager.numLevels;
    static float[] scorePercentages = { .3f, .6f, .9f };
    public float duration = 1f;
    static int score = 0;
    static int perfectScore = 0;
    int bloodCount = scorePercentages.Length;
    public GameObject[] bloodImages;
	public AudioClip[] bloodClips;
    static ScoreManager scoreManager;
    private static string connectionString;

    void Start()
    {
       
        scoreManager = this;
        ScoreManager.scoreText = scoreGameObject.GetComponent<Text>();
        connectionString = GameManager.connectionString;
		Application.targetFrameRate = 60;

		#if UNITY_EDITOR
			if(connectionString != ""){
				LoadLevels();
			}
		#else
       	 	LoadLevels();
		#endif

    }

    public static void UpdateScore(int levelID, int numEnemies, int timeElapsed, int secretPath, int damageTaken)
    {
        int tempScore = 0;
        int minimumScore = LevelManager.GetMinimumScore(); //get the minimum score from the levelManager
        int offset = LevelManager.GetOffset(); // get it from levelManager
        int maxScorePath = LevelManager.GetNumSecretPath() * scoreSecretPath;
        int minimumTime = LevelManager.GetMinimumTime();
        int maxScoreEnemies = LevelManager.GetNumEnemies() * enemyScore;
        int scoreEnemies = numEnemies * enemyScore;
        int scorePath = secretPath * scoreSecretPath;
        int scoreDamage = 0;

        if (levelID == 1)
            perfectScore = minimumScore + totalDamageScore + maxScorePath + maxScoreEnemies;
        else
            perfectScore = minimumScore + totalDamageScore + perfectScoreTime + maxScorePath + maxScoreEnemies;

        if (damageTaken > offset)
        {
            int realDamage = damageTaken - offset;

            scoreDamage = totalDamageScore - (realDamage);

            if (scoreDamage < 0)
                scoreDamage = 0;
        }

        else
            scoreDamage = totalDamageScore;

        int timedifference = timeElapsed - minimumTime;
        int scoreTime = 0;

        if (timedifference < 0)
            scoreTime = perfectScoreTime;
        else
        {
            float newPerc = ((minimumTime - (timeElapsed - minimumTime)) / (float)minimumTime);
            if (newPerc > 0)
            {
                float floatscore = (perfectScoreTime * newPerc);

                scoreTime = (int)floatscore;

            }
        }
        Debug.Log(minimumScore);
        Debug.Log(scoreEnemies);
        Debug.Log(scorePath);
        Debug.Log(scoreTime);
        Debug.Log(scoreDamage);
        tempScore = minimumScore + scoreEnemies + scorePath + scoreTime + scoreDamage;

        if (tempScore > scoresPerLevel[levelID - 1])
        {

            int tempLevelScore = scoresPerLevel[levelID - 1];


            scoresPerLevel[levelID - 1] = tempScore;
            int totalScore = PlayerPrefs.GetInt("totalscore", 0);
            totalScore -= tempLevelScore;
            totalScore += tempScore;
            PlayerPrefs.SetInt("totalscore", totalScore);
            scoreText.color = Color.red;
            score = tempScore;
            scoreManager.StopCoroutine("CountTo");
            scoreManager.StartCoroutine("CountTo", score);
            UpdateDatabase(levelID - 1, true);
        }
        else
        {

            scoreText.color = Color.black;
            score = tempScore;
            scoreManager.StopCoroutine("CountTo");
            scoreManager.StartCoroutine("CountTo", score);
			UpdateDatabase(levelID - 1, false);
        }
     //   int skillsPoints = ConvertToSkills();


    }

    public int GetScore()
    {
        return PlayerPrefs.GetInt("totalscore", 0);
    }

    public static int ConvertToSkills()
    {
        return PlayerPrefs.GetInt("totalscore", 0) / scorePerSkill;
    }

    public static void UpdateDatabase(int level_id, bool found)
    {
		//UPDATE SCORE
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {

			string sqlQuery;
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
             

                if (found)
                {
                    sqlQuery = String.Format("UPDATE scoresLevel SET score = " +
                      scoresPerLevel[level_id]
                        + " WHERE level_id = " + (level_id + 1));
					dbCmd.CommandText = sqlQuery;
					dbCmd.ExecuteScalar();

                }
              //  else
                //    sqlQuery = String.Format("Insert Into scoresLevel (level_id, score) Values (\"{0}\", \"{1}\")",
                  //      level_id + 1, scoresPerLevel[level_id]);

           
           
            }
			//UPDATE SECRETROOM
			using (IDbCommand dbCmd = dbConnection.CreateCommand())
			{
				
				if (LevelManager.instance.activatedRoomList.Count > 0) {
					String count = "";
					for (int i = 0; i < LevelManager.instance.activatedRoomList.Count; i++) {
						if (i != LevelManager.instance.activatedRoomList.Count - 1)
							count += LevelManager.instance.activatedRoomList [i] + ",";
						else
							count += LevelManager.instance.activatedRoomList [i];

					}

					sqlQuery = String.Format ("UPDATE secretRooms SET visited = " +
					1
					+ " WHERE level_id = " + (level_id + 1) + " AND room_number IN ( " +
						count + " )"); //Replace value
			

				
					dbCmd.CommandText = sqlQuery;
					dbCmd.ExecuteScalar();
				}
				dbConnection.Close();
			}
        }
    }
    public void LoadLevels()
    {
        scoresPerLevel = new int[numLevels];

        for (int i = 0; i < numLevels; i++)
        {
            scoresPerLevel[i] = -1;
        }

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "Select * from scoresLevel";

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(1) - 1;
                        int score = reader.GetInt32(2);

                        scoresPerLevel[id] = score;

                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

    }

    IEnumerator CountTo(int target)
    {
        int counter = 0;
        float[] differentScores = { perfectScore * scorePercentages[0],
            perfectScore * scorePercentages[1],
            perfectScore * scorePercentages[2] };
        scoreText.text = "" + 0;
        yield return new WaitForSeconds(1.2f);
		GetComponent<AudioSource> ().Play ();
        int start = 0;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            score = (int)Mathf.Lerp(start, target, progress);
            scoreText.text = "" + score;
            if (counter < bloodCount && score > differentScores[counter])
            {
				if (!bloodImages [counter].activeSelf)
				{
					bloodImages [counter].SetActive (true);
					SoundManager.playCollectibleAudio (bloodClips [counter]);
				}
                counter++;
            }
            yield return null;
        }
        scoreText.text = "" + target;
		GetComponent<AudioSource> ().Stop();
        yield return null;
    }


}
