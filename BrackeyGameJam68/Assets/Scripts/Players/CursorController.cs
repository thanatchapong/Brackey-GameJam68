using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] GameObject crosshair;
    Camera cam;
    [SerializeField] bool lookAtCrossHair;
    [SerializeField] bool CloseCursor;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mwp = cam.ScreenToWorldPoint(Input.mousePosition);
        mwp.z = 0;
        crosshair.transform.position = mwp;

        if(lookAtCrossHair)
        {
            transform.LookAt(crosshair.transform);
        }
        if(CloseCursor)
        {
            Cursor.visible = false;
        }
    }
}