using UnityEngine;

[System.Serializable]
public struct NameMap
{
    public string name;
    public Sprite image;
    public int[] sentencesMap;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public NameMap[] nameMaps;

    [TextArea]
    public string[] sentences;

    public bool isEnded;
}
