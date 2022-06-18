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

            Gizmos.color = Color.red;
            if (!grid.Walkable)
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(2, 5, 2));
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position, new Vector3(2, 5, 2));
            }
            
            Gizmos.color = Color.red;
            /*//Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);*/
        }
    }
}