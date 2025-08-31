using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static WeaponsObject classSelected;
    public static GameObject ult;

    public void AssignClass(WeaponsObject plrClass)
    {
        classSelected = plrClass;
    }
}
