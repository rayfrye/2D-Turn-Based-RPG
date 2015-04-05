using UnityEngine;
using System.Collections;

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

		allData.characterClasses.Add (newCharacterClass);
	}


}
