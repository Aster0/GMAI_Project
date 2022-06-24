using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity.PathFinding
{
    
    public class AStarPathFinding 
    {
        public List<Grid> openGrids = new List<Grid>();
        public List<Grid> closedGrids = new List<Grid>();

        private List<GridPosition> positionsToCheck = new List<GridPosition>();
        public Grid startingGrid, currentGrid, destinationGrid;
        public LayerMask layerMask;
        private bool isSearching;

        public Transform transform; // parent transform

    

        private Vector3 currentDestination, setDestination;

        private Coroutine movementCoroutine;
        
        private bool moving = false, destinationBlocked;
        public Rigidbody rb;
        

        public Animator animator { get; set; }

        
        private Grid nextGridDestination;
        private int nextGridCount;
        private List<Grid> destinationGrids = new List<Grid>();

        [SerializeField]
        private int speed = 3;

        public bool slowDown { get; set; }


        public AStarPathFinding()
        {

      
            GeneratePositions();
            
        }


        // this method should be the method u use to move using A*.
        // put this in Update().
        public void BuildPath(Vector3 destination)
        {
            SetDestination(destination); // we'll set the destination we want to go
            StepLeastF(); // then we'll find neighbouring grids and step into the least F
            Move(); // then, we'll move to the destinations node that we have created.
        }

        private void SetDestination(Vector3 destination) // set a new destination for the A* to know 
        {
         
            //animator.SetFloat("Forward", 0); // turn off the animation for walking



            foreach (Grid grid in GridManager.Instance.grids) // iterate all the grids
            {
                
                // to save cost on checking on every grid as we calculate the g, h, f cost later,
                // we can just check the grids and check which is walkable so
                // we have one less calculation later.
                grid.CheckWalkable();
                
                
            }
            
            GetStartAndEndGrid(destination);
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

        public void GetStartAndEndGrid(Vector3 pos)
        {



            nextGridCount = 0;

            startingGrid = GetNearestGridToPosition(this.transform.position); // getting the startingGrid using the current position
            destinationGrid = GetNearestGridToPosition(pos); // getting the destination using the vector3 from the method parameter 
          

            currentDestination = pos; 

            currentGrid = startingGrid; // the current grid is the starting grid, because... yeah.. we're starting here..

            openGrids.Clear(); // reset grids
            closedGrids.Clear(); // reset grids
            
            closedGrids.Add(startingGrid); // we close the starting grid because we just explored it.
            
            isSearching = false;
            moving = false; // reset variables
            destinationBlocked = false;



        }

        public Grid GetNearestGridToPosition(Vector3 pos)
        {
           

            

            Collider nearestCollider = FindNearestGridToPosition(pos); // to find the nearest grid's collider
            // to the inputted pos (from method parameter)
            

            if (nearestCollider != null) // a null check to make sure we found a collider 
            {
                Grid gridFound = nearestCollider.GetComponent<GridInfo>().grid; // get the grid's GridInfo component.
                // so we can get the grid info.
                
                //Debug.Log("Grid Found: Grid#" + gridFound.index);

                return gridFound; // return the grid found.
          
                
         
            }
          
            
            Debug.Log("Unable to find the nearest collider somehow.. " + pos); // error message
            

            return null;
        }

        public void GeneratePositions() // to generate the different neighbouring grids we need to check around the current grid.
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
                // find the grid at said neighbouring position

           
                
         

                if (grid != null) // if grid is found, sometimes to the left or right or etc might be out of the map so no grid there.
                {
            
                        
                
      
                
                    
                    if (grid.Walkable) // if grid is walkable
                    {

                        bool noSearch = false; // boolean to tell us if we should search this current grid or not. 
                
                        foreach (Grid closedGrid in closedGrids) // if the grid is in the closed grid
                        {
                            if (closedGrid.index == grid.index) // and if its the current grid  we're checking..
                            {
                       
                                
                                noSearch = true; // we don't search it
                            }
                        }
                
                        foreach (Grid openGrid in openGrids) // if the grid is in the open grid
                        {
                            if (openGrid.index == grid.index)  // and if its the current grid  we're checking..
                            {
                               
                                
                                noSearch = true; // we don't search it
                            }
                        }
                        
         

                       
                        if (!noSearch) // only search if noSearch is false.
                        {
                            Grid newGridInstance = new Grid(); // so this instance is unique to this current pathfinder,
                            // as other pathfinders might have different f, g, h calculations to the destination.

                            newGridInstance.transform = grid.transform;
                            newGridInstance.index = grid.index;
                    
                            newGridInstance.g = newGridInstance.CalculateG(currentGrid, gridPosition.gridCost);
                            // g cost = the cost we have used until now.. so that's why currentGrid + the new grid G cost.
                            
                            newGridInstance.h = newGridInstance.CalculateH(destination);
                            // h cost = the distance from this grid, to the destination grid using Euclidean in our case.

                            newGridInstance.f = newGridInstance.g + newGridInstance.h;
                            // f = g + h.

                
                            newGridInstance.previousGrid = currentGrid; // update previous grid to the current one as we're
                            // gonna step into this new grid.


       
                            //grid.transform.gameObject.GetComponent<GridInfo>().explored = true;
                            openGrids.Add(newGridInstance);
                         
                        }
                        

                        
                        
                       

                    }
                    else // if the grid is unwalkable
                    {
                        if (grid.index == destinationGrid.index) // and is a destination grid
                        {


                      
                            // we just grab the closest grid which is this previous grid as the destination.
                            destinationGrid = currentGrid;
                            // as we dont want the AI to stop walking or navigating just because it can't walk to the location
                            // so we can get as close as possible to the grid.
                            
                            // the current grid is the previous grid before opening the new adjacent grids.


                     

                            
                            Debug.Log("Found new destination" + currentGrid.index + " " + destinationGrid.index);

                            break;


                        }
                    }
               
                }
            }

            isSearching = false;





        }
        
  
        
        private void StepLeastF() // step into the lowest F value grid
        {

  
            if (destinationGrid == null) // make sure that destination grid is set.
                return;
                
            isSearching = true;

            
  


            
            
            while (currentGrid.index != destinationGrid.index)
            {
                if (!destinationGrid.Walkable)
                    return;
                
                // FINDING THE SMALLEST F OF THE NEAREST GRID WE JUST CHECKED AND CALCULATED.

         
         
                if (openGrids.Count > 0) // if least one open grid is added
                {
                
                 
             
                    openGrids = openGrids.OrderBy(n => n.f).ToList(); // order by smallest to biggest F costs

            
                    Grid lowestGrid = openGrids[0]; // after being sorted
                


                    
                    currentGrid = lowestGrid; // update the new current grid.
        
               
                    closedGrids.Add(lowestGrid); // as we stepped into it, we can close it.

                    openGrids.Remove(lowestGrid); // remove from open grid as we have already stepped on it.
                    
                    
                
                }
                
                
                SearchPath(destinationGrid.transform.position);
                
       

           



            }
            
            
            
            if(currentGrid.index == destinationGrid.index)
            {

                FindGridPath();

            }
            
         
         
        
            
            
        }

        public void FindGridPath()  // to find the grids we need to take on the path to the destination.
        {

     
            if (!destinationGrid.Walkable)
                return;
        
            Grid endNode = null; 
            foreach (Grid node in closedGrids) // we iterate the closed grid
            {
                if (node.index == destinationGrid.index) // and try to find the end node that we have explored in the closed list.
                {
                    endNode = node; // then we assign it to a variable so now we have an instance of it.
                    break;
            
                }
            }

            destinationGrids = new List<Grid>(); // new list to store all the destination grids.
    
            Grid currentGridHere = endNode; // for us to use to go previous nodes from the end node
                                            // until we find the starting node.
    
            destinationGrids.Add(endNode); // add the end node to the destination grids

        
            
       


            while (currentGridHere != startingGrid) // iterate so as long as the currentGridHere isn't startingGrid, we keep going
            {
      
                destinationGrids.Add(currentGridHere.previousGrid); // add the previous grid 
            
        

                currentGridHere = currentGridHere.previousGrid; // update the currentGridHere as the previous grid
                // so the next iteration, it keeps going back and back until we hit the starting node,
                // then we know the path.
            }

    
 
            destinationGrids.Reverse(); // since we saved from the end grid to the start grid..


            if (destinationGrids.Count > 1) // the index 0 is the starting point, we want the next which is where we should
            // move on to. that's why count > 1 because we need a size of 2 inside.
            {
                nextGridDestination = destinationGrids[1];
                nextGridCount = 1;
            }
 


            // we need to reverse so it starts from the start node to the end grid.
            // so now we have a viable path to the end grid.

            /*if (Vector3.Distance(destinationGrids[0].transform.position, transform.position) > 2)
            {
                destinationGrids.Remove(destinationGrids[0]);
            }



            try
            {
                nextGridDestination = destinationGrids[0];
                nextGridCount = 1;
            }
            catch (ArgumentException e) 
                // catching the error if we can't get the first dest because we are using coroutines.
            {
               
            }*/
    
            
            
            
            
            
    
        }




        private void Move()
        {
                     
        
            
            /*if (currentDestination != setDestination)
            {
                GetStartAndEndGrid(setDestination);
            }*/
            
            StepLeastF();
  
      
            
 
            if (destinationGrids.Count > 0) // have destinations to follow
            {
                
     
           

                Vector3 toPos = 
                    
                    new Vector3(nextGridDestination.transform.TransformPoint(new Vector3(0, 0)).x, 
                        0,
                        nextGridDestination.transform.TransformPoint(new Vector3(0, 0)).z);
                // y is 0 because the grid is slightly elevated so we need to make it back to 0
                // to this position
                
                transform.position = Vector3.MoveTowards(
                    transform.position, toPos, Time.deltaTime * 5);

                   //rb.velocity = (toPos - transform.position).normalized * 4;
                   // we use velocity instead of transform.position because we don't need to constantly update
                   // the new location to move slowly with Time.deltaTime.
                   // with velocity, we can just push the velocity towards a certain direction and do it
                   // only once.
            
                animator.SetFloat("Forward", 1); // turn on the animation for walking
                
        

                if (Vector3.Distance(transform.position, 
                        toPos) < 1f) // if the distance is 0.5f away from the next path grid, 
                {

                    // either move to the next path
                    if (nextGridCount + 1 != destinationGrids.Count) // if we still can count up in the destination grid
                    {
                        nextGridCount++; // we count up
                  
                   
         

                        nextGridDestination = destinationGrids[nextGridCount]; // and we get the next one.

  
                   
                    }
           
                    
                    
             

                }






                LookTowardsGrid(); // rotate towards the grid.

            }
 

            if (destinationGrid != null) // there must be a destination.
            {
                if (Vector3.Distance(transform.position, destinationGrid.transform.position) < 6) // if its this distance away from the destination, we start to slow down.
                {
                    animator.SetFloat("Forward", 0, 0.125f, Time.deltaTime); 
                    // slowly slow down using animation blend because the next grid is the destination.
                }
            }
   
        }

        private void LookTowardsGrid()
        {
       
            Vector3 toPos = 
                    
                new Vector3(nextGridDestination.transform.TransformPoint(new Vector3(0, 0)).x, 
                    0,
                    nextGridDestination.transform.TransformPoint(new Vector3(0, 0)).z);
            
            // y is 0 because the grid is slightly elevated so we need to make it back to 0
            
            // the direction to rotate towards
            Vector3 targetDirection =  toPos
                                       - transform.position;
            
            
            Quaternion rotation =
                Quaternion.LookRotation(targetDirection);




            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5);
            

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