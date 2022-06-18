using System;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.PathFinding
{
    public class GridInfo : MonoBehaviour
    {
        public Grid grid;

        public bool explored = false;



        private void OnDrawGizmosSelected()
        {
            if(explored)
                Gizmos.DrawCube(transform.position, new Vector3(2, 5, 2));
        }
    }
}