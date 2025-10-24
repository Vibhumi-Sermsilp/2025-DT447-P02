using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _halfeExtents;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawCube(transform.position + _offset, _halfeExtents);
    }
}
