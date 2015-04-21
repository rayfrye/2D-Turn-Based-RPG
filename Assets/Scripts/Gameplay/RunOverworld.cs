using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RunOverworld : MonoBehaviour 
{
	public AllData allData;
	public Calendar cal;
	public GameObject player;
	public CharacterGameObject playerCharacterGameObject;
	public PermanentData permanentData;
	public int currentDialogueIndex;
	public GameObject dialogueTarget;
	public Canvas DialogueCanvas;

	public OverworldState currentState;

	public enum OverworldState
	{
		Idle
		,Moving
		,DialogueWithNPC
		,DialogueWithEnvironment
		,Shopping
		,Inventory
		,GoingThroughDoor
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
			movementInput ();
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
		case OverworldState.DialogueWithNPC:
		{
			if(playerInDialogueWithNPC ())
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

	void movementInput()
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
						currentState = OverworldState.DialogueWithNPC;
					}
				}
			}
		}
	}

	bool playerInDialogueWithNPC()
	{
		string keyPressed = Input.inputString;
		CharacterGameObject npc = dialogueTarget.GetComponent<CharacterGameObject> ();
		DialogueCanvas.enabled = true;
		DialogueCanvas.GetComponentInChildren<Text> ().text = npc.dialogue [currentDialogueIndex];
		
		if (keyPressed.Length == 0) 
		{
			return false;
		} 
		else 
		{
			if(currentDialogueIndex < npc.dialogue.Count()-1)
			{
				currentDialogueIndex++;
				keyPressed = "";
				return false;
			}
			else
			{
				DialogueCanvas.enabled = false;
				return true;
			}
		}	}

	bool playerInDialogueWithEnvironment()
	{
		string keyPressed = Input.inputString;
		Cell cell = dialogueTarget.GetComponent<Cell> ();
		DialogueCanvas.enabled = true;
		DialogueCanvas.GetComponentInChildren<Text> ().text = cell.dialogue [currentDialogueIndex];

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
				DialogueCanvas.enabled = false;
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
				//currentCharacterTransform.position = currentCharacter.path [0].transform.position;
				
				GameObject.Find ("Cell_" + currentCharacter.row + "_" + currentCharacter.col).GetComponent<Cell> ().isWalkable = true;
				destCell.isWalkable = false;
				
				currentCharacter.row = currentCharacter.path [0].GetComponent<Cell> ().row;
				currentCharacter.col = currentCharacter.path [0].GetComponent<Cell> ().col;
				
				currentCharacter.path.Clear ();

				permanentData.currentLevel = destCell.doorLevel;
				permanentData.currentDoorNum = destCell.doorNum;

				currentState = OverworldState.GoingThroughDoor;

				return false;
			}
		}
	}
}
