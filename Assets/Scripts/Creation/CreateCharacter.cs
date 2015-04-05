using UnityEngine;
using System.Collections;

public class CreateCharacter : MonoBehaviour 
{
	public AllData allData;

	public void createCharacter
	(
		GameObject folder
		,string characterName
		,int characterID
		,CharacterClass characterClass
	)
	{
		Character newCharacter = folder.AddComponent<Character>();

		newCharacter.characterName = characterName;
		newCharacter.characterID = characterID;
		newCharacter.characterClass = characterClass;

		allData.characters.Add (newCharacter);
	}
}
