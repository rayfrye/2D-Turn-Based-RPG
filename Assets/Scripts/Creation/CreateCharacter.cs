using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCharacter : MonoBehaviour 
{
	public AllData allData;

	public void createCharacter
	(
		GameObject folder
		,string characterName
		,int characterID
		,CharacterClass characterClass
		,List<int> dialogueIDs
		,List<int> questIDPrereqs
	)
	{
		Character newCharacter = folder.AddComponent<Character>();

		newCharacter.characterName = characterName;
		newCharacter.characterID = characterID;
		newCharacter.characterClass = characterClass;

		newCharacter.dialogueIDs = dialogueIDs;
		newCharacter.questIDPrereqs = questIDPrereqs;

		allData.characters.Add (newCharacter);
	}
}
