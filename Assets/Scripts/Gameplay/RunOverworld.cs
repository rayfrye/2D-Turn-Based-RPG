using UnityEngine;
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

	public OverworldState currentState;

	public enum OverworldState
	{
		Idle
		,Moving
		,Dialogue
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
			GameObject dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row) + "_" + playerCharacterGameObject.col);

			switch(keyPressed)
			{
			case "s":
			{
				dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row+1) + "_" + playerCharacterGameObject.col);
				playerCharacterGameObject.currentDir = CharacterGameObject.dir.South;
				break;
			}
			case "w":
			{
				dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row-1) + "_" + playerCharacterGameObject.col);
				playerCharacterGameObject.currentDir = CharacterGameObject.dir.North;
				break;
			}
			case "a":
			{
				dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row) + "_" + (playerCharacterGameObject.col-1));
				playerCharacterGameObject.currentDir = CharacterGameObject.dir.East;
				break;
			}
			case "d":
			{
				dest = GameObject.Find ("Cell_" + (playerCharacterGameObject.row) + "_" + (playerCharacterGameObject.col+1));
				playerCharacterGameObject.currentDir = CharacterGameObject.dir.West;
				break;
			}
			default:
			{
				break;
			}
			}

			if (dest != null) 
			{
				if(dest.GetComponent<Cell>().isWalkable)
				{
					playerCharacterGameObject.path.Add (dest);
					currentState = OverworldState.Moving;
				}
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

				allData.currentLevel = destCell.doorLevel;
				allData.currentDoorNum = destCell.doorNum;

				currentState = OverworldState.GoingThroughDoor;

				return false;
			}
		}
	}
}
