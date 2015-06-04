using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveData : MonoBehaviour 
{
	public void saveData(List<Quest> quests)
	{
		saveQuestData(quests);
	}

	public void saveQuestData(List<Quest> quests)
	{
		string[] questData = new string[quests.Count+1];

		questData[0] = "id,name,desc,iscompleted";

		foreach(Quest quest in quests)
		{
			questData[quest.id+1] = quest.id.ToString() + "," + quest.name + "," + quest.description + "," + quest.isComplete.ToString ().ToUpper();
		}

		File.WriteAllLines("./Assets/Resources/CSV/Save Data/quests.csv",questData);
	}

	public void savePlayerData(PlayerData playerData, int savedX, int savedY, string savedArea, string gameState, string destination, AllData allData)
	{
		string[] playerDataString = new string[2];

		playerDataString [0] = "gold,savedX,savedY,savedArea,gameState,destination";
		playerDataString [1] = playerData.gold + "," + savedX.ToString () + "," + savedY.ToString () + "," + savedArea + "," + gameState + "," + destination;

		File.WriteAllLines ("./Assets/Resources/CSV/Save Data/playerData.csv",playerDataString);

		string[] partyIDs = new string[5];
		partyIDs [0] = "partyIDs";

		for (int i = 1; i < 5; i++) 
		{
			partyIDs[i] = playerData.partyCharacterIDs[i-1].ToString ();
		}

		File.WriteAllLines ("./Assets/Resources/CSV/Save Data/playerParty.csv",partyIDs);

		string[] characters = new string[allData.characters.Count+1];
		characters[0] = "id,name,characterClassID,dialogueIDs,dialogueQuestIDPrereqs,level,addMoveSpeed,addAttack,addDefense,addAttackRange,equippedHead,equippedShirt,equippedHands,equippedLegs,equippedFeet,equippedWeapon";
		
		foreach (Character character in allData.characters) 
		{
			string dialogueString = convertListToString (character.dialogueIDs);
			string dialoguePrereqString = convertListToString (character.questIDPrereqs);

			characters[character.characterID+1] = character.characterID.ToString () + "," + character.characterName + "," + character.characterClass.charClassID + "," + dialogueString + "," + dialoguePrereqString + "," + character.characterLevel.ToString() + "," + character.addMoveSpeed.ToString() + "," + character.addAttack.ToString() + "," + character.addDefense.ToString() + "," + character.addAttackRange.ToString() + "," + "0"/*character.equippedHead.id.ToString()*/ + "," + "0"/*character.equippedShirt.id.ToString()*/ + "," + "0"/*character.equippedHands.id.ToString()*/ + "," + "0"/*character.equippedLegs.id.ToString()*/ + "," + "0"/*character.equippedFeet.id.ToString()*/ + "," + "0"/*character.equippedWeapon.id.ToString()*/;
		}
	
		File.WriteAllLines ("./Assets/Resources/CSV/Save Data/characters.csv",characters);	
	}

	string convertListToString(List<int> l)
	{
		string s = "";
		
		for(int i = 0; i < l.Count; i++)
		{
			s = s + "|" + l[i];
		}
		
		
		if(s != "")
		{
			s = s.Substring(1,s.Length-1) + "|";
		}
		
		return s;
	}
}
