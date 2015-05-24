using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public List<int> dialogueIDs = new List<int>();
	public List<int> questIDPrereqs = new List<int>();

	public int currentDialogueIndex(List<Quest> quests)
	{
		int currentIndex = 0;

		for(int i = 0; i < questIDPrereqs.Count; i++)
		{
			if(quests[questIDPrereqs[i]].isComplete)
			{
				currentIndex = dialogueIDs[i];
			}
			else
			{
				i = questIDPrereqs.Count;
			}
		}
		
		return currentIndex;
	}

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
