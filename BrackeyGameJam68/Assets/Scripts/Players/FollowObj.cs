using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float CamSpeed;

    void FixedUpdate()
    {
        Vector2 pos = new Vector2(Mathf.Lerp(transform.position.x, target.transform.position.x, CamSpeed * Time.fixedDeltaTime), Mathf.Lerp(transform.position.y, target.transform.position.y, CamSpeed * Time.fixedDeltaTime));

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
