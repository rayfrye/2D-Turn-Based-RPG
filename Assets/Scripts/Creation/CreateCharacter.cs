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
		,int level
		,int addMoveSpeed
		,int addAttack
		,int addDefense
		,int addAttackRange
		,Item equippedHead
		,Item equippedShirt
		,Item equippedHands
		,Item equippedLegs
		,Item equippedFeet
		,Item equippedWeapon
	)
	{
		Character newCharacter = folder.AddComponent<Character>();

		newCharacter.characterName = characterName;
		newCharacter.characterID = characterID;
		newCharacter.characterClass = characterClass;

		newCharacter.dialogueIDs = dialogueIDs;
		newCharacter.questIDPrereqs = questIDPrereqs;

		newCharacter.characterLevel = level;
		newCharacter.addMoveSpeed = addMoveSpeed;
		newCharacter.addAttack = addAttack;
		newCharacter.addDefense = addDefense;
		newCharacter.addAttackRange = addAttackRange;
		newCharacter.equippedHead = equippedHead;
		newCharacter.equippedShirt = equippedShirt;
		newCharacter.equippedHands = equippedHands;
		newCharacter.equippedLegs = equippedLegs;
		newCharacter.equippedFeet = equippedFeet;
		newCharacter.equippedWeapon = equippedWeapon;

		allData.characters.Add (newCharacter);
	}
}
