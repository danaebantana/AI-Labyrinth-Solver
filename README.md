# Labyrinth-Solver
Finding the shortest path in a labyrinth using the «BFS-Branch&amp;Bound» algorithm.
To run project download "App" and run "Assignment AI-EE 2020.exe".

# Basic Structures
1. A Two-dimensional 6x5 int array "labyrinth". 1 is for a white Labyrinth cell and 0 is for a black Labyrinth cell.

2. Structure "Node"
	
	X : describes the row of the cell
	
	Y : describes the column of the cell
	
	cost : describes the cost, i.e. the steps that the "branch & bound" algorithm needs to do from the "I" start point to the respective cell. The initial node "I" is not included in the cost calculation.
	
	path : describes the path followed by the "branch & bound" algorithm to reach each cell, along with the identity of the cell itself.

3. Search front «frontSearchQueue» : The search front is a queue of states that have already been visited but have not yet been expanded. The first element to be extended is extracted each time from the front of the queue. Due to "BFS", the children-states are placed at the back of the queue.

4. Closed set «closedSetList» : It constitutes the sum of all states that have already been extended. They are added to the list so that they are not placed again on the search front and re-examined, as the outcome will remain the same.

5. Variable «upperBound» : The variable "upperBound" is the upper limit by which the states are cut/not included and corresponds to the minimum cost of the current shortest path each time. When a path reaches the final "G" node, it is revised. If the cost of the new path is less than the value of the current "upperBound", the "upperBound" changes and gets this new value.

6. node «final_node» : Includes the shortest path and its cost, after the completion of the "BFS-Branch&Bound" algorithm.
