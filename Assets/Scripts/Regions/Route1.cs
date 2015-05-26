using UnityEngine;
using System.Collections;

public class Route1 : MonoBehaviour 
{
	public AllData allData;

	// Use this for initialization
	void Start () 
	{
		allData = GameObject.Find ("GameData").GetComponent<AllData> ();

		if(allData.quests[4].isComplete)
		{
			GameObject go = GameObject.Find ("Traveling Merchant");
			CharacterGameObject cgo = go.GetComponent<CharacterGameObject>();

			Vector3 pos = go.transform.position;

			int originX = cgo.row;
			int originY = cgo.col;

			int newX = cgo.row+1;
			int newY = cgo.col;

			Cell originCell = GameObject.Find ("Cell_"+originX+"_"+originY).GetComponent<Cell>();
			Cell newCell = GameObject.Find ("Cell_"+newX+"_"+newY).GetComponent<Cell>();

			originCell.isWalkable = true;
			originCell.hasNPC = false;
			originCell.NPC = null;
			newCell.isWalkable = false;
			newCell.hasNPC = true;
			newCell.NPC = go;

			go.transform.position = new Vector3((float)newY, (float)newX*-1);

			cgo.row = (int) newX;
			cgo.col = (int) newY;
		}
	}
}
