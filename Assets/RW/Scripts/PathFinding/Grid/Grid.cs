using System;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.PathFinding
{


    [Serializable] // so we can see in the inspector.
    public class Grid  // stores what each grid needs
    {
        public Grid previousGrid;
        public BoxCollider GridCollider { get; set; }

        public Transform transform;
    
  
        
        public int index; // index of the grid. Can see in inspector for debugging too.

        public int gridSizeX, gridSizeZ, gridSizeY;

        public float g { get; set; } = 0;
        public float h { get; set; } = 0;
        public float f { get; set; } = 0;


        public bool Walkable { get; set; } = true; // default will be walkable.

        public float CalculateG(Grid currentGrid, float gridCost)
        {
            return currentGrid.g + gridCost;
        }

        public float CalculateH(Vector3 destination)
        {
            // Euclidean - we want diagonal movement!

            Vector3 pos = transform.position;

           
            return Mathf.Sqrt(
                Mathf.Abs((destination.x - pos.x) * (destination.x - pos.x)) +
                Mathf.Abs((destination.z - pos.z) * (destination.z - pos.z)));

        }


        public void CheckWalkable() // check if current grid is walkable or not.
        {

            RaycastHit m_Hit;
            Physics.BoxCast(GridCollider.gameObject.transform.position,
                GridCollider.gameObject.transform.lossyScale / 2,
                new Vector3(0, -1, 0), out m_Hit, Quaternion.identity, 1500f);
            
            if(m_Hit.collider != null)
                Debug.Log(m_Hit.collider.gameObject);
        }

        
        
        /*private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(2f, 2f ,2f));
        }*/



    }
}