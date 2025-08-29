using UnityEngine;
using System.Collections.Generic;

public class EnemyTypeColorizer : MonoBehaviour
{
  private float totalDeltaTime = 0f;

  // 1000ms stay in same color, 250ms to shift between colors
  private float colorShowTime = 1f;
  private float colorShiftTime = 0.25f;

  private float colorTotalTime;

  Transform square;
  SpriteRenderer spriteRenderer;
  public List<Color> colors = new List<Color>();
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    colorTotalTime = colorShowTime + colorShiftTime;
    for (int i = 0; i < this.gameObject.transform.childCount; i++)
    {
      Transform child = this.gameObject.transform.GetChild(i);
      if (child.name == "Square")
      {
        square = child;
      }
    }
    spriteRenderer = square.GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {

    totalDeltaTime += Time.deltaTime;
    Color color;

    // edge case
    if (colors.Count == 0)
    {
      // do nothing as there's no color to shift from/to
      return;
    }

    int round = (int)Mathf.Floor(totalDeltaTime / colorTotalTime);
    float timeInThisRound = totalDeltaTime - round * colorTotalTime;
    int index = round % colors.Count;

    if (timeInThisRound < colorShowTime)
    {
      // is in showing state
      color = colors[index];
    }
    else
    {
      // is in shifting state
      float red, green, blue;
      float shiftTimeElapsed = timeInThisRound - colorShowTime;
      float nextColorRatio = shiftTimeElapsed / colorShiftTime;
      Color currentColor = colors[index];
      Color nextColor = colors[(index + 1) % colors.Count];
      red = nextColor.r * nextColorRatio + currentColor.r * (1 - nextColorRatio);
      green = nextColor.g * nextColorRatio + currentColor.g * (1 - nextColorRatio);
      blue = nextColor.b * nextColorRatio + currentColor.b * (1 - nextColorRatio);
      color = new Color(red, green, blue);
    }


    spriteRenderer.color = color;
  }
}
