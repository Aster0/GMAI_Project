using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.PathFinding
{
    public class AStarPathFinding : MonoBehaviour
    {
        public List<Grid> openGrids = new List<Grid>();
        public List<Grid> closedGrids = new List<Grid>();

        private List<GridPosition> positionsToCheck = new List<GridPosition>();
        private Grid startingGrid, currentGrid, destinationGrid;
        public LayerMask layerMask;

  
        private void Start()
        {

            GeneratePositions();
            GetStartAndEndGrid();
         
           
        }


        public Collider FindNearestGridToPosition(Vector3 pos) // find the nearest grid t o the inputted position in parameters
        {
            float nearestDistance = Mathf.Infinity;

            Collider nearestCollider = null;
            
            foreach (Collider col in Physics.OverlapSphere(pos, 0.5f))
            {
               

                if (col.gameObject.name.Contains("Grid")) // is a grid
                {
                    if (Vector3.Distance(pos, col.gameObject.transform.position) < nearestDistance)
                    {
                        nearestDistance = Vector3.Distance(pos, col.gameObject.transform.position);
                        nearestCollider = col;
                        // basically find the closest grid to the object.

                    }
                }
            }

            return nearestCollider;
        }

        public void GetStartAndEndGrid()
        {
          
            


            startingGrid = GetNearestGridToPosition(this.transform.position);
            destinationGrid = GetNearestGridToPosition(new Vector3(8.79f, 0, -13.44f));


            currentGrid = startingGrid;
            
            closedGrids.Add(startingGrid);

        }

        public Grid GetNearestGridToPosition(Vector3 pos)
        {
           

            

            Collider nearestCollider = FindNearestGridToPosition(pos);
            

            if (nearestCollider != null)
            {
                Grid gridFound = nearestCollider.GetComponent<GridInfo>().grid;
                Debug.Log("Grid Found: Grid#" + gridFound.index);

                return gridFound;
          
                
         
            }
          
            
            Debug.Log("Unable to find the nearest collider somehow.. " + pos);
            

            return null;
        }

        public void GeneratePositions()
        {
            GridManager gridManager = GridManager.Instance;
            Vector3 colliderSize = gridManager.gridCubeCollider.size;
            // POSITIONS TO CHECK (LEFT RIGHT TOP DOWN, LEFT-TOP RIGHT-TOP, LEFT-DOWN RIGHT-DOWN)
            positionsToCheck.Add(new GridPosition(new Vector3(colliderSize.x, 0, 0), 
                gridManager.GridStraightCost));
            
            
            positionsToCheck.Add(new GridPosition(new Vector3(-colliderSize.x, 0, 0),
                gridManager.GridStraightCost));
            
            positionsToCheck.Add(new GridPosition(new Vector3(colliderSize.x, 0, -colliderSize.z),
                gridManager.GridDiagonalCost));
            
            positionsToCheck.Add(new GridPosition(new Vector3(-colliderSize.x, 0, -colliderSize.z),
                gridManager.GridDiagonalCost));
            
            positionsToCheck.Add(new GridPosition(new Vector3(0, 0, colliderSize.z),
                gridManager.GridStraightCost));
            positionsToCheck.Add(new GridPosition(new Vector3(0, 0, -colliderSize.z),
                gridManager.GridStraightCost));
            
            positionsToCheck.Add(new GridPosition(new Vector3(colliderSize.x, 0, colliderSize.z),
                gridManager.GridDiagonalCost));
            positionsToCheck.Add(new GridPosition(new Vector3(-colliderSize.x, 0, colliderSize.z),
                gridManager.GridDiagonalCost));
        }
        
        public void SearchPath(Vector3 destination)
        {
      

         


            
           
            



            foreach (GridPosition gridPosition in positionsToCheck) // check all the grids around the current grid position.
            {
                
          
                
                Grid grid = GridManager.Instance.FindAtPositionGrid(currentGrid.transform.position + gridPosition.pos);


           
         

                if (grid != null) // if grid is found, sometimes to the left or right or etc might be out of the map so no grid there.
                {
                 
                        
                
                    
                    if (grid.Walkable) // if grid is walkable
                    {

                        bool noSearch = false;
                
                        foreach (Grid closedGrid in closedGrids)
                        {
                            if (closedGrid.index == grid.index)
                            {
                       
                                Debug.Log("We searched this before");
                                noSearch = true;
                            }
                        }
                
                        foreach (Grid openGrid in openGrids)
                        {
                            if (openGrid.index == grid.index)
                            {
                               
                                
                                noSearch = true;
                            }
                        }
                        
         

                        if (!noSearch)
                        {
                            Grid newGridInstance = new Grid(); // so this instance is unique to this current pathfinder,
                            // as other pathfinders might have different f, g, h calculations to the destination.

                            newGridInstance.transform = grid.transform;
                            newGridInstance.index = grid.index;
                    
                            newGridInstance.g = newGridInstance.CalculateG(currentGrid, gridPosition.gridCost);
                            newGridInstance.h = newGridInstance.CalculateH(destination);

                            newGridInstance.f = newGridInstance.g + newGridInstance.h;

                
                            newGridInstance.previousGrid = currentGrid; // update previous grid to the current one as we're
                            // gonna step into this new grid.

                            grid.transform.gameObject.GetComponent<GridInfo>().explored = true;
                            openGrids.Add(newGridInstance);
                        }
                        

                        
                        
                       

                    }
               
                }
            }


 


        }
        
        public void StepLeastF() // step into the lowest F value grid
        {



            if (currentGrid.index != destinationGrid.index)
            {
                // FINDING THE SMALLEST F OF THE NEAREST GRID WE JUST CHECKED AND CALCULATED.

                if (openGrids.Count > 0) // more than 1 because starting node is 1, need to search for more open grids
                {
                
             
                    openGrids = openGrids.OrderBy(n => n.f).ToList(); // order by smallest to biggest F costs

            
                    Grid lowestGrid = openGrids[0]; // after being sorted
                



                    currentGrid = lowestGrid; // update the new current grid.
        
                    Debug.Log("Still finding.. " + currentGrid.index + " " + currentGrid.transform.position);
                    closedGrids.Add(lowestGrid); // as we stepped into it, we can close it.

                    openGrids.Remove(lowestGrid); // remove from open grid as we have already stepped on it.
                
                }
            

        
                SearchPath(destinationGrid.transform.position);
            }
            else
            {
             

                if(!moving)
                    StartCoroutine(MoveToDestination());

            }
            
         
         
        
            
            
        }

        private bool moving = false;
        private IEnumerator MoveToDestination()
        {
            moving = true;
            Grid endNode = null; 
            foreach (Grid node in closedGrids)
            {
                if (node.index == destinationGrid.index)
                {
                    endNode = node;
                    break;
                
                }
            }

            List<Grid> destinationNodes = new List<Grid>();
        
            Grid currentNodeHere = endNode;
        
            destinationNodes.Add(endNode);


            while (currentNodeHere != startingGrid)
            {
                destinationNodes.Add(currentNodeHere.previousGrid);

                currentNodeHere = currentNodeHere.previousGrid;
            }

            destinationNodes.OrderBy(n => n.index);
            destinationNodes.Reverse();
        
            foreach (Grid node in destinationNodes)
            {
                transform.position = node.transform.TransformPoint(new Vector3(0,0));

                yield return new WaitForSeconds(0.2f);


            }
       
        }

        private void Update()
        {
            StepLeastF();
        }
    }
    
    
    
    



    public class GridPosition
    {
        public Vector3 pos;
        public float gridCost;


        public GridPosition(Vector3 pos, float gridCost)
        {
            this.pos = pos;
            this.gridCost = gridCost;
        }
    }
}