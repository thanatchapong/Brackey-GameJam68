using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshRebaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface; // ใส่ NavMeshSurface ของ NavhMesh
    [SerializeField] private Transform level;        // ลาก GameObject "Level" มาวาง

    private bool pendingRebake = false;

    private void Start()
    {
        if (surface == null) surface = GetComponent<NavMeshSurface>();
    }

    private void OnTransformChildrenChanged()
    {
        if (level == null || surface == null) return;

        // เช็คว่า event มาจาก Level เท่านั้น
        if (transform == level || transform.IsChildOf(level))
        {
            if (!pendingRebake)
            {
                pendingRebake = true;
                Invoke(nameof(RebuildNavMesh), 0.2f); // รอ 0.2 วิ ก่อน bake
            }
        }
    }

    private void RebuildNavMesh()
    {
        Debug.Log("Rebuilding NavMesh after Level update...");
        surface.BuildNavMesh();
        pendingRebake = false;
    }
}
