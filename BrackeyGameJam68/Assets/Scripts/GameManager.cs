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

    void Start()
    {
        levelReached = 0;
        enemiesKilled = 0;
        timeParry = 0;
        cardUpgraded = 0;
        obstacleBreak = 0;

        timePlayed = 0;
    }

    void Update()
    {
        timePlayed += Time.deltaTime;

        if (score)
        {
            score.text = "WAVE: " + levelReached + "x250= " + (levelReached * 250) + "\n" +
            "CARD: " + cardUpgraded + "x200= " + (cardUpgraded * 200) + "\n" +
            "KILL: " + enemiesKilled + "x75= " + (enemiesKilled * 75) + "\n" +
            "BREAKABLE: " + obstacleBreak + "x8= " + (obstacleBreak * 8) + "\n" +
            "PARRY: " + timeParry + "x6= " + (timeParry * 6) + "\n" +
            "TOTAL: " + ((levelReached * 250) + (cardUpgraded * 200) + (enemiesKilled * 75) + (obstacleBreak * 8) + (timeParry * 6));
        }
    }

    public void AssignClass(WeaponsObject plrClass)
    {
        classSelected = plrClass;
    }
}
