using UnityEngine;
using System;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private string targetTag;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] GameObject hideWhenNoTarget;
    [SerializeField] AudioSource hideSound;
    [SerializeField] bool lookUp = false;

    void Start()
    {
        if (hideSound) hideSound.volume = 0;
    }

    void Update()
    {
        if (hideWhenNoTarget && target == null)
        {
            hideWhenNoTarget.SetActive(false);
            if (hideSound) hideSound.Play();
        }
        else if (hideWhenNoTarget)
        {
            hideWhenNoTarget.SetActive(true);
            if (hideSound) hideSound.volume = 1;
        }

        if (target != null && target.gameObject.activeSelf == false) target = null;
        if (target == null && targetTag == "") target = GameObject.FindGameObjectWithTag("Player").transform;
        else if (targetTag != "")
        {
            try
            {
                target = GameObject.FindGameObjectWithTag(targetTag).transform;
            }
            catch (Exception ex)
            {
                return;
            }
        } 

        Vector3 dir;
        if (lookUp)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(target != null)
        {
            dir = target.position - transform.position;

            // Compute angle so that Y-axis points toward target
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            // Smooth rotation only around Z-axis
            Quaternion targetRotation = Quaternion.Euler(0, 0, -angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }
}