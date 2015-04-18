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
		,ReadCellData readCellData
		,CreateCharacterGameObject createCharacterGameObject
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
				bool isWalkable = true;

				if(readCellData.getCellValue(grid[row,col],"isWalkable") == "false")
				{
					isWalkable = false;
				}

				setupCell (newCell, row, col, isWalkable);
				setupRectTransform (newCell, row, col, startPoint);
				setupCanvasRenderer(newCell);
				setupButton (newCell,c_Normal,c_Highlighted,c_Pressed,c_Disabled);
				applyImageLayers(newCell, grid[row,col],10, readCellData);
				setupCharacter
				(
					grid[row,col]
					, readCellData
					, createCharacterGameObject
					, cal
					, allData
					, newCell.transform.position
					, row
					, col
				);


				setupTextGameObject(newCell,text, font);

				allData.cells[row,col] = newCell;
			}
		}
	}

	public void setupCharacter
	(
		string cellValue
		, ReadCellData readCellData
		, CreateCharacterGameObject createCharacterGameObject
		, Calendar cal
		, AllData allData
		, Vector3 pos
		, int row
		, int col
	)
	{
		if (cellValue.Contains ("characterID"))
		{
			string factionTag = readCellData.getCellValue(cellValue,"characterFactionTag");
			string enemyFactionTag = readCellData.getCellValue(cellValue,"characterEnemyFactionTag");
			int characterID = int.Parse (enemyFactionTag = readCellData.getCellValue(cellValue,"characterID"));

			if(readCellData.getCellValue(cellValue,"characterIsPlayer") == "True")
			{
				allData.player = createCharacterGameObject.createCharacterGameObject
				(
					cal
					,allData.characterGameObjectFolder
					,pos
					,allData.characters[characterID]
					,"Player"
					,enemyFactionTag
					,true
					,row
					,col
				);

				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCamera2D>().target = allData.player.transform;
			}
			else
			{
				createCharacterGameObject.createCharacterGameObject
				(
					cal
					,allData.characterGameObjectFolder
					,pos
					,allData.characters[characterID]
					,factionTag
					,enemyFactionTag
					,false
					,row
					,col
				);
			}
		}
	}

	public void applyImageLayers(GameObject newCell, string cellValue,int numOfLayers, ReadCellData readCellData)
	{
		for(int imageLayer = 0; imageLayer < numOfLayers; imageLayer ++)
		{
			if(cellValue.Contains ("imageLayer_" + imageLayer))
			{
				int imageLayerIndex = 0;

				if(cellValue.Contains ("imageLayer_" + imageLayer + "_index"))
				{
					imageLayerIndex = int.Parse (readCellData.getCellValue (cellValue, "imageLayer_" + imageLayer + "_index"));
				}

				setupImage(newCell, readCellData.getCellValue(cellValue,"imageLayer_" + imageLayer), imageLayer, imageLayerIndex);
			}
		}
	}

	public void setupCell(GameObject cell, int row, int col, bool isWalkable)
	{
		Cell newCell = cell.AddComponent<Cell> ();

		newCell.row = row;
		newCell.col = col;
		newCell.gScore = 0;
		newCell.hScore = 0;
		newCell.openclosedstate = "untested";
		newCell.parentCell = null;
		newCell.isWalkable = isWalkable;
		newCell.cellName = cell.name;
	}

	public void setupRectTransform(GameObject cell, int row, int col, Vector2 startPoint)
	{
		RectTransform rectTransform =  cell.AddComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(1,1);
		rectTransform.position = new Vector3(startPoint.y + col, startPoint.x - row,0);
	}

	public void setupCanvasRenderer(GameObject cell)
	{
		CanvasRenderer canvasRanderer = cell.AddComponent<CanvasRenderer> ();
	}

	public void setupImage(GameObject cell, string imageFile, int sortingOrder, int imageLayerIndex)
	{
		GameObject imageGameObject = new GameObject ();
		imageGameObject.transform.parent = cell.transform;
		imageGameObject.name = cell.name+"_ImageLayer_" + sortingOrder;
		imageGameObject.tag = "Image";

		SpriteRenderer spriteRenderer = imageGameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = Resources.LoadAll<Sprite> (imageFile)[imageLayerIndex];
		spriteRenderer.sortingOrder = sortingOrder;

		setupTextRectTransform (imageGameObject, 50, 1);
	}

	public void setupButton
	(
		GameObject cell
		,Color32 c_Normal
		,Color32 c_Highlighted
		,Color32 c_Pressed
		,Color32 c_Disabled
	)
	{
		Button button = cell.AddComponent<Button> ();

		ColorBlock button_ColorBlock = new ColorBlock();
		button_ColorBlock.normalColor = c_Normal;
		button_ColorBlock.highlightedColor = c_Highlighted;
		button_ColorBlock.pressedColor = c_Pressed;
		button_ColorBlock.disabledColor = c_Disabled;
		button_ColorBlock.colorMultiplier = 1;
		button_ColorBlock.fadeDuration = .1f;
		
		button.colors = button_ColorBlock;
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

	public void setupTextGameObject(GameObject cell, string textString, Font font)
	{
		GameObject newText = new GameObject ();
		newText.transform.parent = cell.transform;
		newText.name = cell.name+"_Text";
		newText.tag = "Text";

		setupTextRectTransform (newText, 50f, .02f);
		setupTextCanvasRenderer (newText);
		setupText (newText, textString, font);
	}

	public void setupTextRectTransform(GameObject textGameObject, float size, float scale)
	{
		RectTransform rectTransform = textGameObject.AddComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(size,size);
		rectTransform.localScale = new Vector2(scale,scale);
		rectTransform.localPosition = new Vector3(0,0,0);
	}

	public void setupTextCanvasRenderer(GameObject textGameObject)
	{
		CanvasRenderer canvasRenderer = textGameObject.AddComponent<CanvasRenderer>();
	}

	public void setupText(GameObject textGameObject, string textString, Font font)
	{
		Text text = textGameObject.AddComponent<Text>();

		text.font = font;
		text.text = textString;
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
