using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RunTurnBasedBattle : MonoBehaviour 
{
	public AllData allData;

	public List<GameObject> playerParty = new List<GameObject>();
	public List<GameObject> enemyParty = new List<GameObject>();
	public List<GameObject> allParties = new List<GameObject> ();

	public GameObject battleCanvas;
	public List<GameObject> battleOptionButtons = new List<GameObject>();
	public GameObject actionLog;
	public List<GameObject> characterNames = new List<GameObject>();
	public List<GameObject> characterHealths = new List<GameObject>();
	public GameObject abilityPanel;
	public List<GameObject> abilityButtons = new List<GameObject> ();
	public GameObject currentTurnLabel;
	public EventSystem eventSystem;

	public int currentButton = 0;
	public int currentTargetID = 0;
	public int currentPlayerTurn = 0;
	public int currentAbilitySelected = 0;

	public enum battleState
	{
		PickingAction
		,PickingAbility
		,SelectingTarget
		,PerformingAction
		,BattleWon
	}

	public battleState currentState = battleState.PickingAction;

	public enum UIFocus
	{
		SelectAction
		,SelectAbility
		,SelectItem
	}

	public UIFocus currentUIFocus = UIFocus.SelectAction;

	public enum inputType
	{
		verticalOnly
		,horizontalOnly
		,grid
	}

	void Update()
	{
		runBattle ();
	}

	void runBattle()
	{
		switch(currentState)
		{
		case battleState.PickingAction:
		{
			selectAction();
			break;
		}
		case battleState.PickingAbility:
		{
			selectAbility();
			break;
		}
		case battleState.SelectingTarget:
		{
			selectTarget();
			break;
		}
		case battleState.PerformingAction:
		{
			performAction();
			break;
		}
		case battleState.BattleWon:
		{
			int row = int.Parse (allData.readCSV.getMultiDimCSVData ("./Assets/Resources/CSV/Save Data/playerData.csv") [0, 1]);
			int col = int.Parse (allData.readCSV.getMultiDimCSVData ("./Assets/Resources/CSV/Save Data/playerData.csv") [0, 2]);

			allData.permanentData.currentLevel = "Gwain";

			allData.saveData.savePlayerData(allData.playerData,row,col,"Wood Floor Arena","Overworld","Gwain");

			Application.LoadLevel("Battle");
			break;
		}
		default:
		{
			break;
		}
		}
	}

	public void setupCharacterNamesandHealths(AllData allData)
	{
		for(int i = 0; i < 4; i ++)
		{
			if(i < allData.playerData.partyCharacterIDs.Count)
			{
				characterNames[i].transform.GetComponentInChildren<Text>().text = allData.characters[allData.playerData.partyCharacterIDs[i]].characterName;
				characterHealths[i].transform.GetComponentInChildren<Text>().text = allData.characters[allData.playerData.partyCharacterIDs[i]].totalHealth().ToString ();
			}
			else
			{
				characterNames[i].SetActive (false);
				characterHealths[i].SetActive (false);
			}
		}
	}

	void selectAction()
	{
		string keyPressed = Input.inputString;

		if (keyPressed == "e") 
		{
			switch(currentButton)
			{
			case 0:
			{
				abilityPanel.SetActive (true);
				setupAbilityOptions();
				currentState = battleState.PickingAbility;
				break;
			}
			default:
			{
				break;
			}
			}
		}

		eventSystem.SetSelectedGameObject(battleOptionButtons[currentButton]);
		currentButton = handleInput(currentButton, battleOptionButtons.Count - 1, 3, 1);
	}

	void selectAbility()
	{
		string keyPressed = Input.inputString;
		
		if (keyPressed == "e") 
		{
			abilityPanel.SetActive (false);
			currentAbilitySelected = currentButton;
			currentButton = 0;
			allParties [currentButton].transform.FindChild ("battle_sprite_bg").GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
			currentState = battleState.SelectingTarget;
		}
		
		eventSystem.SetSelectedGameObject(abilityButtons[currentButton]);
		currentButton = handleInput(currentButton, allData.characters[allData.playerData.partyCharacterIDs[currentPlayerTurn]].characterClass.abilityIDs.Count - 1, 2, 2);
	}

	void setupAbilityOptions()
	{
		for(int i = 0; i < 4; i ++)
		{
			if(i < allData.characters[allData.playerData.partyCharacterIDs[currentPlayerTurn]].characterClass.abilityIDs.Count)
			{
				abilityButtons[i].transform.GetComponentInChildren<Text>().text = allData.abilities[allData.characters[allData.playerData.partyCharacterIDs[currentPlayerTurn]].characterClass.abilityIDs[i]].abilityName;
			}
			else
			{
				abilityButtons[i].SetActive (false);
			}
		}
	}

	void selectTarget()
	{
		int tempIndex = handleInput(currentButton, allParties.Count - 1, allParties.Count - 1, 1);

		if (tempIndex != currentButton)
		{
			allParties [currentButton].transform.FindChild ("battle_sprite_bg").GetComponent<Image> ().color = new Color32 (255, 255, 255, 0);
			allParties [tempIndex].transform.FindChild ("battle_sprite_bg").GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);

			currentButton = tempIndex;
		}

		string keyPressed = Input.inputString;
		
		if (keyPressed == "e")
		{
			allParties [currentButton].transform.FindChild ("battle_sprite_bg").GetComponent<Image> ().color = new Color32 (255, 255, 255, 0);
			currentTargetID = currentButton;
			currentButton = 0;
			currentState = battleState.PerformingAction;
		}
	}

	void performAction()
	{
		doAbility (allParties [currentPlayerTurn].GetComponent<CharacterGameObject> ().character.characterClass.abilityIDs [currentAbilitySelected], allParties [currentPlayerTurn], allParties [currentTargetID]);
	}

	void doAbility(int abilityID, GameObject attacker, GameObject target)
	{
		Debug.Log (attacker.GetComponent<CharacterGameObject>().character.characterName 
		           + " attacks " + target.GetComponent<CharacterGameObject>().character.characterName 
		           + " with " + allData.abilities [abilityID].abilityName);

		foreach (int abilityEffect in allData.abilities[abilityID].abilityEffectIDs) 
		{
			switch (abilityEffect) 
			{
			case 0:
			{
				doDamage (attacker.GetComponent<CharacterGameObject>(),target.GetComponent<CharacterGameObject>());
				break;
			}
			default:
			{
				break;
			}
			}
		}

		currentState = battleState.BattleWon;
	}

	void doDamage(CharacterGameObject attacker, CharacterGameObject target)
	{
		int targetHealth = target.currentHealth;
		int damage = calcDamage(attacker, target);
		
		if(targetHealth > damage)
		{
			target.currentHealth -= damage;
		}
		else
		{
			target.currentHealth = 0;
			print ("Target dead!");
		}		
	}

	public int calcDamage(CharacterGameObject currentCharacter, CharacterGameObject target)
	{
		int actualAttack = (int) UnityEngine.Random.Range (0,currentCharacter.character.attack());
		int damage = Mathf.Max (actualAttack - target.character.defense(),0);
		
		Debug.Log (currentCharacter.name + " attacks " + target.name + " for " + damage + " damage.");
		
		return damage;
	}

	int handleInput(int index, int maxOptions, int noOfRows, int noOfColumns)
	{
		int returnInt = index;

		string keyPressed = Input.inputString;
	
		if (keyPressed.Length > 0) 
		{
			switch (keyPressed) 
			{
			case "w":
				{
					returnInt--;
					break;
				}
			case "s":
				{
					returnInt++;
					break;
				}
			case "a":
				{
					returnInt--;
					break;
				}
			case "d":
				{
					returnInt++;
					break;
				}
			default:
				{
					break;
				}
			}
		
			if (returnInt > maxOptions) 
			{
				returnInt = 0;
			}
		
			if (returnInt < 0) 
			{
				returnInt = maxOptions;
			}
	
			returnInt = Mathf.Clamp (returnInt, 0, maxOptions);

			return returnInt;
		} 
		else 
		{
			return index;
		}
	}
}
