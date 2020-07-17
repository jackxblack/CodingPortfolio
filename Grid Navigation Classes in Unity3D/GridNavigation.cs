using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridNavigation
{   
    //This method is supposed to provide any character with coordinates to the next step
    //The method is based on Breath First Search method
    //In short: it puts a node into the queue and starts a loop. It then dequeues a node from the queue, check it's neighbours and puts it into the queue. Then it marks the node as visited, to ignore it in the future.
    //The node that followed the node is added to list of previous nodes. Each node on the grid has its entry on that list.
    //In following loop iterations it then dequeues subsequent nodes and repeats the steps.
    //Once node that's supposed to be the end of the path is reached, the loop is stopped.
    //Then, based on the previous list, the path is recreated, from the end
    //Last node of the path is then provided back to the asking character.

    public static GridField GetNextStep(GridField startField, GridField targetField, GridScript grid)
    {
        #region properties
        Queue<GridField> queue = new Queue<GridField>();          //The queue that's the base of the first part of the method.
        List<GridField> visited = new List<GridField>();          //List of nodes that were already handled.
        List<GridField> previous = new List<GridField>();         //List in which a node prior for each node can be found. For example node previous to node "1,0" can be found in this list's index 10.
        List<GridField> path = new List<GridField>();             //This is the path to the final node. Since it's always writen backwards, ending node is already added to it. Subsequent nodes are separated with a ;. It's important that after split(), last element will be empty, so the actual first step is 2nd to last.

        GridScript gameGrid = grid;                         //Grid is necessary to calculate surrounding nodes and to get field information.                

        float stime = Time.realtimeSinceStartup;
        queue.Enqueue(startField);     //Enqueueing the 1st node - start - to kickstart the loop.
        visited.Add(startField);                                     //s node is immediately marked as visited.
        #endregion

        //This loop adds initially empty strings to 'previous' list, for each node on the grid.
        #region fill 'previous' list with empty strings
        for (int i = 0; i < (gameGrid.rows * gameGrid.columns); i++)
        {
            previous.Add(null);
        }
        #endregion

        //This loop is ultimately responssible for filling the 'previous' list with proper data, up to the end node.
        #region 1st loop filling in previous list
        int loop1c = 0;                                     //This is just a safety check. If loop takes too many iterations, we break out of it, based on this value.
        while (queue.Count > 0)
        {
            loop1c++;
            bool foundEnd = false;                                                      //This is set to true when found neighbour turns out to be the ending node.
            GridField field = queue.Dequeue();                                          //Dequeueing node from queue. Initially it's the 's' node.
            List<string> nodesNeighbours = GetSurroundingNodes(field, gameGrid);        //Getting surrounding nodes for currently processed node.
            foreach (string z in nodesNeighbours)                                       //Loop within a loop could be dangerous. Luckily, it's 4 iterations at most, in theory.
            {
                if (z == targetField.stringRepresentation)                              //Handling case in which found neighbour turns out to bbe the ending node
                {
                    string[] split = z.Split(',');                                      //Temprorary solution since node handling is a mix of processing a string and two ints. To be changed to a unified format.
                    int x = int.Parse(split[0]); int y = int.Parse(split[1]);
                    previous[int.Parse(x.ToString() + y.ToString())] = field;           //Replacing empty string in 'previous' for the found NEIGHBOUR list with currently handled (dequeued) node.
                    foundEnd = true;                                                    //Finding the ending node ends the loop.
                    break;
                }
                if (!grid.isFieldWalkable(z))                                           //If field is not walkable, we simply don't process it.
                {
                    visited.Add(grid.GetFieldByCoord(z));
                }
                if (!visited.Contains(grid.GetFieldByCoord(z)))                         //This part of the loop only executes if found neighbour wasn't already processed.
                {
                    queue.Enqueue(grid.GetFieldByCoord(z));                             //Each found neighbour is put to the queue.
                    visited.Add(grid.GetFieldByCoord(z));                               //It's also marked as visited, to not revisit it in the future.

                    string[] split = z.Split(',');
                    int x = int.Parse(split[0]); int y = int.Parse(split[1]);           //Temprorary solution since node handling is a mix of processing a string and two ints. To be changed to a unified format.                                        
                    previous[int.Parse(x.ToString() + y.ToString())] = field;           //Replacing empty string in 'previous' for the found NEIGHBOUR list with currently handled (dequeued) node.
                }
            }
            if (foundEnd)                                                               //If we've found the end node, not only foreach (inner) loop is ended, but the whole while (outer) loop as well.
                break;
            if (loop1c > 100)                                                           //If the loop has reached 100 iterations we crash.
            {
                throw new Exception("Breaking out of 1st loop of GetNextStep() method as it reached more than 100 iterations, suggesting an infinite loop.");
                break; //Probably unnecessary.
            }
        }
        #endregion

        //This loop recreates the path based on 'previous' list and puts it to a string.
        #region 2nd loop creating path string
        GridField previousField = previous[int.Parse(targetField.intRepresentationX.ToString() + targetField.intRepresentationY.ToString())]; //previousNode represents the previous node of currently handled node. Since we have to start somewhere, we're initially assigning it to the end node.
        int loop2c = 0;                                                                             //As with the first loop, we also break out of this one if it reaches 100 iterations.
        while (true)
        {
            loop2c++;            
            if (previous[int.Parse(previousField.intRepresentationX.ToString() + previousField.intRepresentationY.ToString())] != startField)                 //Unless the previousNode is the starting node, we process it.
            {
                path.Add(previous[int.Parse(previousField.intRepresentationX.ToString() + previousField.intRepresentationY.ToString())]);                     //We add the the node that's in the 'previous' list in the index reserved for previousNode, so we add previous node of previousNode. Previousception.
                previousField = previous[int.Parse(previousField.intRepresentationX.ToString() + previousField.intRepresentationY.ToString())];               //Then we replace previousNode with previous node of current previousNode. Names are stupid.
            }
            else                                                                                    //If previous node turns out to be starting node, we break out of the loop.
            {
                break;
            }
            if (loop2c > 100)                                                                       //If the loop has reached 100 iterations, we crash.
            {
                throw new Exception("Breaking out of 2nd loop of GetNextStep() method as it reached more than 100 iterations, suggesting an infinite loop.");
                break;
            }
        }
        #endregion

        float etime = Time.realtimeSinceStartup;
        Debug.Log("Calculating path fromm " + startField.stringRepresentation + " to " + targetField.stringRepresentation + " took " + ((etime - stime) * 1000).ToString() + "ms.");
        return path[path.Count - 1];     //
    }

    //This method returns a list of neighbours of provided node, based on the grid size.

    public static List<string> GetSurroundingNodes(GridField field, GridScript grid)
    {
        #region properties
        string[] splitCoord = field.stringRepresentation.Split(',');
        int x = int.Parse(splitCoord[0]); int y = int.Parse(splitCoord[1]);
        bool isBottom = false; bool isTop = false; bool isLeft = false; bool isRight = false;   //Border bools, intially false.
        List<string> surroundingNodes = new List<string>();                                     //List to be returned.
        #endregion

        //Check if either x or y coordinate is edge of the grid and mark proper bool to true.
        #region borderChecks
        if (x == 0)                         //If x is 0, then field must be on left edge.
        {
            isLeft = true;
        }
        else if (x == (grid.rows - 1))      //if x is (grid.rows-1), then field must be on right edge.
        {
            isRight = true;
        }
        if (y == 0)                         //If y is 0, then field must be on bottom edge.
        {
            isBottom = true;
        }
        else if (y == (grid.columns - 1))   //if y is (grid.columns-1), then field must be on top edge.
        {
            isTop = true;
        }
        #endregion

        //Depending of border cases add either two or one neighbours for each axis
        #region fill in the list
        if (!isBottom)
            surroundingNodes.Add(x.ToString() + "," + (y - 1).ToString());        
        if(!isLeft)
            surroundingNodes.Add((x - 1).ToString() + "," + y.ToString());
        if (!isTop)
            surroundingNodes.Add(x.ToString() + "," + (y + 1).ToString());
        if (!isRight)
            surroundingNodes.Add((x + 1).ToString() + "," + y.ToString());
        #endregion

        return surroundingNodes;
    }
}
