using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class RunOverworld : MonoBehaviour 
{
	public AllData allData;
	public Calendar cal;
	public GameObject player;
	public CharacterGameObject playerCharacterGameObject;
	public PermanentData permanentData;
	public int currentDialogueIndex;
	public GameObject dialogueTarget;
	public GameObject dialoguePanel;
	public GameObject dialogueOptionPanel;
	public List<GameObject> dialogueOptionButtons = new List<GameObject>();
	public GameObject InventoryCanvas;
	public GameObject InventoryList;
	public GameObject InventoryListButtons;
	public GameObject InventoryListButton;
	public GameObject ItemCount;
	public GameObject ItemName;
	public GameObject ItemDesc;
	public EventSystem eventSystem;

	List<GameObject> temporaryGameObjects = new List<GameObject>();

	public int currentButton = 0;

	public OverworldState currentState;

	public enum OverworldState
	{
		Idle
		,Moving
		,SetupDialogueWithNPC
		,DialogueWithEnvironment
		,Shopping
		,SetupInventory
		,Inventory
		,GoingThroughDoor
		,DoDialogueWithNPC
	}

	void Update()
	{
		if (allData.finishedLoading) 
		{
			getCurrentAction();
		}
	}

	void getCurrentAction()
	{
		switch (currentState) 
		{
		case OverworldState.Idle:
		{
			handleInput ();
			break;
		}
		case OverworldState.Moving:
		{
			if(playerMoveAlongPath(playerCharacterGameObject))
			{
				currentState = OverworldState.Idle;
			}
			break;
		}
		case OverworldState.GoingThroughDoor:
		{
			Application.LoadLevel("Battle");
			break;
		}
		case OverworldState.DialogueWithEnvironment:
		{
			if(playerInDialogueWithEnvironment ())
			{
				currentState = OverworldState.Idle;
			}
			break;
		}
		case OverworldState.SetupDialogueWithNPC:
		{
			setupPlayerNPCDialogue();
			break;
		}
		case OverworldState.DoDialogueWithNPC:
		{
			if(playerInDialogueWithNPC ())
			{
				currentState = OverworldState.Idle;
			}
			break;
		}
		case OverworldState.SetupInventory:
		{
			setupInventory();
			break;
		}
		case OverworldState.Inventory:
		{
			if(playerInInventory())
			{
				currentState = OverworldState.Idle;
			}
			break;
		}
		default:
		{
			break;
		}
		}
	}

	void handleInput()
	{
		string keyPressed = Input.inputString;
		
		if (keyPressed.Length > 0) 
		{
			GameObject currentCell = GameObject.Find ("Cell_" + (playerCharacterGameObject.row) + "_" + playerCharacterGameObject.col);
			GameObject dest = currentCell;
			GameObject targetCell = currentCell;
			CharacterGameObject.dir oppositeDir = CharacterGameObject.dir.North;

			switch(keyPressed)
			{
			case "s":
			{
				dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row + 1) + "_" + playerCharacterGameObject.col);
				playerCharacterGameObject.currentDir = CharacterGameObject.dir.South;
				break;
			}
			case "w":
			{
				dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row - 1) + "_" + playerCharacterGameObject.col);
				playerCharacterGameObject.currentDir = CharacterGameObject.dir.North;
				break;
			}
			case "a":
			{
				dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row) + "_" + (playerCharacterGameObject.col - 1));
				playerCharacterGameObject.currentDir = CharacterGameObject.dir.West;
				break;
			}
			case "d":
			{
				dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row) + "_" + (playerCharacterGameObject.col + 1));
				playerCharacterGameObject.currentDir = CharacterGameObject.dir.East;
				break;
			}
			case "o":
			{
				allData.saveData.savePlayerData(allData.playerData,playerCharacterGameObject.row,playerCharacterGameObject.col,permanentData.currentLevel,"Turn_Based_Battle","Wood Floor Arena",allData);
				permanentData.currentLevel = "Wood Floor Arena";
				Application.LoadLevel("Battle");
				break;
			}
			case "e":
			{
				switch(playerCharacterGameObject.currentDir)
				{
				case CharacterGameObject.dir.North:
				{
					targetCell = GameObject.Find ("Cell_" + (playerCharacterGameObject.row - 1) + "_" + (playerCharacterGameObject.col));
					oppositeDir = CharacterGameObject.dir.South;
					break;
				}
				case CharacterGameObject.dir.South:
				{
					targetCell = GameObject.Find ("Cell_" + (playerCharacterGameObject.row + 1) + "_" + (playerCharacterGameObject.col));
					oppositeDir = CharacterGameObject.dir.North;
					break;
				}
				case CharacterGameObject.dir.West:
				{
					targetCell = GameObject.Find ("Cell_" + (playerCharacterGameObject.row) + "_" + (playerCharacterGameObject.col - 1));
					oppositeDir = CharacterGameObject.dir.East;
					break;
				}
				case CharacterGameObject.dir.East:
				{
					targetCell = GameObject.Find ("Cell_" + (playerCharacterGameObject.row) + "_" + (playerCharacterGameObject.col + 1));
					oppositeDir = CharacterGameObject.dir.West;
					break;
				}
				default:
				{
					break;
				}
				}
				break;
			}
			case "t":
			{
				currentState = OverworldState.SetupInventory;
				break;
			}
			default:
			{
				break;
			}
			}

			if (dest != currentCell) 
			{
				if(dest.GetComponent<Cell>().isWalkable)
				{
					playerCharacterGameObject.path.Add (dest);
					playerCharacterGameObject.currentAnimation ("walk");
					currentState = OverworldState.Moving;
				}
				else
				{
					playerCharacterGameObject.currentAnimation ("idle");
				}
            }

			if(targetCell != currentCell)
			{
				Cell cell = targetCell.GetComponent<Cell>();

				if(cell != null)
				{
					if(cell.hasDialogue == true)
					{
						currentDialogueIndex = 0;
						dialogueTarget = cell.gameObject;
						currentState = OverworldState.DialogueWithEnvironment;
					}

					if(cell.hasNPC == true)
					{
						currentDialogueIndex = 0;
						dialogueTarget = cell.NPC;
						CharacterGameObject npc = dialogueTarget.GetComponent<CharacterGameObject>();
						npc.currentDir = oppositeDir;
						npc.currentAnimation ("idle");
						currentDialogueIndex = npc.character.currentDialogueIndex(allData.quests);
						currentState = OverworldState.SetupDialogueWithNPC;
					}
				}
			}
		}
	}

	void setupInventory()
	{
		InventoryCanvas.SetActive(true);

		Dictionary<int,int> tempItemIDs = allData.playerData.itemIDs;

		for(int i = 0; i < tempItemIDs.Keys.Count; i++)
		{
			if(i == 0)
			{
				ItemCount.GetComponentInChildren<Text>().text = tempItemIDs[i].ToString ();
				ItemName.GetComponentInChildren<Text>().text = allData.items[tempItemIDs.Keys.ToList ()[i]].itemName;
				ItemDesc.GetComponentInChildren<Text>().text = allData.items[tempItemIDs.Keys.ToList ()[i]].desc;
			}
			else
			{
				GameObject newItemCount = Instantiate(ItemCount);
				newItemCount.name = "ItemCount"+i;
				newItemCount.transform.parent = InventoryList.transform;

				GameObject newItemName = Instantiate(ItemName);
				newItemName.name = "ItemName"+i;
				newItemName.transform.parent = InventoryList.transform;

				GameObject newItemDesc = Instantiate(ItemDesc);
				newItemDesc.name = "ItemName"+i;
				newItemDesc.transform.parent = InventoryList.transform;

				GameObject newItemButton = Instantiate(InventoryListButtons);
				newItemButton.name = "InventoryListButton"+i;
				newItemButton.transform.parent = InventoryListButtons.transform;

				newItemCount.GetComponentInChildren<Text>().text = tempItemIDs[i].ToString ();
				newItemName.GetComponentInChildren<Text>().text = allData.items[tempItemIDs.Keys.ToList ()[i]].itemName;
				newItemDesc.GetComponentInChildren<Text>().text = allData.items[tempItemIDs.Keys.ToList ()[i]].desc;

				temporaryGameObjects.Add (newItemCount);
				temporaryGameObjects.Add (newItemName);
				temporaryGameObjects.Add (newItemDesc);
				temporaryGameObjects.Add (newItemButton);
			}
		}

		currentState = OverworldState.Inventory;
	}

	bool playerInInventory()
	{
		string keyPressed = Input.inputString;
		
		if (keyPressed == "t") 
		{
			foreach(GameObject temporaryGameObject in temporaryGameObjects)
			{
				Destroy(temporaryGameObject);
			}

			InventoryCanvas.SetActive(false);
			return true;
		}
		else
		{

			eventSystem.SetSelectedGameObject(dialogueOptionButtons[currentButton]);
			handleInput(allData.dialogues[currentDialogueIndex].options.Count - 1);
			return false;
		}
	}

	void setupPlayerNPCDialogue()
	{
		currentButton = 0;
	
		CharacterGameObject npc = dialogueTarget.GetComponent<CharacterGameObject> ();
		dialoguePanel.SetActive(true);
		dialoguePanel.GetComponentInChildren<Text> ().text = allData.dialogues [currentDialogueIndex].text;

		if(allData.dialogues[currentDialogueIndex].hasOptions)
		{
			dialogueOptionPanel.SetActive(true);

			switch(allData.dialogues[currentDialogueIndex].options.Count)
			{
			case 1:
			{
				dialogueOptionButtons[0].SetActive (true);
				dialogueOptionButtons[0].GetComponentInChildren<Text>().text = allData.dialogues [currentDialogueIndex].options[0];
				//dialogueOptionButtons[1].SetActive (false);
				//dialogueOptionButtons[3].SetActive (false);
				break;
			}
			case 2:
			{
				dialogueOptionButtons[0].SetActive (true);
				dialogueOptionButtons[0].GetComponentInChildren<Text>().text = allData.dialogues [currentDialogueIndex].options[0];
				dialogueOptionButtons[1].SetActive (true);
				dialogueOptionButtons[1].GetComponentInChildren<Text>().text = allData.dialogues [currentDialogueIndex].options[1];
				//dialogueOptionButtons[3].SetActive (false);
				break;
			}
			case 3:
			{
				dialogueOptionButtons[0].SetActive (true);
				dialogueOptionButtons[1].SetActive (true);
				dialogueOptionButtons[3].SetActive (true);
				break;
			}
			default:
			{
				break;
			}
			}
		}

		currentState = OverworldState.DoDialogueWithNPC;
	}

	bool playerInDialogueWithNPC()
	{
		string keyPressed = Input.inputString;
		eventSystem.SetSelectedGameObject(dialogueOptionButtons[currentButton]);
		if(allData.dialogues[currentDialogueIndex].hasOptions)
		{
			handleInput(allData.dialogues[currentDialogueIndex].options.Count - 1);
		}
		CharacterGameObject npc = dialogueTarget.GetComponent<CharacterGameObject> ();

		if (keyPressed != "e") 
		{
			return false;
		}
		else 
		{
			//check to see if dialogue id has actions. if so, check what those actions are and apply them.
			if(allData.dialogues[currentDialogueIndex].hasOptions)
			{
				currentDialogueIndex = allData.dialogues[currentDialogueIndex].optionDests[currentButton];

				dialogueOptionButtons[0].SetActive (false);
				dialogueOptionButtons[1].SetActive (false);
				dialogueOptionButtons[2].SetActive (false);
				dialogueOptionPanel.SetActive (false);

				currentState = OverworldState.SetupDialogueWithNPC;

				return false;
			}
			else
			{
				bool endDialogue = false;

				if(allData.dialogues[currentDialogueIndex].hasActions)
				{
					endDialogue = doDialogueAction(allData.dialogues[currentDialogueIndex].actions);
				}
				
				dialoguePanel.SetActive (false);
				dialogueOptionButtons[0].SetActive (false);
				dialogueOptionButtons[1].SetActive (false);
				dialogueOptionButtons[2].SetActive (false);

				if(endDialogue)
				{
					return true;
				}
				else
				{
					currentState = OverworldState.SetupDialogueWithNPC;
					return false;
				}
			}
		}
	}

	bool doDialogueAction(List<string> actions)
	{
		bool closeDialogue = false;

		foreach(string action in actions)
		{
			switch(getActionType(action))
			{
			case "questCompleted":
			{
				int questID = int.Parse (getActionValue(action,0));
				allData.quests[questID].isComplete = true;
				break;
			}
			case "goToDialogueID":
			{
				currentDialogueIndex = int.Parse (getActionValue (action,0));
				break;
			}
			case "endDialogue":
			{
				closeDialogue = true;
				break;
			}
			default:
			{
				break;
			}
			}
		}

		return closeDialogue;
	}

	string getActionType(string action)
	{
		int index = action.IndexOf (":");
		string actionType = action.Substring (0,index);

		return actionType;
	}

	string getActionValue(string action, int actionValueIndex)
	{
		int index = action.IndexOf (":");
		string actionValue = action.Substring (index+1,action.Length-index-1);

		return actionValue;
	}

	void handleInput(int maxOptions)
	{
		string keyPressed = Input.inputString;

		if(keyPressed.Length > 0)
		{
			switch(keyPressed)
			{
			case "w":
			{
				currentButton--;
				break;
			}
			case "s":
			{
				currentButton++;
				break;
			}
			default:
			{
				break;
			}
			}

			if(currentButton > maxOptions)
			{
				currentButton = 0;
			}

			if(currentButton < 0)
			{
				currentButton = maxOptions;
			}

			currentButton = Mathf.Clamp(currentButton,0,maxOptions);
		}
	}

	bool playerInDialogueWithEnvironment()
	{
		string keyPressed = Input.inputString;
		Cell cell = dialogueTarget.GetComponent<Cell> ();
		dialoguePanel.SetActive (true);
		dialoguePanel.transform.FindChild ("DialogueText").GetComponent<Text> ().text = cell.dialogue [currentDialogueIndex];

		if (keyPressed.Length == 0) 
		{
			return false;
		}
		else 
		{
			if(currentDialogueIndex < cell.dialogue.Count()-1)
			{
				currentDialogueIndex++;
				keyPressed = "";
				return false;
			}
			else
			{
				dialoguePanel.SetActive(false);
				return true;
			}
		}
	}

	bool playerMoveAlongPath(CharacterGameObject currentCharacter)
	{
		Transform currentCharacterTransform = allData.player.transform;
		float maxDistanceDelta = Time.deltaTime * cal.timeSpeed * 1.25f;

		Cell destCell = currentCharacter.path [0].GetComponent<Cell> ();

		if (!destCell.isDoor) 
		{
			if ((currentCharacterTransform.position - currentCharacter.path [0].transform.position).sqrMagnitude > .001f) 
			{
				currentCharacterTransform.position = Vector3.MoveTowards (currentCharacterTransform.position, currentCharacter.path [0].transform.position, maxDistanceDelta);
				return false;
			}
			else 
			{
				currentCharacterTransform.position = currentCharacter.path [0].transform.position;

				GameObject.Find ("Cell_" + currentCharacter.row + "_" + currentCharacter.col).GetComponent<Cell> ().isWalkable = true;
				GameObject.Find ("Cell_" + currentCharacter.path [0].GetComponent<Cell> ().row + "_" + currentCharacter.path [0].GetComponent<Cell> ().col).GetComponent<Cell> ().isWalkable = false;
				
				currentCharacter.row = currentCharacter.path [0].GetComponent<Cell> ().row;
				currentCharacter.col = currentCharacter.path [0].GetComponent<Cell> ().col;
				currentCharacter.currentAnimation ("idle");

				currentCharacter.path.Clear ();
				return true;
			}
		}
		else 
		{
			if ((currentCharacterTransform.position - currentCharacter.path [0].transform.position).sqrMagnitude > .05f)
			{
				currentCharacterTransform.position = Vector3.MoveTowards (currentCharacterTransform.position, currentCharacter.path [0].transform.position, maxDistanceDelta);
				return false;
			}
			else 
			{
				GameObject.Find ("Cell_" + currentCharacter.row + "_" + currentCharacter.col).GetComponent<Cell> ().isWalkable = true;
				destCell.isWalkable = false;
				
				currentCharacter.row = currentCharacter.path [0].GetComponent<Cell> ().row;
				currentCharacter.col = currentCharacter.path [0].GetComponent<Cell> ().col;
				
				currentCharacter.path.Clear ();

				permanentData.currentLevel = destCell.doorLevel;
				permanentData.currentDoorNum = destCell.doorNum;

				allData.saveData.saveData(allData.quests);
				allData.saveData.savePlayerData(allData.playerData,destCell.destx,destCell.desty,permanentData.currentLevel,"Overworld",permanentData.currentLevel,allData);

				currentState = OverworldState.GoingThroughDoor;

				return false;
			}
		}
	}
}
