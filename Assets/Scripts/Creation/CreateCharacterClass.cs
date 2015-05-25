using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCharacterClass : MonoBehaviour 
{
	public AllData allData;

	public void createCharacterClass
	(
		GameObject folder
		,string className
		,int classID
		,int moveSpeed
		,int attack
		,int defense
		,int totalHealth
		,int attackRange
		,List<int> abilityIDs
	)
	{
		CharacterClass newCharacterClass = folder.AddComponent<CharacterClass>();

		newCharacterClass.charClassName = className;
		newCharacterClass.charClassID = classID;
		newCharacterClass.moveSpeed = moveSpeed;
		newCharacterClass.attack = attack;
		newCharacterClass.defense = defense;
		newCharacterClass.totalHealth = totalHealth;
		newCharacterClass.attackRange = attackRange;
		newCharacterClass.abilityIDs = abilityIDs;

		allData.characterClasses.Add (newCharacterClass);
	}


}
