using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] bool lookUp = false;

    void Update()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 dir;
        if (lookUp)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
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