using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Cell : MonoBehaviour 
{
	public int id;
	public string cellName;
	public int row;
	public int col;

	public bool isDoor;
	public string doorLevel;
	public int doorNum;

	public bool isSpawnPoint;
	public int spawnPointNum;

	#region pathfinding
	public int gScore;
	public int hScore;
	public int fScore()
	{
		return gScore + hScore;
	}
	public string openclosedstate;
	public Cell parentCell;
	public bool isWalkable;
	#endregion pathfinding

	public List<Cell> neighborCells = new List<Cell>();

	public List<string> dialogue = new List<string>();
	public bool hasNPC;
	public GameObject NPC;
	public bool hasDialogue;

}
