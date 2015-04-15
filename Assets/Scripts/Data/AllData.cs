using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AllData : MonoBehaviour 
{
	Calendar cal;

	public CreateCharacterClass createCharacterClass;
	public CreateCharacter createCharacter;
	public CreateCharacterGameObject createCharacterGameObject;
	public CreateCells createCells;
	public Pathfinder pathfinder;
	public CreateBattle createBattle;
	public RunBattle runBattle;
	public ReadCSV readCSV;
	public ReadCellData readCellData;
	public bool startRunningBattle = false;

	public Font arial;

	#region DataFolders
	public GameObject characterClassFolder;
	public GameObject characterFolder;
	public GameObject characterGameObjectFolder;
	public GameObject cellFolder;
	public GameObject canvas;
	#endregion DataFolders

	#region colors
	public Color32 c_White;
	public Color32 c_White_0pct;
	public Color32 c_White_25pct;
	public Color32 c_White_50pct;
	public Color32 c_White_75pct;
	public Color32 c_Gray;
	public Color32 c_Gray_50pct;
	#endregion colors
	
	public List<CharacterClass> characterClasses = new List<CharacterClass>();
	public List<Character> characters = new List<Character>();
	public List<GameObject> characterGameObjects = new List<GameObject>();
	public GameObject[,] cells = new GameObject[0,0];
	
	public enum gameState
	{
		Battle
		,Overworld
	}

	public gameState currentState;

	// Use this for initialization
	void Start ()
	{
		currentState = gameState.Overworld;

		getComponents();
		createFolders();
		loadData ();
	}

	public void createFolders()
	{
		characterClassFolder = new GameObject();
		characterClassFolder.name = "Character Class Folder";
		characterClassFolder.transform.parent = transform;

		characterFolder = new GameObject();
		characterFolder.name = "Character Folder";
		characterFolder.transform.parent = transform;

		characterGameObjectFolder = new GameObject();
		characterGameObjectFolder.name = "Character GameObject Folder";
		characterGameObjectFolder.transform.parent = canvas.transform;

		cellFolder = new GameObject();
		cellFolder.name = "Cell Folder";
		cellFolder.transform.parent = canvas.transform;
	}

	public void getComponents()
	{
		cal = GameObject.Find("GameData").GetComponent<Calendar>();
		canvas = GameObject.Find ("Canvas");

		readCSV = gameObject.AddComponent<ReadCSV> ();
		readCellData = gameObject.AddComponent<ReadCellData> ();

		createCharacterClass = gameObject.AddComponent<CreateCharacterClass>();
		createCharacterClass.allData = this;

		createCharacter = gameObject.AddComponent<CreateCharacter>();
		createCharacter.allData = this;

		createCharacterGameObject = gameObject.AddComponent<CreateCharacterGameObject>();
		createCharacterGameObject.allData = this;

		createCells = gameObject.AddComponent<CreateCells>();
		createCells.allData = this;

		pathfinder = gameObject.AddComponent<Pathfinder>();
		pathfinder.GetComponent<Pathfinder> ().allData = this;

		createBattle = gameObject.AddComponent<CreateBattle>();
		createBattle.allData = this;
		createBattle.numOfDiceSides = 10000;
		createBattle.pathfinder = pathfinder;

		runBattle = gameObject.AddComponent<RunBattle>();
		runBattle.allData = this;
		runBattle.cal = cal;
		runBattle.pathfinder = pathfinder;
		runBattle.currentBattleState = RunBattle.battleStateType.GetTarget;
	}

	public void loadData()
	{
		switch(currentState)
		{
		case gameState.Battle:
		{
			loadClasses();
			loadCharacters();
			loadCells("Test Level");
			loadCharacterGameObjects_Manual();
			loadBattle();
			break;
		}
		case gameState.Overworld:
		{
			loadClasses();
			loadCharacters();
			loadCells("Test Level");

			break;
		}
		default:
		{
			break;
		}
		}
	}

	public void loadClasses()
	{
		/*
		 * GameObject folder
		 * string className
		 * int classID
		 * int moveSpeed
		 * int attack
		 * int defense
		 * int totalHealth
		 * int attackRange
		 */

		Debug.Log ("Creating Character Classes manually - at some point will load this information from spreadsheet and loop over it.");
		createCharacterClass.createCharacterClass
		(
			characterClassFolder
			,"Infantry"
			,0
			,3
			,2
			,2
			,10
			,1
		);
		
		createCharacterClass.createCharacterClass
		(
			characterClassFolder
			,"Archer"
			,0
			,3
			,5
			,1
			,10
			,3
		);
	}

	public void loadCharacters()
	{
		/*
		 * GameObject folder
		 * string characterName
		 * int characterID
		 * CharacterClass characterClass
		 */

		Debug.Log ("Creating Characters manually - at some point will create randomly.");
		createCharacter.createCharacter
		(
			characterFolder
			,"John"
			,0
			,characterClasses[0]
		);
		
		createCharacter.createCharacter
		(
			characterFolder
			,"George"
			,1
			,characterClasses[1]
		);

		createCharacter.createCharacter
		(
			characterFolder
			,"Ephram"
			,2
			,characterClasses[1]
		);
	}

	public void loadCharacterGameObjects_Manual()
	{
		/*
		 * Calendar cal
		 * GameObject folder
		 * Vector3 pos
		 * Character character
		 * string factionTag
		 * string enemyFactionTag
		 */

		Debug.Log ("Creating Character GameObjects manually - at some point will create randomly.");

		createCharacterGameObject.createCharacterGameObject
		(
			cal
			,characterGameObjectFolder
			,cells[0,0].transform.position
			,characters[0]
			,"Faction - Side 1"
			,"Faction - Side 2"
			,false
		);

		createCharacterGameObject.createCharacterGameObject
		(
			cal
			,characterGameObjectFolder
			,cells[3,6].transform.position
			,characters[1]
			,"Faction - Side 2"
			,"Faction - Side 1"
			,false
		);

		createCharacterGameObject.createCharacterGameObject
		(
			cal
			,characterGameObjectFolder
			,cells[3,3].transform.position
			,characters[2]
			,"Faction - Side 2"
			,"Faction - Side 1"
			,false
		);
	}

	public void loadCells(string currentLocation)
	{
		Debug.Log ("Creating cells. Will need to update this to load from resource csv file");

		string[,] grid = readCSV.getMultiDimCSVData ("./Assets/Resources/CSV/" + currentLocation + ".csv");
		cells = new GameObject[grid.GetLength(0),grid.GetLength(1)];

		/*
		 * Calendar cal
		 * GameObject folder
		 * string[] grid
		 */
		
		createCells.createCells
		(
			cal
			,cellFolder
			,grid
			,new Vector2(0,0)
			,c_White_25pct
			,c_White_50pct
			,c_White_75pct
			,c_White_50pct
			,arial
			,"test"
			,readCellData
			,createCharacterGameObject
		);

		createCells.addNeighborsToCells();
	}

	public void loadBattle()
	{
		createBattle.calculateBattleOrder
		(
			characterGameObjects
		);

	}
}
