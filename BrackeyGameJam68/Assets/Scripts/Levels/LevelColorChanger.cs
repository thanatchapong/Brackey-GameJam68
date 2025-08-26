using UnityEngine;
using TMPro;

public class LevelColorChanger : MonoBehaviour
{
    [Header("Optional: per-5-level colors")]
    [SerializeField] Color[] groundColorsByLevel;
    [SerializeField] Color[] wallColorsByLevel;
    [SerializeField] Color[] textColorsByLevel;

    public void ApplyColors(int levelIndex)
    {
        levelIndex = (levelIndex - 1) / 5;

        Color groundColor = groundColorsByLevel[levelIndex % groundColorsByLevel.Length];
        Color wallColor = wallColorsByLevel[levelIndex % wallColorsByLevel.Length];
        Color textColor = textColorsByLevel[levelIndex % textColorsByLevel.Length];

        ApplyColorToObject("Ground", groundColor);
        ApplyColorToObject("WallL", wallColor);
        ApplyColorToObject("WallR", wallColor);
        ApplyColorToObject("WallT", wallColor);
        ApplyColorToObject("WallB", wallColor);

        var textT = transform.Find("RoomNumber");
        var textSR = textT.GetComponent<TextMeshPro>();
        textSR.color = textColor;
    }

    void ApplyColorToObject(string objectName, Color color)
    {
        var objectT = transform.Find(objectName);
        var objectSR = objectT.GetComponent<SpriteRenderer>();
        objectSR.color = color;
    }

    void Start()
    {
        ApplyColors(1);
    }
}