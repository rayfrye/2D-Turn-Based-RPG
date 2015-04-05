using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour 
{
	public AllData allData;

	public List<GameObject> path
	(
		GameObject start
		,GameObject end
		,GameObject[,] grid
	)
	{
		allData.createCells.clearGrid ();

		List<GameObject> path = new List<GameObject>();

		Cell startCell = start.GetComponent<Cell>();
		Cell endCell = end.GetComponent<Cell>();
		
		bool success = search (startCell, endCell);

		if(success)
		{
			Cell cell = endCell;

			while(cell.parentCell != null)
			{
				path.Add (cell.gameObject);
				cell = cell.parentCell;
			}

			path.Reverse ();
		}
		
		return path;
	}

	bool search(Cell currentCell, Cell endCell)
	{
		currentCell.openclosedstate = "closed";
		List<Cell> nextCells = GetAdjacentWalkableCells(currentCell, endCell);
		nextCells.Sort ((cell1,cell2) => cell1.fScore ().CompareTo(cell2.fScore()));
		
		foreach(Cell nextCell in nextCells)
		{
			if(nextCell == endCell)
			{
				return true;
			}
			else
			{
				if(search (nextCell,endCell))
				{
					return true;
				}
			}
		}
		
		return false;
	}


	List<Cell> GetAdjacentWalkableCells(Cell fromCell, Cell endCell)
	{
		List<Cell> walkableNodes = new List<Cell>();

		foreach(Cell cell in fromCell.neighborCells)
		{
			if((cell.isWalkable || cell == endCell) && cell.openclosedstate != "closed")
			{
				if(cell.openclosedstate == "open")
				{
					float traversalCost = calcDistance(cell, cell.parentCell);
					float gTemp = fromCell.gScore + traversalCost;
					if (gTemp < cell.gScore)
					{
						cell.parentCell = fromCell;
						walkableNodes.Add(cell);
						cell.hScore = calcDistance(cell.parentCell,cell);
						cell.gScore = calcDistance(cell,endCell);
					}
				}
				else
				{
					cell.parentCell = fromCell;
					cell.openclosedstate = "open";
					walkableNodes.Add (cell);
					cell.hScore = calcDistance(cell.parentCell,cell);
					cell.gScore = calcDistance(cell,endCell);
				}
			}
		}
		
		return walkableNodes;
	}

	public int calcDistance(Cell startCell, Cell endCell)
	{
		int rowDiff = Mathf.Abs(startCell.row - endCell.row);
		int colDiff = Mathf.Abs (startCell.col - endCell.col);

		return rowDiff + colDiff;
	}

	public int calcDistance(CharacterGameObject startCharacter, CharacterGameObject endCharacter)
	{
		int rowDiff = Mathf.Abs(startCharacter.row - endCharacter.row);
		int colDiff = Mathf.Abs (startCharacter.col - endCharacter.col);
		
		return rowDiff + colDiff;
	}

}
