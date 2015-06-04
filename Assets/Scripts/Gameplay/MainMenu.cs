using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour 
{
	public AllData allData;
	public GameObject mainMenuCanvas;
	public GameObject mainMenuButtonPanel;
	public List<GameObject> mainMenuButtons = new List<GameObject>();
	public GameObject mainMenuClassPanel;
	public List<GameObject> mainMenuClassButtons = new List<GameObject>();
	public EventSystem eventSystem;
	public GameObject characterButtonPanel;
	public List<GameObject> characterButtons = new List<GameObject>();
	public GameObject characterNameInput;

	public int currentButton;

	public int currentCharacter;
	public int currentClass;

	public mainMenuState currentState;

	public enum mainMenuState
	{
		MainMenu
		,SelectParty
		,EnterCharacterName
		,ChooseCharacterClass
		,StartGame
		,Options
		,LoadGame
		,QuitGame
	}

	void Update()
	{
		if (allData.finishedLoading) 
		{
			getCurrentAction();
		}
	}

	public void getCurrentAction()
	{
		switch (currentState) 
		{
		case mainMenuState.MainMenu:
		{
			runMainMenu();
			break;
		}
		case mainMenuState.SelectParty:
		{
			selectParty ();
			break;
		}
		case mainMenuState.EnterCharacterName:
		{
			if(enterCharacterName())
			{
				currentState = mainMenuState.ChooseCharacterClass;
			}
			break;
		}
		case mainMenuState.ChooseCharacterClass:
		{
			if(chooseCharacterClass())
			{
				currentState = mainMenuState.SelectParty;
			}
			break;
		}
		case mainMenuState.StartGame:
		{

			allData.saveData.savePlayerData(allData.playerData,22,22,"Gwain","Overworld","Gwain",allData);
			Application.LoadLevel("Battle");
			break;
		}
		case mainMenuState.LoadGame:
		{
			break;
		}
		case mainMenuState.Options:
		{
			break;
		}
		case mainMenuState.QuitGame:
		{

			break;
		}
		default:
		{
			break;
		}
		}
	}

	bool runMainMenu()
	{
		string keyPressed = Input.inputString;
		eventSystem.SetSelectedGameObject(mainMenuButtons[currentButton]);
		handleInput(mainMenuButtons.Count - 1);

		if (keyPressed == "e") 
		{
			switch(currentButton)
			{
			case 0:
			{
				mainMenuButtonPanel.SetActive (false);
				currentButton = 0;
				characterButtonPanel.SetActive(true);
				setupCharacterCard(0);
				setupCharacterCard(1);
				setupCharacterCard(2);
				setupCharacterCard(3);
				currentState = mainMenuState.SelectParty;
				break;
			}
			default:
			{
				break;
			}
			}
		}

		return true;
	}

	void setupCharacterCard(int characterID)
	{
		characterButtons [characterID].transform.FindChild ("CharacterName").GetComponent<Text> ().text = allData.characters [characterID].characterName;
		characterButtons [characterID].transform.FindChild ("Class").GetComponent<Text> ().text = allData.characters [characterID].characterClass.charClassName;
		characterButtons [characterID].transform.FindChild ("StatsPanel").transform.FindChild ("Health").GetComponent<Text>().text = "Health: " + allData.characters [characterID].totalHealth().ToString ();
		characterButtons [characterID].transform.FindChild ("StatsPanel").transform.FindChild ("Attack").GetComponent<Text>().text = "Attack: " + allData.characters [characterID].attack().ToString ();
		characterButtons [characterID].transform.FindChild ("StatsPanel").transform.FindChild ("Defense").GetComponent<Text>().text = "Defense: " + allData.characters [characterID].defense().ToString ();

	}

	void selectParty()
	{
		string keyPressed = Input.inputString;
		eventSystem.SetSelectedGameObject(characterButtons[currentCharacter]);
		handleInput(characterButtons.Count - 1);
		currentCharacter = currentButton;

		if (keyPressed == "e") 
		{
			characterNameInput.SetActive (true);
			eventSystem.SetSelectedGameObject(characterNameInput.transform.FindChild ("InputField").gameObject);
			currentButton = 0;
			characterNameInput.transform.FindChild ("InputField").GetComponent<InputField>().text = allData.characters[currentCharacter].characterName;
			currentState = mainMenuState.EnterCharacterName;
		}

		if (Input.GetKey (KeyCode.Return)) 
		{
			currentState = mainMenuState.StartGame;
		}
	}

	bool enterCharacterName()
	{
		if (Input.GetKey (KeyCode.Return)) 
		{
			allData.characters[currentCharacter].characterName = characterNameInput.transform.FindChild ("InputField").GetComponent<InputField>().text;
			setupCharacterCard(currentCharacter);
			characterNameInput.SetActive (false);
			mainMenuClassPanel.SetActive (true);
			return true;
		} 
		else 
		{
			return false;
		}
	}

	bool chooseCharacterClass()
	{
		string keyPressed = Input.inputString;
		eventSystem.SetSelectedGameObject(mainMenuClassButtons[currentClass]);
		handleInput(mainMenuClassButtons.Count - 1);
		currentClass = currentButton;
		
		if (keyPressed == "e") 
		{
			allData.characters[currentCharacter].characterClass = allData.characterClasses[currentClass];
			setupCharacterCard(currentCharacter);
			mainMenuClassPanel.SetActive (false);
			return true;
		}
		else
		{
			return false;
		}
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
}
