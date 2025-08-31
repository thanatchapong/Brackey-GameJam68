using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static WeaponsObject classSelected;
    public static GameObject ult;

    // -----GAMESCORE----- \\
    public static int levelReached;
    public static int enemiesKilled;
    public static int timeParry;
    public static int cardUpgraded;
    public static int obstacleBreak;
    public static float timePlayed;

    [SerializeField] TMP_Text score;
    [SerializeField] GameObject shotgun;
    [SerializeField] GameObject rifle;

    bool isHighScore;

    void Start()
    {
        levelReached = 0;
        enemiesKilled = 0;
        timeParry = 0;
        cardUpgraded = 0;
        obstacleBreak = 0;

        timePlayed = 0;

        if (shotgun) shotgun.SetActive(false);
        if (rifle) rifle.SetActive(false);
    }

    void Update()
    {
        timePlayed += Time.deltaTime;

        if (levelReached >= 5 && PlayerPrefs.GetInt("Shotgun", 0) == 0)
        {
            PlayerPrefs.SetInt("Shotgun", 1);
            PlayerPrefs.Save();
        }
        if (levelReached >= 15 && PlayerPrefs.GetInt("Rifle", 0) == 0)
        {
            PlayerPrefs.SetInt("Rifle", 1);
            PlayerPrefs.Save();
        }

        if (shotgun && PlayerPrefs.GetInt("Shotgun", 0) == 1) shotgun.SetActive(true);
        if (rifle && PlayerPrefs.GetInt("Rifle", 0) == 1) rifle.SetActive(true);

        if (score)
        {
            if (PlayerPrefs.GetInt("HighScore", 0) <= ((levelReached * 250) + (cardUpgraded * 200) + (enemiesKilled * 75) + (obstacleBreak * 8) + (timeParry * 6)))
            {
                PlayerPrefs.SetInt("HighScore", (levelReached * 250) + (cardUpgraded * 200) + (enemiesKilled * 75) + (obstacleBreak * 8) + (timeParry * 6));
                PlayerPrefs.Save();

                isHighScore = true;
            }

            score.text = "WAVE: " + levelReached + "x250= " + (levelReached * 250) + "\n" +
            "CARD: " + cardUpgraded + "x200= " + (cardUpgraded * 200) + "\n" +
            "KILL: " + enemiesKilled + "x75= " + (enemiesKilled * 75) + "\n" +
            "BREAKABLE: " + obstacleBreak + "x8= " + (obstacleBreak * 8) + "\n" +
            "PARRY: " + timeParry + "x6= " + (timeParry * 6) + "\n" +
            "TOTAL: " + ((levelReached * 250) + (cardUpgraded * 200) + (enemiesKilled * 75) + (obstacleBreak * 8) + (timeParry * 6)) + "\n\n" +
            "HIGHSCORE: " + PlayerPrefs.GetInt("HighScore", 0);

            if (isHighScore)
            {
                score.text += "\n[ NEW HIGHSCORE ]";
            }
        }
    }

    public void AssignClass(WeaponsObject plrClass)
    {
        classSelected = plrClass;
    }
}
