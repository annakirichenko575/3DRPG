using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class Blob : MonoBehaviour
    {
        private float radius = .5f;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}