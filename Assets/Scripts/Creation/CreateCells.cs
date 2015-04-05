using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateCells : MonoBehaviour 
{
	public AllData allData;

	public void createCells
	(
		Calendar cal
		,GameObject folder
		,string[,] grid
		,Vector2 startPoint	
		,Color32 c_Normal
		,Color32 c_Highlighted
		,Color32 c_Pressed
		,Color32 c_Disabled
		,Font font
		,string text
	)
	{
		for(int row = 0; row < grid.GetLength (0); row++)
		{
			for(int col = 0; col < grid.GetLength (1); col++)
			{
				GameObject newCell = new GameObject();
				newCell.transform.parent = folder.transform;
				newCell.name = "Cell_"+row+"_"+col;
				newCell.tag = "Cell";

				newCell.AddComponent<Cell>();
				newCell.GetComponent<Cell>().row = row;
				newCell.GetComponent<Cell>().col = col;
				newCell.GetComponent<Cell>().gScore = 0;
				newCell.GetComponent<Cell>().hScore = 0;
				newCell.GetComponent<Cell>().openclosedstate = "untested";
				newCell.GetComponent<Cell>().parentCell = null;
				newCell.GetComponent<Cell>().isWalkable = true;
			
				newCell.AddComponent<RectTransform>();
				newCell.GetComponent<RectTransform>().sizeDelta = new Vector2(1,1);
				newCell.GetComponent<RectTransform>().position = new Vector3(startPoint.y + col, startPoint.x - row,0);

				newCell.AddComponent<CanvasRenderer>();

				newCell.AddComponent<Image>();
				newCell.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI Buttons_0");

				allData.cells[row,col] = newCell;

				ColorBlock button_ColorBlock = new ColorBlock();
				button_ColorBlock.normalColor = c_Normal;
				button_ColorBlock.highlightedColor = c_Highlighted;
				button_ColorBlock.pressedColor = c_Pressed;
				button_ColorBlock.disabledColor = c_Disabled;
				button_ColorBlock.colorMultiplier = 1;
				button_ColorBlock.fadeDuration = .1f;

				newCell.AddComponent<Button>();
				newCell.GetComponent<Button>().colors = button_ColorBlock;

				GameObject newText = new GameObject();
				newText.transform.parent = newCell.transform;
				newText.name = newCell.name+"_Text";
				newText.tag = "Text";

				newText.AddComponent<RectTransform>();
				newText.GetComponent<RectTransform>().sizeDelta = new Vector2(50,50);
				newText.GetComponent<RectTransform>().localScale = new Vector2(.02f,.02f);
				newText.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);

				newText.AddComponent<CanvasRenderer>();

				newText.AddComponent<Text>();
				newText.GetComponent<Text>().font = font;
				newText.GetComponent<Text>().text = text;
			}
		}
	}

	public void addNeighborsToCells()
	{
		GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");

		foreach(GameObject cell in cells)
		{
			int row = cell.GetComponent<Cell>().row;
			int col = cell.GetComponent<Cell>().col;

			List<GameObject> neighborsToTest = new List<GameObject>();
			neighborsToTest.Add (GameObject.Find ("Cell_"+ (row+1) + "_" + col));
			neighborsToTest.Add (GameObject.Find ("Cell_"+ (row+-1) + "_" + col));
			neighborsToTest.Add (GameObject.Find ("Cell_"+ (row) + "_" + (col+1)));
			neighborsToTest.Add (GameObject.Find ("Cell_"+ (row) + "_" + (col-1)));
			
			foreach(GameObject neighborToTest in neighborsToTest)
			{
				if(neighborToTest != null)
				{
					cell.GetComponent<Cell>().neighborCells.Add (neighborToTest.GetComponent<Cell>());
				}
			}
		}
	}

	public void clearGrid()
	{
		GameObject[] cellGameobjects = GameObject.FindGameObjectsWithTag("Cell");
		
		foreach(GameObject cellGameObject in cellGameobjects)
		{
			Cell cell = cellGameObject.GetComponent<Cell>();

			cell.gScore = 0;
			cell.hScore = 0;
			cell.openclosedstate = "untested";
			cell.parentCell = null;
		}
	}
}
