using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class AllData : MonoBehaviour 
{
	#region creation
	public CreateCharacterClass createCharacterClass;
	public CreateCharacter createCharacter;
	public CreateCharacterGameObject createCharacterGameObject;
	public CreateDialogue createDialogue;
	public CreateQuest createQuest;
	public CreatePlayerData createPlayerData;
	public CreateItems createItems;
	public CreateCells createCells;
	public CreateTurnBasedBattle createTurnBasedBattle;
	public CreateBattle createBattle;
	public CreateAbility createAbility;
	public MainMenu mainMenu;

	#endregion creation

	#region gameplay
	public RunBattle runBattle;
	public RunOverworld runOverworld;
	public RunTurnBasedBattle runTurnBasedBattle;

	public bool finishedLoading = false;
	public GameObject player;

	#endregion gameplay

	#region data
	public PermanentData permanentData;
	public Calendar cal;
	public SaveData saveData;
	public string currentLevel;
	public int currentDoorNum ;
	
	public int playerCharacterID;
	#endregion data

	#region functions
	public ReadCSV readCSV;
	public ReadCellData readCellData;
	public Pathfinder pathfinder;
	public ConvertData convertData;

	#endregion functions


	#region DataFolders
	public GameObject gameDataFolder;
	public GameObject characterClassFolder;
	public GameObject characterFolder;
	public GameObject characterGameObjectFolder;
	public GameObject cellFolder;
	public GameObject dialogueFolder;
	public GameObject questFolder;
	public GameObject canvas;
	public GameObject playerDataFolder;
	public GameObject itemFolder;
	public GameObject abilityFolder;
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

	#region UI
	public Font arial;
	#endregion UI

	public List<CharacterClass> characterClasses = new List<CharacterClass>();
	public List<Character> characters = new List<Character>();
	public List<GameObject> characterGameObjects = new List<GameObject>();
	public List<Dialogue> dialogues = new List<Dialogue> ();
	public List<Quest> quests = new List<Quest>();
	public GameObject[,] cells = new GameObject[0,0];
	public PlayerData playerData;
	public List<Item> items = new List<Item>();
	public List<Ability> abilities = new List<Ability>();
	
	public enum gameState
	{
		Battle
		,Overworld
		,Turn_Based_Battle
		,MainMenu
	}

	public gameState currentState;

	// Use this for initialization
	void Start ()
	{
		getComponents();
		createPermanentData ();
		createFolders();
		loadData ();
	}

	public gameState savedState()
	{
		switch (readCSV.getMultiDimCSVData ("./Assets/Resources/CSV/Save Data/playerData.csv")[0,4])
		{
		case ("Battle"):
		{
			return gameState.Battle;
			break;
		}
		case ("Overworld"):
		{
			return gameState.Overworld;
			break;
		}
		case ("Turn_Based_Battle"):
		{
			return gameState.Turn_Based_Battle;
			break;
		}
		default:
		{
			return gameState.Overworld;
			break;
		}
		}
	}

	public void createPermanentData()
	{
		GameObject tempPermanentData = GameObject.Find ("PermanentData");

		if (tempPermanentData == null) 
		{
			currentState = gameState.MainMenu;//savedState ();
			GameObject newPermanentData = new GameObject ();
			newPermanentData.name = "PermanentData";
			PermanentData permanentDataScript = newPermanentData.AddComponent<PermanentData> ();
			permanentDataScript.currentDoorNum = 0;
			permanentDataScript.currentLevel = readCSV.getMultiDimCSVData ("./Assets/Resources/CSV/Save Data/playerData.csv") [0, 5];
			permanentDataScript.playerCharacterID = 0;
		} 
		else 
		{
			currentState = savedState ();
		}

		permanentData = GameObject.Find ("PermanentData").GetComponent<PermanentData> ();
		currentLevel = permanentData.currentLevel;
		currentDoorNum = permanentData.currentDoorNum;
		playerCharacterID = permanentData.playerCharacterID;
	}

	public void createFolders()
	{
		characterClassFolder = new GameObject();
		characterClassFolder.name = "Character Class Folder";
		characterClassFolder.transform.parent = gameDataFolder.transform;

		characterFolder = new GameObject();
		characterFolder.name = "Character Folder";
		characterFolder.transform.parent = gameDataFolder.transform;

		characterGameObjectFolder = new GameObject();
		characterGameObjectFolder.name = "Character GameObject Folder";
		characterGameObjectFolder.transform.parent = canvas.transform;

		cellFolder = new GameObject();
		cellFolder.name = "Cell Folder";
		cellFolder.transform.parent = canvas.transform;

		dialogueFolder = new GameObject ();
		dialogueFolder.name = "Dialogue Folder";
		dialogueFolder.transform.parent = gameDataFolder.transform;

		questFolder = new GameObject ();
		questFolder.name = "Quest Folder";
		questFolder.transform.parent = gameDataFolder.transform;

		playerDataFolder = new GameObject();
		playerDataFolder.name = "Player Data Folder";
		playerDataFolder.transform.parent = gameDataFolder.transform;

		itemFolder = new GameObject();
		itemFolder.name = "Item Folder";
		itemFolder.transform.parent = gameDataFolder.transform;

		abilityFolder = new GameObject ();
		abilityFolder.name = "Ability Folder";
		abilityFolder.transform.parent = gameDataFolder.transform;
	}

	public void getComponents()
	{
		gameDataFolder = GameObject.Find ("GameDataFolder");
	
		convertData = gameObject.AddComponent<ConvertData>();
		saveData = gameObject.AddComponent<SaveData>();

		createTurnBasedBattle = gameObject.AddComponent<CreateTurnBasedBattle>();
		
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

		createDialogue = gameObject.AddComponent<CreateDialogue> ();
		createDialogue.GetComponent<CreateDialogue>().allData = this;

		createQuest = gameObject.AddComponent<CreateQuest> ();
		createQuest.GetComponent<CreateQuest> ().allData = this;

		createPlayerData = gameObject.AddComponent<CreatePlayerData>();
		createPlayerData.GetComponent<CreatePlayerData>().allData = this;

		createItems = gameObject.AddComponent<CreateItems>();
		createItems.GetComponent<CreateItems>().allData = this;

		createAbility = gameObject.AddComponent<CreateAbility> ();
	}

	public void loadData()
	{
		switch(currentState)
		{
		case gameState.Battle:
		{
			loadGameData();
			loadCells(currentLevel);
			loadCharacterGameObjects_Manual();

			createBattle = gameObject.AddComponent<CreateBattle>();
			createBattle.allData = this;
			createBattle.numOfDiceSides = 10000;
			createBattle.pathfinder = pathfinder;
			loadBattle();

			runBattle = gameObject.AddComponent<RunBattle>();
			runBattle.allData = this;
			runBattle.cal = cal;
			runBattle.pathfinder = pathfinder;
			runBattle.currentBattleState = RunBattle.battleStateType.GetTarget;
			break;
		}
		case gameState.Overworld:
		{
			loadGameData();
			loadCells(currentLevel);
			loadPlayer();
			loadRegionScript();

			runOverworld = gameObject.AddComponent<RunOverworld> ();
			runOverworld.allData = this;
			runOverworld.cal = cal;
			runOverworld.currentState = RunOverworld.OverworldState.Idle;
			runOverworld.player = player;
			runOverworld.playerCharacterGameObject = player.GetComponent<CharacterGameObject>();
			runOverworld.permanentData = permanentData;
			runOverworld.currentDialogueIndex = 0;
			runOverworld.dialoguePanel = GameObject.Find ("DialoguePanel");
			runOverworld.dialoguePanel.SetActive (false);
			runOverworld.dialogueOptionButtons.Add(GameObject.Find ("DialogueButtonOption1"));
			runOverworld.dialogueOptionButtons.Add(GameObject.Find ("DialogueButtonOption2"));
			runOverworld.dialogueOptionButtons.Add(GameObject.Find ("DialogueButtonOption3"));
			runOverworld.dialogueOptionButtons[0].SetActive (false);
			runOverworld.dialogueOptionButtons[1].SetActive (false);
			runOverworld.dialogueOptionButtons[2].SetActive (false);
			runOverworld.dialogueOptionPanel = GameObject.Find ("DialogueOptionPanel");
			runOverworld.dialogueOptionPanel.SetActive (false);

			runOverworld.ItemCount = GameObject.Find ("ItemCount");
			runOverworld.ItemName = GameObject.Find ("ItemName");
			runOverworld.ItemDesc = GameObject.Find ("ItemDesc");
			runOverworld.InventoryCanvas = GameObject.Find("InventoryCanvas");
			runOverworld.InventoryList = GameObject.Find("InventoryList");
			runOverworld.InventoryListButtons = GameObject.Find ("InventoryListButtons");
			runOverworld.InventoryListButton = GameObject.Find ("InventoryListButton");
			runOverworld.InventoryCanvas.SetActive (false);
			runOverworld.eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem>();

			GameObject.Find ("BattleCanvas").SetActive(false);
			GameObject.Find ("MainMenuCanvas").SetActive(false);

			finishedLoading = true;
			break;
		}
		case gameState.Turn_Based_Battle:
		{
			loadGameData();

			loadCells("Wood Floor Arena");
			loadTurnBasedBattle();

			runTurnBasedBattle = gameObject.AddComponent<RunTurnBasedBattle>();
			runTurnBasedBattle.allData = this;
			runTurnBasedBattle.battleCanvas = GameObject.Find ("BattleCanvas");
			runTurnBasedBattle.battleOptionButtons.Add (GameObject.Find ("Attack"));
			runTurnBasedBattle.battleOptionButtons.Add (GameObject.Find ("Item"));
			runTurnBasedBattle.battleOptionButtons.Add (GameObject.Find ("Run"));
			runTurnBasedBattle.actionLog = GameObject.Find ("ActionLog");
			runTurnBasedBattle.characterNames.Add (GameObject.Find ("Character1Name"));
			runTurnBasedBattle.characterNames.Add (GameObject.Find ("Character2Name"));
			runTurnBasedBattle.characterNames.Add (GameObject.Find ("Character3Name"));
			runTurnBasedBattle.characterNames.Add (GameObject.Find ("Character4Name"));
			runTurnBasedBattle.characterHealths.Add (GameObject.Find ("Character1Health"));
			runTurnBasedBattle.characterHealths.Add (GameObject.Find ("Character2Health"));
			runTurnBasedBattle.characterHealths.Add (GameObject.Find ("Character3Health"));
			runTurnBasedBattle.characterHealths.Add (GameObject.Find ("Character4Health"));
			runTurnBasedBattle.abilityPanel = GameObject.Find ("AbilityPanel");
			runTurnBasedBattle.abilityButtons.Add (GameObject.Find ("Ability1"));
			runTurnBasedBattle.abilityButtons.Add (GameObject.Find ("Ability2"));
			runTurnBasedBattle.abilityButtons.Add (GameObject.Find ("Ability3"));
			runTurnBasedBattle.abilityButtons.Add (GameObject.Find ("Ability4"));
			runTurnBasedBattle.abilityPanel.SetActive(false);
			runTurnBasedBattle.eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem>();

			runTurnBasedBattle.setupCharacterNamesandHealths(this);

			GameObject.Find ("DialogueCanvas").SetActive (false);
			GameObject.Find("InventoryCanvas").SetActive (false);

			loadCharacterGameObjectsForTurnBasedBattle();

			break;
		}
		case gameState.MainMenu:
		{
			loadGameData();
			mainMenu = gameObject.AddComponent<MainMenu> ();

			mainMenu.allData = this;
			mainMenu.mainMenuCanvas = GameObject.Find ("MainMenuCanvas");
			mainMenu.mainMenuButtons.Add(GameObject.Find ("StartGame"));
			mainMenu.mainMenuButtons.Add(GameObject.Find ("LoadGame"));
			mainMenu.mainMenuButtons.Add(GameObject.Find ("Options"));
			mainMenu.mainMenuButtons.Add(GameObject.Find ("QuitGame"));
			mainMenu.mainMenuButtonPanel = GameObject.Find ("MenuButtons");
			mainMenu.mainMenuClassPanel = GameObject.Find ("ClassButtons");

			mainMenu.characterButtonPanel = GameObject.Find("CharacterButtons");
			mainMenu.characterButtons.Add (GameObject.Find ("Character1"));
			mainMenu.characterButtons.Add (GameObject.Find ("Character2"));
			mainMenu.characterButtons.Add (GameObject.Find ("Character3"));
			mainMenu.characterButtons.Add (GameObject.Find ("Character4"));

			mainMenu.characterNameInput = GameObject.Find ("CharacterNameInput");

			mainMenu.mainMenuClassButtons.Add (GameObject.Find ("Class1"));
			mainMenu.mainMenuClassButtons[0].transform.FindChild("Text").GetComponent<Text>().text = characterClasses[0].charClassName;

			for(int i = 1; i < characterClasses.Count; i++)
			{
				GameObject newButton = (GameObject) Instantiate(GameObject.Find ("Class1"));
				newButton.name = "Class"+i;
				mainMenu.mainMenuClassButtons.Add (newButton);
				mainMenu.mainMenuClassButtons[i].transform.FindChild("Text").GetComponent<Text>().text = characterClasses[i].charClassName;
				newButton.transform.parent = mainMenu.mainMenuClassPanel.transform;
				mainMenu.mainMenuClassButtons.Add (newButton);
			}

			mainMenu.characterNameInput.SetActive (false);
			mainMenu.characterButtonPanel.SetActive (false);
			mainMenu.mainMenuClassPanel.SetActive(false);

			mainMenu.eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem>();
			GameObject.Find ("BattleCanvas").SetActive (false);
			GameObject.Find ("DialogueCanvas").SetActive (false);
			GameObject.Find ("InventoryCanvas").SetActive (false);

			mainMenu.currentButton = 0;
			mainMenu.currentState = MainMenu.mainMenuState.MainMenu;

			finishedLoading = true;
			break;
		}
		default:
		{
			break;
		}
		}
	}

	public void loadTurnBasedBattle()
	{
		createTurnBasedBattle.createTurnBasedBattle
		(
			this
			,GameObject.Find ("Cell_4_6").transform
		);
	}

	public void loadGameData()
	{
		loadPlayerData();
		loadItems();
		loadClasses();
		loadDialogue();
		loadQuests();
		loadCharacters();
		loadAbilities ();
	}

	public void loadRegionScript()
	{
		gameObject.AddComponent(Type.GetType (permanentData.currentLevel.Replace (" ", "")));
	}

	public void loadAbilities()
	{
		string[,] tempAbilities = readCSV.getMultiDimCSVData("./Assets/Resources/CSV/GameData/abilities.csv");
		
		for(int row = 0; row < tempAbilities.GetLength (0); row ++)
		{
			createAbility.createAbility
			(
				this
				,abilityFolder
				,int.Parse (tempAbilities[row,0])
				,tempAbilities[row,1]
				,float.Parse(tempAbilities[row,2])
				,float.Parse (tempAbilities[row,3])
				,int.Parse (tempAbilities[row,4])
				,convertData.convertStringtoListInt(tempAbilities[row,5])
			);
		}
	}

	public void loadItems()
	{
		string[,] tempItems = readCSV.getMultiDimCSVData("./Assets/Resources/CSV/GameData/items.csv");
		
		for(int row = 0; row < tempItems.GetLength (0); row ++)
		{
			createItems.createItems
			(
				itemFolder
				,int.Parse (tempItems[row,0])
				,tempItems[row,1]
				,tempItems[row,2]
			);
		}
	}

	public void loadPlayerData()
	{
		string[,] tempPlayerData = readCSV.getMultiDimCSVData("./Assets/Resources/CSV/Save Data/playerData.csv");
		string[] tempPlayerPartyIDs = readCSV.getSingleDimCSVData("./Assets/Resources/CSV/Save Data/playerParty.csv");
		string[,] tempPlayerInv = readCSV.getMultiDimCSVData("./Assets/Resources/CSV/Save Data/playerItems.csv");

		Dictionary<int,int> inventoryItemIDs = new Dictionary<int,int>();

		for (int row = 0; row < tempPlayerInv.GetLength (0); row++) 
		{
			inventoryItemIDs.Add (int.Parse (tempPlayerInv[row,0]),int.Parse (tempPlayerInv[row,1]));
		}
		
		List<int> partyCharacterIDs = new List<int> ();
		
		for (int row = 0; row < tempPlayerPartyIDs.Length; row++) 
		{
			partyCharacterIDs.Add (int.Parse (tempPlayerPartyIDs[row]));
		}

		createPlayerData.createPlayerData
		(
			playerDataFolder
			,int.Parse (tempPlayerData[0,0])
			,partyCharacterIDs
			,inventoryItemIDs
		);
	}

	public void loadQuests()
	{
		string[,] tempQuests = readCSV.getMultiDimCSVData("./Assets/Resources/CSV/Save Data/quests.csv");
		
		for(int row = 0; row < tempQuests.GetLength (0); row ++)
		{
			createQuest.createQuest
			(
				questFolder
				,convertData.convertStringToInt(tempQuests[row,0])
				,tempQuests[row,1]
				,tempQuests[row,2]
				,convertData.convertStringToBool(tempQuests[row,3])
			);
		}
	}

	public void loadDialogue()
	{
		string[,] tempDialogues = readCSV.getMultiDimCSVData("./Assets/Resources/CSV/GameData/dialogue.csv");

		for(int row = 0; row < tempDialogues.GetLength (0); row ++)
		{
			createDialogue.createDialogue
			(
				dialogueFolder
				,int.Parse (tempDialogues[row,0])
				,tempDialogues[row,1]
				,convertData.convertStringToBool(tempDialogues[row,2])
				,convertData.convertStringtoListString(tempDialogues[row,6])
				,convertData.convertStringToBool(tempDialogues[row,3])
				,convertData.convertStringtoListString(tempDialogues[row,4])
				,convertData.convertStringtoListInt(tempDialogues[row,5])
			);
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

		string[,] tempCharacterClasses = readCSV.getMultiDimCSVData("./Assets/Resources/CSV/GameData/character_classes.csv");
		
		for(int row = 0; row < tempCharacterClasses.GetLength (0); row ++)
		{
			createCharacterClass.createCharacterClass
			(
				characterClassFolder
				,tempCharacterClasses[row,1]
				,int.Parse (tempCharacterClasses[row,0])
				,int.Parse (tempCharacterClasses[row,2])
				,int.Parse (tempCharacterClasses[row,3])
				,int.Parse (tempCharacterClasses[row,4])
				,int.Parse (tempCharacterClasses[row,5])
				,int.Parse (tempCharacterClasses[row,6])
				,convertData.convertStringtoListInt(tempCharacterClasses[row,7])
			);
		}
	}

	public void loadCharacters()
	{
		/*
		GameObject folder
		,string characterName
		,int characterID
		,CharacterClass characterClass
		,List<int> dialogueIDs
		,List<int> questIDPrereqs
		,int level
		,int addMoveSpeed
		,int addAttack
		,int addDefense
		,Item equippedHead
		,Item equippedShirt
		,Item equippedHands
		,Item equippedLegs
		,Item equippedFeet
		,Item equippedWeapon
		 */

		string[,] tempCharacters = readCSV.getMultiDimCSVData("./Assets/Resources/CSV/Save Data/characters.csv");
		
		for(int row = 0; row < tempCharacters.GetLength (0); row ++)
		{
			createCharacter.createCharacter
			(
				characterFolder
				,tempCharacters[row,1]
				,int.Parse (tempCharacters[row,0])
				,characterClasses[int.Parse (tempCharacters[row,2])]
				,convertData.convertStringtoListInt(tempCharacters[row,3])
				,convertData.convertStringtoListInt(tempCharacters[row,4])
				,int.Parse (tempCharacters[row,5])
				,int.Parse (tempCharacters[row,6])
				,int.Parse (tempCharacters[row,7])
				,int.Parse (tempCharacters[row,8])
				,int.Parse (tempCharacters[row,9])
				,items[int.Parse (tempCharacters[row,10])]
				,items[int.Parse (tempCharacters[row,11])]
				,items[int.Parse (tempCharacters[row,12])]
				,items[int.Parse (tempCharacters[row,13])]
				,items[int.Parse (tempCharacters[row,14])]
				,items[int.Parse (tempCharacters[row,15])]
			);
		}
	}

	public void loadCharacterGameObjectsForTurnBasedBattle()
	{
		foreach(int playerCharacterID in playerData.partyCharacterIDs)
		{
			runTurnBasedBattle.playerParty.Add 
			(
				createCharacterGameObject.createCharacterGameObject
				(
					cal
					,characterGameObjectFolder
					,cells[1,2].transform.position
					,characters[playerCharacterID]
					,"NPC"
					,"Friendly"
					,"Enemy"
					,false
					,2
					,2
					,new List<string>()
					,true
				)
			);
		}
		
		runTurnBasedBattle.enemyParty.Add 
		(
			createCharacterGameObject.createCharacterGameObject
			(
				cal
				, characterGameObjectFolder
				, cells [1, 9].transform.position
				, characters [1]
				, "NPC"
				, "Enemy"
				, "Friendly"
				, false
				, 1
				, 9
				, new List<string> ()
				, true
			)
		);

		runTurnBasedBattle.allParties.AddRange (runTurnBasedBattle.playerParty);
		runTurnBasedBattle.allParties.AddRange (runTurnBasedBattle.enemyParty);

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
			,"NPC"
			,"Faction - Side 1"
			,"Faction - Side 2"
			,false
			,0
			,0
			,new List<string>()
			,true
		);

		createCharacterGameObject.createCharacterGameObject
		(
			cal
			,characterGameObjectFolder
			,cells[3,6].transform.position
			,characters[1]
			,"NPC"
			,"Faction - Side 2"
			,"Faction - Side 1"
			,false
			,3
			,6
			,new List<string>()
			,true
		);

		createCharacterGameObject.createCharacterGameObject
		(
			cal
			,characterGameObjectFolder
			,cells[3,3].transform.position
			,characters[2]
			,"NPC"
			,"Faction - Side 2"
			,"Faction - Side 1"
			,false
			,3
			,3
			,new List<string>()
			,true
		);
	}

	public void loadCells(string currentLocation)
	{
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
			,""
			,readCellData
			,createCharacterGameObject
		);

		createCells.addNeighborsToCells();
	}

	public void loadPlayer()
	{
		int row = int.Parse (readCSV.getMultiDimCSVData ("./Assets/Resources/CSV/Save Data/playerData.csv") [0, 1]);
		int col = int.Parse (readCSV.getMultiDimCSVData ("./Assets/Resources/CSV/Save Data/playerData.csv") [0, 2]);
		
		Vector3 pos = GameObject.Find ("Cell_" + row + "_" + col).transform.position;

		player = createCharacterGameObject.createCharacterGameObject
		(
			cal
			, characterGameObjectFolder
			, pos
			, characters [playerCharacterID]
			, "Player"
			, "Good"
			, "Bad"
			, true
			, row
			, col
			,new List<string>()
			,false
		);

		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<SmoothCamera2D> ().target = player.transform;
	}

	public void loadBattle()
	{
		createBattle.calculateBattleOrder
		(
			characterGameObjects
		);

	}
}
