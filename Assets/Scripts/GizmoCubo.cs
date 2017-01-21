using UnityEngine;
using System.Collections;

public class GizmoCubo : MonoBehaviour
{
    public Color color;

    void OnDrawGizmos()
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;

        Gizmos.color = color;
        Gizmos.DrawCube(Vector3.zero, new Vector3(1, 1, 1));
    }
}
