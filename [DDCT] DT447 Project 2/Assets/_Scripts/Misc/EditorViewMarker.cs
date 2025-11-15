using UnityEngine;

public class EditorViewMarker : MonoBehaviour
{
    [SerializeField] private bool _marker = true;
    [SerializeField] private Color _color;
    [SerializeField] private bool _applyLocalTransform = true;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _halfExtents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start() { }

    // Update is called once per frame
    //void Update() { }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_marker)
        {
            if (_applyLocalTransform)
            {
                /* Apply the object's local to world transform */
                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = transform.localToWorldMatrix;

                /* Draw in local space */
                Gizmos.color = _color;
                Gizmos.DrawCube(_offset, _halfExtents * 2f);

                // Restore matrix
                Gizmos.matrix = oldMatrix;
            }
            else
            {
                /* Draw in world space */
                Gizmos.color = _color;
                Gizmos.DrawCube(transform.position + _offset, _halfExtents * 2f);
            }
        }
    }
#endif
}
