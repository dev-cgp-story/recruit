using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertGizmoDrawer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.9f);
    }

    void OnDrawGizmosSelected()
    {
        var beforColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 2);
        Gizmos.color = beforColor;
    }
}
