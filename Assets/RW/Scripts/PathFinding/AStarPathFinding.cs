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
        public Grid startingGrid, currentGrid, destinationGrid, cachedCurrentGrid;
        public LayerMask layerMask;
        private bool unreachableDestination;

        public Transform transform; // parent transform

    

        private Vector3 currentDestination, setDestination;

        private Coroutine movementCoroutine;
        
        public Rigidbody rb;
        

        public Animator animator { get; set; }

        
        private Grid nextGridDestination;

        private List<Grid> destinationGrids = new List<Grid>();



        public bool slowDown { get; set; }


        public AStarPathFinding()
        {

      
            GeneratePositions();
            
        }


        // this method should be the method u use to move using A*.
        // put this in Update().
        public void BuildPath(Vector3 destination)
        {
            GetStartAndEndGrid(destination); // we'll set the destination we want to go
            

            if (!unreachableDestination && destinationGrid != null && startingGrid != null) // we only want to step and move if its reachable.
            {
                /*
                    StepLeastF Explanation: 
                    We then run the #StepLeastF method that finds the neighbouring grids
                    (left, right, top left, top right, bottom left, bottom right).
                    If it’s walkable and not in the open or closed list already,
                    we put it into the open list as explored.
                    At the same time, we save the current grid that was linked to the neighbour
                    grid so we can trace our steps back to find the path later.

                    For each neighbour, we use the #CheckWalkable from the grid instance to use raycast to check
                    if there’s a game object in the area with the tag “Obstacles”. If there is,
                    Walkable is set to false and is ignored, not explored. So it’ll never be part of the path.

                    We then sort these walkable neighbour grids from the lowest to highest F cost.
                    To get the F cost, we take G cost + H cost. 

                    G cost = the accumulated movement cost so far (straight cost = 10, diagonal cost = 14)
                    H cost = the distance using Euclidean calculation to the destination grid
                    
                    After sorting, we get the 0 index, as that is the smallest F cost grid. 
                    We then step into that grid as current grid
                    and repeat the process above again to find its neighbours.
                    
                 */
                StepLeastF(); // then we'll find neighbouring grids and step into the least F
                
                
                Move(); // then, we'll move to the destinations node that we have created.
            }
      

            
            
            // because it's a moving player/object, we need to constantly update the destinations and search the grid.
            // for the path.
            
            // for the movement, we only need to move 1 forward (from the starting point),
            // doing this will eventually get us to the destination as we constantly search for it.
        }



        public Collider FindNearestGridToPosition(Vector3 pos) // find the nearest grid t o the inputted position in parameters
        {
            float nearestDistance = Mathf.Infinity;

            Collider nearestCollider = null;
            
            foreach (Collider col in Physics.OverlapSphere(pos, 0.5f)) // iterate the colliders around the sphere.
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

            return nearestCollider; // return the nearest collider after iterating all the collider found around the raycasted sphere
        }

        private void CheckIfUnreachable()
        {
            // for destination grid
            int unwalkableNeighbours = 0;
            int totalNeighboursGrids = 0;
            
            // for current grid.
            int unwalkableNeighboursCurrentGrid = 0;
            int totalNeighboursGridsCurrentGrid = 0;
            unreachableDestination = false;
 
            
            foreach (GridPosition gridPosition in
                     positionsToCheck)
            {


                // this is to check all the neighboring grids to ensure the destination isn't sandwiched in the middle
                // of a bunch of unreachable place. we don't even want to try walking there
                // as our current grid will never reach there to calculate because well.. it's sandwiched
                // between a bunch of unwalkables which will never be explored in the opened list.
                
                // this is so we can efficiently update the map as well, to get a good idea on where is walkable and not.

                

                Grid grid = GridManager.Instance.FindAtPositionGrid(destinationGrid.transform.position + gridPosition.pos);
                // find the grid at said neighbouring position
                
                Grid gridCurrentPositionNeighbour = GridManager.Instance.FindAtPositionGrid(startingGrid.transform.position + gridPosition.pos);
                // find the grid at said neighbouring position
                // we also want to check the starting position if its not surrounded by neighbouring unwalkable grids.

                if (grid != null) // not null because it has to be an actual grid there. neighbouring could mean out of the map which doesn't have a grid so it returns a null.
                {
                    grid.CheckWalkable(); // check if these grids are indeed walkable.
                    
                    if (!grid.Walkable) // if it's not, 
                    {
                        unwalkableNeighbours++; // we add on to the unwalkable neighbour total amount
                    }

                    totalNeighboursGrids++; // regardless, we count the number of actual neighbour grids there are
                }
                
                // later, if this goes through, we again check if the grids are walkable whenever we step into the least
                // f grid and check its neighbours.
                
                // this method is just a safety measure because if a grid is sandwiched in the middle of unwalkable grids,
                // it could never be stepped into and check if its unwalkable.

                // same logic as above but this time to check the current grid neighbouring grids if its 100% walkable in 
                // all direction.
                if (gridCurrentPositionNeighbour != null)
                {
                    // this code was added after the video documentation to ensure no flow errors.
                    // although it's very unlikely that the enemy will be surrounded with unwalkable neighbour grids 
                    // at its start grid, it's a good habit to prevent these errors so
                    // the project will not crash.
                    
                    gridCurrentPositionNeighbour.CheckWalkable(); // check if these grids are indeed walkable.
                    
                    if (!gridCurrentPositionNeighbour.Walkable) // if it's not, 
                    {
                        unwalkableNeighboursCurrentGrid++; // we add on to the unwalkable neighbour total amount
                    }

                    totalNeighboursGridsCurrentGrid++; // regardless, we count the number of actual neighbour grids there are
                }
                
            }

            // if either destination or starting grid has all unwalkable neighbours, we deem as unreachable.
            if ((unwalkableNeighbours == totalNeighboursGrids) ||
                (unwalkableNeighboursCurrentGrid == totalNeighboursGridsCurrentGrid)) // if the actual neighbour grids and unwalkable neighbour are the same
            {
             
                unreachableDestination = true; // maens its sandwiched in the middle of a bunch of unwalkable grids.
                
            }
        }

        private void GetStartAndEndGrid(Vector3 pos)
        {



            /*
             *  We have a special method in AStarPathFinding.cs that finds the nearest grid to a given position.
             * (e.g., if I give Vector3(1, 0, 5), it’ll try to find the nearest grid to that position using a
             * smart ray-casting algorithm)
 

                We can use that method now to find the Start & Destination grid.
                The start grid will be where the NPC is standing on, destination normally would be where the player is, 
                or what object the NPC is trying to seek. 

                We can safely assume that the Start Grid is the Current Grid. 
                We’ll also clear the open and close lists for preparation of finding the path to the destination. 
                Also, to check if the paths are reachable, if not, we shouldn’t even start path finding.
                 

                Open Grid List = Stores all the grids that have been explored (but not stepped into)
                Closed Grid List = Stores all the grids that have been stepped into (and explored before)
             */
            
            
  

            startingGrid = GetNearestGridToPosition(this.transform.position); // getting the startingGrid using the current position
            destinationGrid = GetNearestGridToPosition(pos); // getting the destination using the vector3 from the method parameter 


            if (destinationGrid == null || startingGrid == null) // make sure there's a destination.
            {
                return;
            }

            currentDestination = pos; 

            currentGrid = startingGrid; // the current grid is the starting grid, because... yeah.. we're starting here..

            
            CheckIfUnreachable();
            
            
            openGrids.Clear(); // reset grids
            closedGrids.Clear(); // reset grids
            
            closedGrids.Add(startingGrid); // we close the starting grid because we just explored it.
            

            // Since the starting grid is the first grid we step into, we put it into the closed grids list.
            
        
      



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




       
                    grid.CheckWalkable(); // check if that grid is walkable.
                    
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
                            // and cannot be edited by other AIs. (unique to this AI instance)
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


                            
                          

                            break;


                        }
                    }
               
                }
            }

           





        }
        
  
        
        private void StepLeastF() // step into the lowest F value grid
        {

  
            if (destinationGrid == null || currentGrid == null) // make sure that destination grid is set.
                return;


            cachedCurrentGrid = destinationGrid; // we just cache the destination grid as the current grid
            // so our clause is not affected below currentGrid.index != cachedCurrentGrid.index
            // as we know that the currentGrid index will never be the same as cachedCurrentGrid
            // until the end when we find the destination and currentGrid will then be the same.
            // this only matters for the first iteration below.

            
  


            
            
            while (currentGrid.index != destinationGrid.index) // loop until we step into the destination grid
            {
                //This keeps iterating until our current grid (stepped in grid) is the destination, 
                //or if it’s not changed in two iterations (using cachedCurrentGrid variable to sense
                //if it’s still the same from the previous iteration), meaning that the NPC
                //is unable to build a path due to obstacles blocking the destination and have no path to it. 



                
                // FINDING THE SMALLEST F OF THE NEAREST GRID WE JUST CHECKED AND CALCULATED.

         
                

                
         
                if (openGrids.Count > 0) // if least one open grid is added
                {
                
                 
             
                    openGrids = openGrids.OrderBy(n => n.f).ToList(); // order by smallest to biggest F costs

            
                    Grid lowestGrid = openGrids[0]; // after being sorted
                


                    
                    currentGrid = lowestGrid; // update the new current grid.

        
 
                    
                    closedGrids.Add(lowestGrid); // as we stepped into it, we can close it.

                    openGrids.Remove(lowestGrid); // remove from open grid as we have already stepped on it.
                    
                    
                
                }


                if (cachedCurrentGrid != null) // null check
                {
                    
                    if (currentGrid.index != cachedCurrentGrid.index) // if it enters the openGrids.Count > 0 
                    // clause, the currentGrid will change. If it changed, we know that it'll be different from the cachedCurrentGrid.
                    // if it's not the same, we'll update the cachedCurrentGrid to take the currentGrid.
                    // and for the next iteration, it goes into the openGrids.Count > 0 clause again
                    // and changes the currentGrid instance.
                    // 
                    {
                        
                        cachedCurrentGrid = currentGrid; // cache the current grid to make sure later there's a change in it.
                        // we dont want it looping at the same current grid and getting stuck because
                        // there's no path.
                    
                    }
                    else 
                    // so if it doesn't go into the openGrids.Count, we know that we have explored all possible
                    // open grids and have no route to the destination.
                    // so we can safely say that the previosuly cached current grid will be the same as the current grid 
                    // as there's no change (because did not go into the openGrids.Count clause.)
                    {
                        unreachableDestination = true;
                        break;
                        // safety measure so the while loop isn't infinite and crashes the project.
                    }
                }
        
                
                SearchPath(destinationGrid.transform.position);
                
       

           



            }
            
            
            
            if(currentGrid.index == destinationGrid.index) // if we stepped into the destination grid
            {

                FindGridPath(); // we can start to find the path to it.
                //if we manage to step into our destination grid with our current grid variable,
                //we then iterate the closed grid list and find the destination grid and save it. (#FindGridPath method)

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
            // we need to reverse so it starts from the start node to the end grid.
            // so now we have a viable path to the end grid.


            if (destinationGrids.Count > 1) // the index 0 is the starting point, we want the next which is where we should
            // move on to. that's why count > 1 because we need a size of 2 inside.
            {
                nextGridDestination = destinationGrids[1];
                // we jump to 1 because 0 is the starting point (where we are currently on)
                // so we want to go 1 grid forward.
                
                // for the movement, we only need to move 1 forward (from the starting point),
                // doing this will eventually get us to the destination as we constantly search for it.
                
                // as we search again later, we'll start from this point that we moved to,
                // and the next point will be the point we want to move to.
            }
 




     
    
            
            
            
            
            
    
        }




        private void Move()
        {
                     
        
            
            /*if (currentDestination != setDestination)
            {
                GetStartAndEndGrid(setDestination);
            }*/
            
            //StepLeastF();
  
      
            
 
            if (destinationGrids.Count > 0 && nextGridDestination != null) // have destinations to follow
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

       
                
                
                






                LookTowardsGrid(); // rotate towards the grid.

            }
 

            if (destinationGrid != null) // there must be a destination.
            {

                //Debug.Log(destinationGrid.index + " " + Vector3.Distance(transform.position, destinationGrid.transform.position));
                if (Vector3.Distance(transform.position, destinationGrid.transform.position) < 3) // if its this distance away from the destination, we start to slow down.
                {
                    animator.SetFloat("Forward", 0, 0.125f, Time.deltaTime); 
                    // slowly slow down using animation blend because the next grid is the destination.
                    //Debug.Log("STOP");
                }
                else
                {
                    animator.SetFloat("Forward", 1); // turn on the animation for walking
                }



                if (unreachableDestination) // if while stepping f and realised that the destination is actually unreachable too, maybe surrounded by unwalkable grids
                {
                    animator.SetFloat("Forward", 0);  // stop walking.
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

            if (targetDirection != Vector3.zero)
            {
                Quaternion rotation =
                    Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5);
            }
            





    
            

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