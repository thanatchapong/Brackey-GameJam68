using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshRebaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private Transform level;

    private bool pendingRebake = false;

    private void Start()
    {
        if (surface == null) surface = GetComponent<NavMeshSurface>();
    }

    private void OnTransformChildrenChanged()
    {
        if (level == null || surface == null) return;

        if (transform == level || transform.IsChildOf(level))
        {
            if (!pendingRebake)
            {
                pendingRebake = true;
                Invoke(nameof(RebuildNavMesh), 0.2f);
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
