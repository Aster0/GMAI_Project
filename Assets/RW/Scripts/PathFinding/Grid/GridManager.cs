using System;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.PathFinding
{


    
    
    public class GridManager : MonoBehaviour
    {


        public static GridManager Instance { get; set; }

    
        
        public int GridStraightCost { get; set; }= 10; 
        public int GridDiagonalCost { get; set; }= 14;
 

        public List<Grid> grids = new List<Grid>(); // stores all the grid on the map
        
        
        [SerializeField]
        private int maxZ, maxX; // MAX Z & X of the grid size.

        [SerializeField]
        private GameObject gridCubePrefab;

        public BoxCollider gridCubeCollider;
        
        private void Awake()
        {
            // simple singleton.
            if (Instance == null)
                Instance = this;
            
            gridCubeCollider = gridCubePrefab.GetComponent<BoxCollider>();

            GenerateGrid();
         
        }

        public void GenerateGrid()
        {

            int gridCount = 1;
            for (int x = 0; x < Mathf.Abs(maxX); x++) // left to right
            {
            
                for (int z = 0; z < maxZ; z++) // up to down 3D space
                {
                    
                    
                    GameObject gridCube = Instantiate(gridCubePrefab, 
                        new Vector3(0,
                        0, 0), Quaternion.identity);
                    
                    gridCube.transform.SetParent(this.transform);

                    gridCube.transform.localPosition = new Vector3(-(x * gridCubeCollider.size.x),
                        0.5f, z * gridCubeCollider.size.z); // edit the new local position relative to parent.
                    
                 

                    Grid grid = new Grid();

                    grid.GridCollider = gridCubeCollider;

                    grid.transform = gridCube.transform;
                    
                    grid.CheckWalkable();

                    gridCube.name = "GridCube" + gridCount;

                    grid.index = gridCount;
                    grids.Add(grid);

                    gridCube.GetComponent<GridInfo>().grid = grid;

                    gridCount++;



                }
            }
        }


        public Grid FindAtPositionGrid(Vector3 position)
        {
            foreach (Grid grid in grids)
            {
                Vector3 gridPositionToWorld = grid.transform.TransformPoint(new Vector3(0,0)); // translate to world pos so we can compare.

              
                if (gridPositionToWorld.x == position.x && gridPositionToWorld.z == position.z) 
                    // we want to ignore y
                {
                    // found node!


                    return grid;

                }
            }

            
       
            return null; 
        }

   
    }
}