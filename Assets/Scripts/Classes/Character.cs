using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
	public string characterName;
	public int characterID;
	public CharacterClass characterClass;
	public int characterLevel;
	public int moveSpeedModifier;
	public int attackModifier;
	public int defenseModifier;
	public int totalHealthModifier;
	public int attackRangeModifier;


	public int moveSpeed()
	{
		return characterClass.moveSpeed += moveSpeedModifier;
	}

	public int attack()
	{
		return characterClass.attack += attackModifier;
	}

	public int defense()
	{
		return characterClass.defense += defenseModifier;
	}

	public int totalHealth()
	{
		return characterClass.totalHealth += totalHealthModifier;
	}

	public int attackRange()
	{
		return characterClass.attackRange += attackRangeModifier;
	}
}
